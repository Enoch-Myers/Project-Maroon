using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BossAI : MonoBehaviour
{
    [Header("Pathfinding")]
    public AStarGrid pathGrid;        // Must have a side-scroller config (no diagonals)
    public Transform player;          // Player transform
    public float pathAgain = 1f;      // How often (seconds) to recalc path

    [Header("Movement")]
    public float speed = 3f;
    public float waypointThreshold = 0.2f;  // Increased threshold to avoid oscillation

    [Header("Combat")]
    public float attackRange = 1.5f;

    [Header("Jumping")]
    public float jumpForce = 5f;
    public Transform groundCheck;         // A transform at the boss's feet for ground checking
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;         // Layer(s) that count as ground
    public float jumpThreshold = 0.5f;      // Minimum vertical difference to trigger a jump
    public float horizontalJumpThreshold = 0.2f; // Must be near the platform edge horizontally for a jump
    public float jumpCooldown = 1.0f;       // Minimum time between jumps

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
            lastRepathTime = Time.time;
            List<Vector2> newPath = pathGrid.FindPath(transform.position, player.position);
            Debug.Log("Boss Path found. Count = " + newPath.Count);
            if (newPath != null && newPath.Count > 0)
            {
                // Instead of always starting at index 0, choose the waypoint closest to the boss.
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

        // Update horizontal movement.
        UpdateDirection();

        // Attempt to jump if conditions are met.
        if (ShouldJump())
        {
            Jump();
        }

        // Check for attack range.
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer < attackRange)
        {
            Attack();
        }
    }

    void FixedUpdate()
    {
        // Update horizontal movement while preserving vertical velocity.
        float vy = rb.linearVelocity.y;
        rb.linearVelocity = new Vector2(currentDirection.x * speed, vy);
    }

    private void UpdateDirection()
    {
        // If no path is available, move directly toward the player.
        if (path == null || path.Count == 0)
        {
            float dx = player.position.x - transform.position.x;
            if (Mathf.Abs(dx) < 0.1f)
            {
                // If the player is directly above, continue moving in the last known direction.
                dx = lastHorizontalDir;
            }
            else
            {
                lastHorizontalDir = Mathf.Sign(dx);
            }
            currentDirection = new Vector2(Mathf.Sign(dx), 0);
            return;
        }

        // If the waypoint index is out of range, fallback to moving toward the player.
        if (currentWaypointIndex >= path.Count)
        {
            float dx = player.position.x - transform.position.x;
            if (Mathf.Abs(dx) < 0.1f)
            {
                dx = lastHorizontalDir;
            }
            else
            {
                lastHorizontalDir = Mathf.Sign(dx);
            }
            currentDirection = new Vector2(Mathf.Sign(dx), 0);
            return;
        }

        // Get the current target waypoint.
        Vector2 targetWaypoint = path[currentWaypointIndex];
        Vector2 toWaypoint = targetWaypoint - (Vector2)transform.position;

        // Advance to the next waypoint if close enough.
        if (toWaypoint.magnitude < waypointThreshold)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= path.Count)
            {
                float dx = player.position.x - transform.position.x;
                if (Mathf.Abs(dx) < 0.1f)
                {
                    dx = lastHorizontalDir;
                }
                else
                {
                    lastHorizontalDir = Mathf.Sign(dx);
                }
                currentDirection = new Vector2(Mathf.Sign(dx), 0);
                return;
            }
            targetWaypoint = path[currentWaypointIndex];
            toWaypoint = targetWaypoint - (Vector2)transform.position;
        }

        float horizontalDiff = targetWaypoint.x - transform.position.x;
        // If the horizontal difference is very small but the player is above,
        // continue moving in the last horizontal direction.
        if (Mathf.Abs(horizontalDiff) < 0.1f)
        {
            if (player.position.y - transform.position.y > jumpThreshold)
            {
                horizontalDiff = lastHorizontalDir;
            }
            else
            {
                horizontalDiff = player.position.x - transform.position.x;
                if (Mathf.Abs(horizontalDiff) < 0.1f)
                {
                    horizontalDiff = lastHorizontalDir;
                }
            }
        }
        else
        {
            lastHorizontalDir = Mathf.Sign(horizontalDiff);
        }

        // If there's a discrepancy between the direction to the target and the player,
        // force the boss to move toward the player.
        float playerDiff = player.position.x - transform.position.x;
        if (Mathf.Abs(playerDiff) > 0.1f && Mathf.Sign(horizontalDiff) != Mathf.Sign(playerDiff))
        {
            horizontalDiff = playerDiff;
            lastHorizontalDir = Mathf.Sign(playerDiff);
        }

        currentDirection = new Vector2(Mathf.Sign(horizontalDiff), 0);
    }

    private bool ShouldJump()
    {
        // Respect jump cooldown, ensure the boss is grounded and not already moving vertically.
        if (Time.time < lastJumpTime + jumpCooldown)
            return false;
        if (!IsGrounded())
            return false;
        if (Mathf.Abs(rb.linearVelocity.y) >= 0.1f)
            return false;

        float verticalDiff = 0f;
        float horizontalDiff = 0f;
        if (path != null && currentWaypointIndex < path.Count)
        {
            Vector2 targetWaypoint = path[currentWaypointIndex];
            verticalDiff = targetWaypoint.y - transform.position.y;
            horizontalDiff = Mathf.Abs(targetWaypoint.x - transform.position.x);
        }

        // Option 1: The next waypoint is above and we're near the platform edge.
        bool option1 = verticalDiff > jumpThreshold && horizontalDiff < horizontalJumpThreshold;
        // Option 2: The player is directly above by a sufficient amount.
        bool option2 = (player.position.y - transform.position.y) > jumpThreshold;
        // Option 3: There is no ground ahead (indicating a platform edge).
        bool option3 = !IsGroundAhead();

        return (option1 || option2 || option3);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        lastJumpTime = Time.time;
    }

    // Check if the boss is on the ground.
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    // Cast a ray from the ground check point in the current horizontal direction to detect ground ahead.
    private bool IsGroundAhead()
    {
        Vector2 origin = groundCheck.position;
        Vector2 direction = new Vector2(currentDirection.x, 0);
        float distance = 0.3f; // Adjust as needed for your platform size.
        return Physics2D.Raycast(origin, direction, distance, groundLayer);
    }

    void Attack()
    {
        Debug.Log("Boss is attacking the player!");
        // Insert attack logic here.
    }
}
