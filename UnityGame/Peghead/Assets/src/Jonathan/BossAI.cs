using System.Collections.Generic;
using UnityEngine;

// This makes sure the boss always has a Rigidbody2D (needed for movement)
[RequireComponent(typeof(Rigidbody2D))]
public class BossAI : MonoBehaviour
{
    [Header("Pathfinding")]
    public AStarGrid pathGrid;        // The grid used for A* pathfinding
    public Transform player;          // The target the boss is chasing
    public float pathAgain = 2f;      // How often the boss should recalculate its path

    [Header("Movement")]
    public float speed = 3f;
    public float waypointThreshold = 0.4f;  // How close it needs to be to a waypoint to move to the next

    [Header("Jumping")]
    public float jumpForce = 5f;          // How high the boss jumps
    public Transform groundCheck;        // Empty object under boss to check if grounded
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;        // What layers count as ground
    public float jumpCooldown = 1.0f;    // Time between jumps

    [Header("Jump Trigger Thresholds")]
    public float verticalPursuitThreshold = 0.1f; // If player or waypoint is this much higher, jump
    public float horizontalJumpThreshold = 0.1f;  // Used to detect platform edges near waypoint

    [Header("Overhead & Oscillation")]
    public float stopOscillationThreshold = 0.2f;  // If directly under the player, don't shuffle back and forth
    public float overheadDetectionRadius = 0.2f;   // Used to detect platforms overhead
    public float clearanceRayDistance = 1.0f;      // How far to check for clearance left/right
    public float clearanceRayYOffset = 1.0f;       // How high to offset those checks

    private Rigidbody2D rb;
    private List<Vector2> path = new List<Vector2>();
    private int currentWaypointIndex = 0;
    private float lastRepathTime = 0f;
    private float lastJumpTime = -Mathf.Infinity;
    private Vector2 currentDirection = Vector2.zero;
    private float lastHorizontalDir = 1f;  // Remember last move direction

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Every few seconds, recalculate the path to the player
        if(Time.time >= lastRepathTime + pathAgain){
            RecalculatePath();
        }

        // Figure out which direction to move
        UpdateDirection();

