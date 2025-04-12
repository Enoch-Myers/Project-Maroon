using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float patrolSpeed = 2f;
    public float chaseSpeed = 3f;
    public float patrolRange = 5f;
    public float detectionRange = 10f;// Modified by Miles to work better with spawn script
    public float attackRange = 1f;
    public int health = 3;
    public float attackCooldown = 1.5f;

    private bool movingRight = true;
    private Vector2 startPos;
    private Transform player;
    private bool isAttacking = false;
    private bool isDead = false;

    public LayerMask playerLayer;
    public Transform attackPoint;
    public float attackRadius = 0.5f;
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        rb.gravityScale = 0; // Prevents gravity from affecting the enemy
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        startPos = transform.position;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (isDead) return;
        
        if (PlayerDetected())
        {
            if (Vector2.Distance(transform.position, player.position) <= attackRange)
            {
                StartCoroutine(PerformAttack());
            }
            else
            {
                ChasePlayer();
            }
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        float moveDirection = movingRight ? 1 : -1;
        rb.linearVelocity = new Vector2(moveDirection * patrolSpeed, rb.linearVelocity.y);
        
        if (movingRight && transform.position.x >= startPos.x + patrolRange)
            Flip();
        else if (!movingRight && transform.position.x <= startPos.x - patrolRange)
            Flip();
    }

    private void Flip()
    {
        movingRight = !movingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    public bool PlayerDetected()
    {
        if (player == null) return false;
        return Vector2.Distance(transform.position, player.position) <= detectionRange;
    }

    private void ChasePlayer()
    {
        float direction = player.position.x > transform.position.x ? 1 : -1;
        rb.linearVelocity = new Vector2(direction * chaseSpeed, 0);
        transform.localScale = new Vector3(direction, transform.localScale.y, transform.localScale.z);
    }

    private IEnumerator PerformAttack()
    {
        if (isAttacking) yield break;
        isAttacking = true;
        
        // Simulate attack delay
        yield return new WaitForSeconds(0.5f);
        
        Collider2D hitPlayer = Physics2D.OverlapBox(attackPoint.position, new Vector2(attackRadius, attackRadius), 0, playerLayer);
        if (hitPlayer != null)
        {
            Debug.Log("Player hit!");
            // Apply damage to player if they have a health script
        }
        
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    public void TakeDamage(int damage)
    {
        if(isDead) return;

        health -= damage;
        if (health <= 0)
        {
            if(!isDead) Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Enemy died!");
        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true;
        coll.enabled = false;
        Destroy(gameObject, 1f); // Delayed destruction
    }

    private void OnTriggerEnter2D(Collider2D collision)// added by miles
    {
        if (collision.CompareTag("Projectile"))
        {
            TakeDamage(1); // Apply damage (tweak value as needed)
            Destroy(collision.gameObject); // Destroy the projectile
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(attackPoint.position, new Vector3(attackRadius, attackRadius, 1));
        }
    }
}
