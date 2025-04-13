using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BossAI : MonoBehaviour
{
    [Header("Pathfinding")]
    public AStarGrid pathGrid;          // Must have a side-scroller config (no diagonals)
    public Transform player;            // Player transform
    public float pathAgain = 1f;        // How often (seconds) to recalc path

    [Header("Movement")]
    public float speed = 3f;
    public float waypointThreshold = 0.2f;  // Threshold for waypoint proximity

    [Header("Combat")]
    public float attackRange = 1.5f;

    [Header("Jumping")]
    public float jumpForce = 5f;
    public Transform groundCheck;       // A transform at the boss's feet for ground checking
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;       // Layer(s) that count as ground
    public float jumpCooldown = 1.0f;     // Minimum time between jumps

    [Header("Jump Trigger Thresholds")]
    // Vertical difference required to trigger a jump while pursuing the player.
    public float verticalPursuitThreshold = 0.3f;
    // Horizontal difference near platform edge to trigger jump if a waypoint is above.
    public float horizontalJumpThreshold = 0.2f; 

    [Header("Overhead & Oscillation Settings")]
    // When under the player, if within this horizontal distance, stop moving (to avoid oscillation).
    public float stopOscillationThreshold = 0.2f;
    // Radius for detecting obstacles overhead.
    public float overheadDetectionRadius = 0.3f;
    // Distance for clearance raycasts when trying to navigate around overhead platforms.
    public float clearanceRayDistance = 1.0f;
    // Vertical offset for clearance raycasts.
    public float clearanceRayYOffset = 1.0f;

    private Rigidbody2D rb;
    private List<Vector2> path = new List<Vector2>();
    private int currentWaypointIndex = 0;
    private float lastRepathTime = 0f;
    private float lastJumpTime = -Mathf.Infinity;
    private Vector2 currentDirection = Vector2.zero;
    private float lastHorizontalDir = 1f;  // Default horizontal direction (1 = right)

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Recalculate the path periodically.
        if (Time.time >= lastRepathTime + pathAgain)
        {
            RecalculatePath();
        }

        // Update horizontal movement.
        UpdateDirection();

        // Attempt to jump if conditions allow.
        if (ShouldJump())
        {
            Jump();
        }

        // Attack if within range.
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer < attackRange)
        {
            Attack();
        }
    }

    void FixedUpdate()
    {
        // Update horizontal velocity while preserving vertical velocity.
        float vy = rb.linearVelocity.y;
        rb.linearVelocity = new Vector2(currentDirection.x * speed, vy);
    }

    /// <summary>
    /// Recalculates the path to the player using A*.
    /// </summary>
    private void RecalculatePath()
    {
        lastRepathTime = Time.time;
        List<Vector2> newPath = pathGrid.FindPath(transform.position, player.position);
        Debug.Log("Boss Path found. Count = " + newPath.Count);
        if (newPath != null && newPath.Count > 0)
        {
            // Find the closest waypoint.
            int closestIndex = 0;
            float closestDistance = Vector2.Distance(transform.position, newPath[0]);
            for (int i = 1; i < newPath.Count; i++)
            {
                float dist = Vector2.Distance(transform.position, newPath[i]);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    closestIndex = i;
                }
            }
            path = newPath;
            currentWaypointIndex = closestIndex;
        }
    }

    /// <summary>
    /// Updates the horizontal movement direction.
    /// </summary>
    private void UpdateDirection()
    {
        // 1. If the boss is directly under the player and very close horizontally, stop moving.
        if (player.position.y > transform.position.y)
        {
            float horizontalDistance = Mathf.Abs(player.position.x - transform.position.x);
            if (horizontalDistance < stopOscillationThreshold)
            {
                currentDirection = Vector2.zero;
                return;
            }
        }

        // 2. If no path is available, use basic movement with overhead avoidance.
        if (path == null || path.Count == 0)
        {
            if (IsObstructedAbove(overheadDetectionRadius) && player.position.y > transform.position.y)
            {
                // Determine which direction is clearer.
                int clearanceDir = GetClearanceDirection();
                currentDirection = new Vector2(clearanceDir, 0);
            }
            else
            {
                float dx = player.position.x - transform.position.x;
                if (Mathf.Abs(dx) < 0.1f)
                    dx = lastHorizontalDir;
                else
                    lastHorizontalDir = Mathf.Sign(dx);
                currentDirection = new Vector2(Mathf.Sign(dx), 0);
            }
            return;
        }

        // 3. If the current waypoint is out of range, default to moving toward the player.
        if (currentWaypointIndex >= path.Count)
        {
            float dx = player.position.x - transform.position.x;
            if (Mathf.Abs(dx) < 0.1f)
                dx = lastHorizontalDir;
            else
                lastHorizontalDir = Mathf.Sign(dx);
            currentDirection = new Vector2(Mathf.Sign(dx), 0);
            return;
        }

        // 4. Get the current waypoint.
        Vector2 targetWaypoint = path[currentWaypointIndex];
        Vector2 toWaypoint = targetWaypoint - (Vector2)transform.position;

        // If close to the waypoint, move to the next one.
        if (toWaypoint.magnitude < waypointThreshold)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= path.Count)
            {
                float dx = player.position.x - transform.position.x;
                if (Mathf.Abs(dx) < 0.1f)
                    dx = lastHorizontalDir;
                else
                    lastHorizontalDir = Mathf.Sign(dx);
                currentDirection = new Vector2(Mathf.Sign(dx), 0);
                return;
            }
            targetWaypoint = path[currentWaypointIndex];
            toWaypoint = targetWaypoint - (Vector2)transform.position;
        }

        // Consider horizontal direction.
        float horizontalDiff = targetWaypoint.x - transform.position.x;
        if (Mathf.Abs(horizontalDiff) < 0.1f)
        {
            // If the player is above, maintain the last horizontal direction.
            if (player.position.y - transform.position.y > verticalPursuitThreshold)
                horizontalDiff = lastHorizontalDir;
            else
            {
                horizontalDiff = player.position.x - transform.position.x;
                if (Mathf.Abs(horizontalDiff) < 0.1f)
                    horizontalDiff = lastHorizontalDir;
            }
        }
        else
        {
            lastHorizontalDir = Mathf.Sign(horizontalDiff);
        }

        // Force movement toward the player if directions conflict.
        float playerDiff = player.position.x - transform.position.x;
        if (Mathf.Abs(playerDiff) > 0.1f && Mathf.Sign(horizontalDiff) != Mathf.Sign(playerDiff))
        {
            horizontalDiff = playerDiff;
            lastHorizontalDir = Mathf.Sign(playerDiff);
        }

        // If an overhead obstacle exists and the player is above, choose a clearance direction.
        if (IsObstructedAbove(overheadDetectionRadius) && player.position.y > transform.position.y)
        {
            int clearanceDir = GetClearanceDirection();
            horizontalDiff = clearanceDir;
        }

        currentDirection = new Vector2(Mathf.Sign(horizontalDiff), 0);
    }

    /// <summary>
    /// Decides whether the boss should jump.
    /// </summary>
    private bool ShouldJump()
    {
        // Respect jump cooldown, ensure the boss is grounded, and not already moving vertically.
        if (Time.time < lastJumpTime + jumpCooldown)
            return false;
        if (!IsGrounded())
            return false;
        if (Mathf.Abs(rb.linearVelocity.y) >= 0.1f)
            return false;

        float verticalDiff = 0f;
        float horizontalDiff = 0f;

        // Check the current waypoint if available.
        if (path != null && currentWaypointIndex < path.Count)
        {
            Vector2 targetWaypoint = path[currentWaypointIndex];
            verticalDiff = targetWaypoint.y - transform.position.y;
            horizontalDiff = Mathf.Abs(targetWaypoint.x - transform.position.x);
        }

        // Conditions to jump:
        // a) The player is above by at least the vertical pursuit threshold.
        bool playerAbove = (player.position.y - transform.position.y) > verticalPursuitThreshold;
        // b) The current waypoint (if any) is above.
        bool waypointAbove = verticalDiff > verticalPursuitThreshold;
        // c) There is no ground ahead (potentially at a platform edge).
        bool noGroundAhead = !IsGroundAhead();
        // d) There is an obstacle overhead.
        bool overheadObstacle = IsObstructedAbove(overheadDetectionRadius);

        return playerAbove || waypointAbove || noGroundAhead || overheadObstacle;
    }

    /// <summary>
    /// Applies an upward jump force.
    /// </summary>
    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        lastJumpTime = Time.time;
    }

    /// <summary>
    /// Returns true if the boss is grounded.
    /// </summary>
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    /// <summary>
    /// Checks for ground in front of the boss by raycasting from the ground check.
    /// </summary>
    private bool IsGroundAhead()
    {
        Vector2 origin = groundCheck.position;
        Vector2 direction = new Vector2(currentDirection.x, 0);
        float distance = 0.3f; // Adjust as needed.
        return Physics2D.Raycast(origin, direction, distance, groundLayer);
    }

    /// <summary>
    /// Checks for obstacles above the boss using a configurable radius.
    /// </summary>
    private bool IsObstructedAbove(float checkRadius)
    {
        Vector2 origin = (Vector2)transform.position + Vector2.up * (groundCheckRadius + 0.1f);
        return Physics2D.OverlapCircle(origin, checkRadius, groundLayer);
    }

    /// <summary>
    /// Determines which horizontal direction (left/right) is clearer to navigate around an overhead platform.
    /// Uses raycasts with an elevated Y offset.
    /// </summary>
    private int GetClearanceDirection()
    {
        Vector2 rayOrigin = (Vector2)transform.position + new Vector2(0, clearanceRayYOffset);
        // Raycast to the right.
        RaycastHit2D hitRight = Physics2D.Raycast(rayOrigin, Vector2.right, clearanceRayDistance, groundLayer);
        // Raycast to the left.
        RaycastHit2D hitLeft = Physics2D.Raycast(rayOrigin, Vector2.left, clearanceRayDistance, groundLayer);

        if (hitRight.collider == null && hitLeft.collider != null)
        {
            return 1;
        }
        else if (hitLeft.collider == null && hitRight.collider != null)
        {
            return -1;
        }
        else if (hitLeft.collider == null && hitRight.collider == null)
        {
            // Both sides are clear; default to moving toward the player's horizontal position.
            float dx = player.position.x - transform.position.x;
            return Mathf.Abs(dx) > 0.1f ? (int)Mathf.Sign(dx) : (int)lastHorizontalDir;
        }
        else
        {
            // Both sides appear blocked; stick to previous direction.
            return (int)lastHorizontalDir;
        }
    }

    void Attack()
    {
        Debug.Log("Boss is attacking the player!");
        // Insert attack logic here.
    }

    // Debug visualization for path waypoints.
    void OnDrawGizmos()
    {
        if (path != null)
        {
            Gizmos.color = Color.green;
            foreach (Vector2 point in path)
            {
                Gizmos.DrawSphere(point, 0.1f);
            }
            if (currentWaypointIndex < path.Count)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(path[currentWaypointIndex], 0.15f);
            }
        }
    }
}
