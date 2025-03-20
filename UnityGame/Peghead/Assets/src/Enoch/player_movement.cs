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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        Move = Input.GetAxis("Horizontal");

        if (Move != 0)
        {
            lastMoveDirection = Mathf.Sign(Move);   // Dash in the correct direction
        }

        if (isDashing)
        {
            dashTimeLeft -= Time.deltaTime;
            if (dashTimeLeft <= 0)
            {
                isDashing = false;
                StartDeceleration();    // Decelerate from dash naturally
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
