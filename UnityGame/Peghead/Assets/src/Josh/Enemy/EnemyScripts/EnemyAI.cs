using System.Collections;
using UnityEngine;

public abstract class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float detectionRange = 4f;
    public int health = 3;
    public float attackCooldown = 1.5f;

    public Transform patrolPointA; // Point A for patrolling
    public Transform patrolPointB; // Point B for patrolling

    protected Transform player;
    protected Rigidbody2D rb;
    protected bool isAttacking = false;
    protected bool isDead = false;

    private Transform currentPatrolTarget;
    private float playerLostCooldown = 1f; // Time to wait before resuming patrol
    private float timeSincePlayerLost = 0f;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Prevent unwanted movement
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Initialize patrol target to Point A
        currentPatrolTarget = patrolPointA;
    }

    protected virtual void Update()
    {
        if (isDead) return;

        if (PlayerDetected())
        {
            // Chase the player
            ChasePlayer();
            timeSincePlayerLost = 0f; // Reset cooldown
        }
        else
        {
            // Increment cooldown timer
            timeSincePlayerLost += Time.deltaTime;

            if (timeSincePlayerLost >= playerLostCooldown)
            {
                // Reset patrol target to the nearest point
                if (currentPatrolTarget == null || Vector2.Distance(transform.position, currentPatrolTarget.position) > 0.1f)
                {
                    currentPatrolTarget = GetNearestPatrolPoint();
                }

                // Resume patrolling
                Patrol();
            }
        }
    }

    protected abstract void Attack(); // Melee and Ranged will define their own attacks

    protected virtual void Patrol()
    {
        if (patrolPointA == null || patrolPointB == null) return;

        // Move towards the current patrol target
        transform.position = Vector2.MoveTowards(transform.position, currentPatrolTarget.position, moveSpeed * Time.deltaTime);

        // Check if the enemy is close enough to the current patrol target
        if (Vector2.Distance(transform.position, currentPatrolTarget.position) <= 0.1f)
        {
            // Switch to the other patrol target
            currentPatrolTarget = currentPatrolTarget == patrolPointA ? patrolPointB : patrolPointA;
        }
    }

    private Transform GetNearestPatrolPoint()
    {
        float distanceToA = Vector2.Distance(transform.position, patrolPointA.position);
        float distanceToB = Vector2.Distance(transform.position, patrolPointB.position);

        return distanceToA < distanceToB ? patrolPointA : patrolPointB;
    }

    protected bool PlayerDetected()
    {
        if (player == null) return false;

        // Check if the player is within detection range
        return Vector2.Distance(transform.position, player.position) <= detectionRange;
    }

    public virtual void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        isDead = true;
        Debug.Log(gameObject.name + " died!");
        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true;
        Destroy(gameObject, 1f);
    }

    protected virtual void ChasePlayer()
    {
        if (player == null) return;

        // Move toward the player
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        if (patrolPointA != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(patrolPointA.position, 0.2f);
        }

        if (patrolPointB != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(patrolPointB.position, 0.2f);
        }

        if (currentPatrolTarget != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, currentPatrolTarget.position);
        }
    }
}
