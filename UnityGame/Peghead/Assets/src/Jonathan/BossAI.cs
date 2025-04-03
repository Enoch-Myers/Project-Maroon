using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BossAI : MonoBehaviour
{
    [Header("Pathfinding")]
    public AStarGrid pathGrid;    // Must have a side-scroller config (no diagonals)
    public Transform player;      // Player transform
    public float pathAgain = 1f;  // How often (seconds) to recalc path

    [Header("Movement")]
    public float speed = 3f;          
    public float waypointThreshold = 0.2f;  // Increased slightly to avoid oscillation

    [Header("Combat")]
    public float attackRange = 1.5f;

    [Header("Jumping")]
    public float jumpForce = 5f;
    public Transform groundCheck;       // A transform at the boss's feet for checking the ground
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;       // Layer(s) that count as ground
    public float jumpThreshold = 0.5f;    // Minimum vertical difference to trigger a jump

    private Rigidbody2D rb;
    private List<Vector2> path = new List<Vector2>();
    private int currentWaypointIndex = 0;
    private float lastRepathTime = 0f;
    private Vector2 currentDirection = Vector2.zero;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Recalculate path periodically
        if (Time.time >= lastRepathTime + pathAgain)
        {
            lastRepathTime = Time.time;
            List<Vector2> newPath = pathGrid.FindPath(transform.position, player.position);
            Debug.Log("Boss Path found. Count = " + newPath.Count);
            if (newPath != null && newPath.Count > 0)
            {
                // Instead of simply resetting to 0, pick the waypoint closest to our current position.
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

        // Update movement direction
        UpdateDirection();

        // Jump logic
        if (ShouldJump())
        {
            Jump();
        }

        // Check attack range
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer < attackRange)
        {
            Attack();
        }
    }

    void FixedUpdate()
    {
        // Preserve vertical velocity (for jump/gravity) and update horizontal movement.
        float vy = rb.linearVelocity.y;
        rb.linearVelocity = new Vector2(currentDirection.x * speed, vy);
    }

    private void UpdateDirection()
    {
        // If no path exists, move directly toward the player.
        if (path == null || path.Count == 0)
        {
            float dx = player.position.x - transform.position.x;
            currentDirection = (Mathf.Abs(dx) > 0.01f) ? new Vector2(Mathf.Sign(dx), 0) : Vector2.zero;
            return;
        }

        if (currentWaypointIndex >= path.Count)
        {
            float dx = player.position.x - transform.position.x;
            currentDirection = (Mathf.Abs(dx) > 0.01f) ? new Vector2(Mathf.Sign(dx), 0) : Vector2.zero;
            return;
        }

        Vector2 targetWaypoint = path[currentWaypointIndex];
        Vector2 toWaypoint = targetWaypoint - (Vector2)transform.position;

        // If close enough to the current waypoint, move to the next one.
        if (toWaypoint.magnitude < waypointThreshold)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= path.Count)
            {
                float dx = player.position.x - transform.position.x;
                currentDirection = (Mathf.Abs(dx) > 0.01f) ? new Vector2(Mathf.Sign(dx), 0) : Vector2.zero;
                return;
            }
            targetWaypoint = path[currentWaypointIndex];
            toWaypoint = targetWaypoint - (Vector2)transform.position;
        }

        // Compute horizontal difference toward the waypoint.
        float horizontalDiff = targetWaypoint.x - transform.position.x;

        // If this difference is extremely small, fallback to using the player's horizontal difference.
        if (Mathf.Abs(horizontalDiff) < 0.1f)
        {
            horizontalDiff = player.position.x - transform.position.x;
        }

        // Ensure the boss always moves in the direction of the player.
        float playerDiff = player.position.x - transform.position.x;
        if (Mathf.Abs(playerDiff) > 0.1f && Mathf.Sign(horizontalDiff) != Mathf.Sign(playerDiff))
        {
            horizontalDiff = playerDiff;
        }

        currentDirection = new Vector2(Mathf.Sign(horizontalDiff), 0);
    }

    private bool ShouldJump()
    {
        if (path == null || currentWaypointIndex >= path.Count)
            return false;

        Vector2 targetWaypoint = path[currentWaypointIndex];
        float verticalDiff = targetWaypoint.y - transform.position.y;
        return verticalDiff > jumpThreshold && IsGrounded();
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void Attack()
    {
        Debug.Log("Boss is attacking the player!");
        // Add your boss attack code here
    }
}
