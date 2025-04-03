using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DemoMode : MonoBehaviour
{
    [Header("Path & Waypoints")]
    public Transform[] waypoints;
    public float arriveThreshold = 0.2f;

    [Header("Movement")]
    public float moveSpeed = 3f;
    public float jumpForce = 7f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private int currentIndex = 0;
    private bool isGrounded = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(currentIndex >= waypoints.Length){
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        Vector2 targetPos = waypoints[currentIndex].position;
        Vector2 currentPos = transform.position;
        Vector2 diff = targetPos - currentPos;

        if(diff.magnitude < arriveThreshold){
            currentIndex++;
            return;
        }

        //Move horizontally
        float dir = Mathf.Sign(diff.x);
        rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);

        //Jump if needed
        if(diff.y > 0.5f && isGrounded){
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(waypoints == null || waypoints.Length == 0) return;
        Gizmos.color = Color.cyan;
        for(int i = 0; i < waypoints.Length - 1; i++){
            if(waypoints[i] != null && waypoints[i+1] != null){
                Gizmos.DrawLine(waypoints[i].position, waypoints[i+1].position);
            }
        }
    }
}
