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

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Prevent unwanted movement
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Initialize patrol target
        currentPatrolTarget = patrolPointA;
    }

    protected virtual void Update()
    {
        if (isDead) return;

        if (PlayerDetected())
        {
            Attack();
        }
        else
        {
            Patrol();
        }
    }

    protected abstract void Attack(); // Melee and Ranged will define their own attacks

    protected virtual void Patrol()
    {
        if (patrolPointA == null || patrolPointB == null) return;

        // Move towards the current patrol target
        transform.position = Vector2.MoveTowards(transform.position, currentPatrolTarget.position, moveSpeed * Time.deltaTime);

        // Check if the enemy has reached the current patrol target
        if (Vector2.Distance(transform.position, currentPatrolTarget.position) < 0.1f)
        {
            // Switch to the other patrol target
            currentPatrolTarget = currentPatrolTarget == patrolPointA ? patrolPointB : patrolPointA;
        }
    }

    protected bool PlayerDetected()
    {
        if (player == null) return false;
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
        // Default behavior for chasing the player (can be empty or provide basic logic)
        Debug.Log("Enemy is chasing the player!");
    }
}
