using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerAutoIdle : MonoBehaviour
{
    [Header("Manual Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public Transform groundCheck;       
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Idle / Auto-Play Settings")]
    public float idleThreshold = 10f;   // Seconds of no input to switch to auto-play
    public AStarGrid pathGrid;          // Reference to the A* grid
    public Transform autoTarget;        // Where we want to go in auto mode

    private Rigidbody2D rb;
    private bool isGrounded;
    private float horizontalInput;
    private bool inAutoMode = false;
    private float lastInputTime;

    [Header("Auto-Path Settings")]
    public float repathRate = 1f;
    private float lastRepathTime = 0f;
    private List<Vector2> path = new List<Vector2>();
    private int currentWaypointIndex = 0;
    public float waypointThreshold = 0.1f;
    public float autoMoveSpeed = 3f; // Speed during auto mode

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        lastInputTime = Time.time;
    }

    void Update()
    {
        if (!inAutoMode)
        {
            // MANUAL CONTROL
            horizontalInput = Input.GetAxisRaw("Horizontal");

            // Basic ground check
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

            // Jump with W
            if (Input.GetKeyDown(KeyCode.W) && isGrounded)
            {
                // Apply jump velocity
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }

            // If user pressed movement or jump, reset idle timer
            if (Mathf.Abs(horizontalInput) > 0.01f || Input.GetKeyDown(KeyCode.W))
            {
                lastInputTime = Time.time;
            }

            // If idle too long, go auto
            if (Time.time - lastInputTime > idleThreshold)
            {
                EnterAutoMode();
            }
        }
        else
        {
            // AUTO-PLAY
            if (Time.time >= lastRepathTime + repathRate)
            {
                lastRepathTime = Time.time;
                // Ask A* for path to autoTarget
                path = pathGrid.FindPath(transform.position, autoTarget.position);
                currentWaypointIndex = 0;
                Debug.Log("Auto path found. Count = " + path.Count);
            }

            UpdateAutoMovement();
        }
    }

    void FixedUpdate()
    {
        if (!inAutoMode)
        {
            // Only set X velocity, preserve Y for gravity/jumps
            float vy = rb.linearVelocity.y;
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, vy);
        }
        else
        {
            // In auto mode, movement is handled by UpdateAutoMovement()
        }
    }

    void EnterAutoMode()
    {
        inAutoMode = true;
        Debug.Log("Switched to Auto Mode!");
    }

    void UpdateAutoMovement()
    {
        if (path == null || path.Count == 0) return;
        if (currentWaypointIndex >= path.Count) return;

        Vector2 targetWaypoint = path[currentWaypointIndex];
        Vector2 toWaypoint = targetWaypoint - (Vector2)transform.position;

        // If close enough, go to next waypoint
        if (toWaypoint.magnitude < waypointThreshold)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= path.Count) return;

            targetWaypoint = path[currentWaypointIndex];
            toWaypoint = targetWaypoint - (Vector2)transform.position;
        }

        // Move horizontally toward waypoint, keep vertical velocity for gravity
        float currentVerticalVelocity = rb.linearVelocity.y;
        Vector2 direction = toWaypoint.normalized;
        float vx = direction.x * autoMoveSpeed;
        rb.linearVelocity = new Vector2(vx, currentVerticalVelocity);

        // Optionally revert to manual if any key pressed:
        // if (Input.anyKeyDown)
        // {
        //     inAutoMode = false;
        //     lastInputTime = Time.time;
        // }
    }
}