        // If needed, jump
        if(ShouldJump()) Jump();
    }

    void FixedUpdate()
    {
        // Apply horizontal movement while keeping vertical velocity
        float vy = rb.linearVelocity.y;
        rb.linearVelocity = new Vector2(currentDirection.x * speed, vy);
    }

    private void RecalculatePath()
    {
        lastRepathTime = Time.time;
        List<Vector2> newPath = pathGrid.FindPath(transform.position, player.position);
        Debug.Log("Boss Path found. Count = " + newPath.Count);
        if(newPath != null && newPath.Count > 0){
            // Choose the closest point on the path as the starting point
            int closestIndex = 0;
            float closestDistance = Vector2.Distance(transform.position, newPath[0]);
            for(int i = 1; i < newPath.Count; i++){
                float dist = Vector2.Distance(transform.position, newPath[i]);
                if(dist < closestDistance){
                    closestDistance = dist;
                    closestIndex = i;
                }
            }
            path = newPath;
            currentWaypointIndex = closestIndex;
        }
    }

    private void UpdateDirection()
    {
        // If under player and very close, stop to avoid glitchy back-and-forth movement
        if(player.position.y > transform.position.y){
            float horizontalDistance = Mathf.Abs(player.position.x - transform.position.x);
            if(horizontalDistance < stopOscillationThreshold){
                currentDirection = Vector2.zero;
                return;
            }
        }

        // If no path (blocked or failed), fallback to basic left/right move toward player
        if(path == null || path.Count == 0){
            if(IsObstructedAbove(overheadDetectionRadius) && player.position.y > transform.position.y){
                // Try to go around platform overhead
                int clearanceDir = GetClearanceDirection();
                currentDirection = new Vector2(clearanceDir, 0);
            }else{
                float dx = player.position.x - transform.position.x;
                if(Mathf.Abs(dx) < 0.1f) dx = lastHorizontalDir;
                else lastHorizontalDir = Mathf.Sign(dx);
                currentDirection = new Vector2(Mathf.Sign(dx), 0);
            }
            return;
        }

        // If we're at the end of the path, just go toward player
        if(currentWaypointIndex >= path.Count){
            float dx = player.position.x - transform.position.x;
            if(Mathf.Abs(dx) < 0.1f) dx = lastHorizontalDir;
            else lastHorizontalDir = Mathf.Sign(dx);
            currentDirection = new Vector2(Mathf.Sign(dx), 0);
            return;
        }

        // Get direction to current waypoint
        Vector2 targetWaypoint = path[currentWaypointIndex];
        Vector2 toWaypoint = targetWaypoint - (Vector2)transform.position;

        // If close to the current waypoint, move to the next one
        if(toWaypoint.magnitude < waypointThreshold){
            currentWaypointIndex++;
            if(currentWaypointIndex >= path.Count){
                // Again just follow player if no more waypoints
                float dx = player.position.x - transform.position.x;
                if(Mathf.Abs(dx) < 0.1f) dx = lastHorizontalDir;
                else lastHorizontalDir = Mathf.Sign(dx);
                currentDirection = new Vector2(Mathf.Sign(dx), 0);
                return;
            }
            targetWaypoint = path[currentWaypointIndex];
            toWaypoint = targetWaypoint - (Vector2)transform.position;
        }

        // Figure out which direction to move based on waypoint/player location
        float horizontalDiff = targetWaypoint.x - transform.position.x;
        if(Mathf.Abs(horizontalDiff) < 0.1f){
            // If directly below player, keep last direction
            if(player.position.y - transform.position.y > verticalPursuitThreshold){
                horizontalDiff = lastHorizontalDir;
            }else{
                horizontalDiff = player.position.x - transform.position.x;
                if(Mathf.Abs(horizontalDiff) < 0.1f){
                    horizontalDiff = lastHorizontalDir;
                }
            }
        }else{
            lastHorizontalDir = Mathf.Sign(horizontalDiff);
        }

        // Make sure we don't go the wrong way if the player is clearly on the other side
        float playerDiff = player.position.x - transform.position.x;
        if(Mathf.Abs(playerDiff) > 0.1f && Mathf.Sign(horizontalDiff) != Mathf.Sign(playerDiff)){
            horizontalDiff = playerDiff;
            lastHorizontalDir = Mathf.Sign(playerDiff);
        }

        // If blocked overhead, try to move left/right
        if(IsObstructedAbove(overheadDetectionRadius) && player.position.y > transform.position.y){
            int clearanceDir = GetClearanceDirection();
            horizontalDiff = clearanceDir;
        }

        currentDirection = new Vector2(Mathf.Sign(horizontalDiff), 0);
    }

    private bool ShouldJump()
    {
        // Check if:
        // - cooldown is over
        // - we're grounded
        // - we're not already going up or down

        if(Time.time < lastJumpTime + jumpCooldown) return false;
        if(!IsGrounded()) return false;
        if(Mathf.Abs(rb.linearVelocity.y) >= 0.1f) return false;

        float verticalDiff = 0f;
        float horizontalDiff = 0f;

        // Check difference in position if path exists
        if(path != null && currentWaypointIndex < path.Count){
            Vector2 targetWaypoint = path[currentWaypointIndex];
            verticalDiff = targetWaypoint.y - transform.position.y;
            horizontalDiff = Mathf.Abs(targetWaypoint.x - transform.position.x);
        }

        // Jump if:
        // - Player is high above
        // - Waypoint is high above
        // - We're at the edge of a platform (no ground ahead)
        // - There's something blocking us above
        bool playerAbove = (player.position.y - transform.position.y) > verticalPursuitThreshold;
        bool waypointAbove = verticalDiff > verticalPursuitThreshold;
        bool noGroundAhead = !IsGroundAhead();
        bool overheadObstacle = IsObstructedAbove(overheadDetectionRadius);

        return playerAbove || waypointAbove || noGroundAhead || overheadObstacle;
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        lastJumpTime = Time.time;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    // Raycast ahead of the boss to see if there's ground coming up (used to detect edges)
    private bool IsGroundAhead()
    {
        Vector2 origin = groundCheck.position;
        Vector2 direction = new Vector2(currentDirection.x, 0);
        float distance = 0.3f;
        return Physics2D.Raycast(origin, direction, distance, groundLayer);
    }

    // Check if there's something above the boss
    private bool IsObstructedAbove(float checkRadius)
    {
        Vector2 origin = (Vector2)transform.position + Vector2.up * (groundCheckRadius + 0.1f);
        return Physics2D.OverlapCircle(origin, checkRadius, groundLayer);
    }

    // Figure out whether it's easier to go left or right around a platform above the boss
    private int GetClearanceDirection()
    {
        Vector2 rayOrigin = (Vector2)transform.position + new Vector2(0, clearanceRayYOffset);

        RaycastHit2D hitRight = Physics2D.Raycast(rayOrigin, Vector2.right, clearanceRayDistance, groundLayer);
        RaycastHit2D hitLeft  = Physics2D.Raycast(rayOrigin, Vector2.left,  clearanceRayDistance, groundLayer);

        if(hitRight.collider == null && hitLeft.collider != null){
            return 1;
        }else if(hitLeft.collider == null && hitRight.collider != null){
            return -1;
        }else if(hitLeft.collider == null && hitRight.collider == null){
            float dx = player.position.x - transform.position.x;
            return Mathf.Abs(dx) > 0.1f ? (int)Mathf.Sign(dx) : (int)lastHorizontalDir;
        }else return (int)lastHorizontalDir;
    }
}