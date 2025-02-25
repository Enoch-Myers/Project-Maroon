using UnityEngine;

public class player_movement : MonoBehaviour
{
    public float speed;
    private float Move;

    public float Jump;

    public bool isJumping;

    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move = Input.GetAxis("Horizontal");
        // Horizontal movement
        rb.linearVelocity = new Vector2(speed * Move,rb.linearVelocity.y);

        // Ability to jump
         if (Input.GetButtonDown("Jump") && isJumping == false)
        {
            rb.AddForce(new Vector2(rb.linearVelocity.x, Jump));
            Debug.Log("Jump");
        }
    }
    // Stops spam jumping
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
