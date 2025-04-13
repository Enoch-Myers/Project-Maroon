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
    public GameObject projectilePrefab;
    public Transform firePoint; 


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        Move = Input.GetAxis("Horizontal");

        if (Move != 0)
        {
            lastMoveDirection = Mathf.Sign(Move);

            // Flip player and firepoint when changing direction
            Vector3 localScale = transform.localScale;
            localScale.x = Mathf.Abs(localScale.x) * lastMoveDirection;
            transform.localScale = localScale;
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

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

    }

    public void StartDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        rb.gravityScale = 0f; // Freeze vertical movement
        rb.linearVelocity = new Vector2(dashSpeed * lastMoveDirection, 0f); 
    }

    void StartDeceleration()
    {
        rb.gravityScale = 1f; // Re-enable gravity
        StartCoroutine(Decelerate());
    }

    IEnumerator Decelerate()
    {
        while (Mathf.Abs(rb.linearVelocity.x) > 0.1f)
        {
            rb.linearVelocity = new Vector2(
                Mathf.MoveTowards(rb.linearVelocity.x, 0, deceleration * Time.deltaTime),
                rb.linearVelocity.y
            );
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

    void Shoot()
    {
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        Projectile_behavior pb = proj.GetComponent<Projectile_behavior>();
        if (pb != null)
        {
            pb.direction = lastMoveDirection > 0 ? Vector2.right : Vector2.left;
        }
        // Flip the projectile's visual if needed
        Vector3 projectileScale = proj.transform.localScale;
        projectileScale.x = Mathf.Abs(projectileScale.x) * lastMoveDirection;
        proj.transform.localScale = projectileScale;

    }



}