using System.Collections;
using UnityEngine;

public class player_movement : MonoBehaviour
{
    public float speed;
    public float dashSpeed;
    public float dashTime;
    public float deceleration;
    private float dashTimeLeft;
    private bool isDashing;

    public float Move;
    private float lastMoveDirection = 1f;
    public float Jump;
    public bool isJumping;
    public Rigidbody2D rb;
    public bool isTouchingWall;
    public float minX = -10f; // Left boundary
p   ublic float maxX = 10f;  // Right boundary
    public float minY = -5f;  // Bottom boundary
    public float maxY = 5f;   // Top boundary


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

void Update()
{
    Move = Input.GetAxis("Horizontal");

    if (Move != 0)
    {
        lastMoveDirection = Mathf.Sign(Move);
    }

    if (isDashing)
    {
        dashTimeLeft -= Time.deltaTime;
        if (dashTimeLeft <= 0)
        {
            isDashing = false;
            StartDeceleration();
        }
    }
    else
    {
        rb.linearVelocity = new Vector2(speed * Move, rb.linearVelocity.y);

        if (Input.GetButtonDown("Jump") && isJumping == false)
        {
            rb.AddForce(new Vector2(rb.linearVelocity.x, Jump));
            Debug.Log("Jump");
        }

        if (Input.GetButtonDown("Dash"))
        {
            StartDash();
        }
    }

    // Clamping the player's position to defined boundaries
    transform.position = new Vector3(
        Mathf.Clamp(transform.position.x, minX, maxX),
        Mathf.Clamp(transform.position.y, minY, maxY),
        transform.position.z
    );
}


    public void StartDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        rb.linearVelocity = new Vector2(dashSpeed * lastMoveDirection, rb.linearVelocity.y);
    }

    void StartDeceleration()
    {
        StartCoroutine(Decelerate());
    }

    IEnumerator Decelerate()
    {
        while (Mathf.Abs(rb.linearVelocity.x) > 0.1f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x * deceleration, rb.linearVelocity.y);
            yield return null;
        }

        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isJumping = true;
        }
    }
}
