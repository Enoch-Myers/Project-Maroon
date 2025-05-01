using System.Collections;
using UnityEngine;

public class FlyingEnemy : EnemyAI
{
    public Transform pointA;
    public Transform pointB;
    public float verticalChaseRange = 3f;

    public float patrolSpeed = 2f; // Speed for patrolling
    public float chaseSpeed = 3f;  // Speed for chasing the player
    public float swoopSpeed = 4f;  // Speed for swooping
    public float swoopHeight = 2f; // How far down the enemy swoops
    public float attackRange = 1f; // Range within which the flying enemy can attack
    public float attackCooldown = 1.5f; // Cooldown between attacks

    private bool movingToB = true;
    private bool isSwooping = false;
    private bool canAttack = true; // Tracks if the enemy can attack

    protected override void Start()
    {
        base.Start();
        transform.position = pointA.position; // Start at point A
    }

    protected override void Update()
    {
        if (isDead) return;

        if (PlayerDetected() && !isSwooping)
        {
            StartCoroutine(SwoopDownAndAttack());
        }
        else if (!isSwooping)
        {
            Patrol();
        }
    }

    protected override void Patrol()
    {
        Transform target = movingToB ? pointB : pointA;
        transform.position = Vector2.MoveTowards(transform.position, target.position, patrolSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            movingToB = !movingToB;
        }
    }

    private IEnumerator SwoopDownAndAttack()
    {
        isSwooping = true;

        // Swoop down toward the player
        Vector2 swoopTarget = new Vector2(player.position.x, player.position.y - swoopHeight);
        while (Vector2.Distance(transform.position, swoopTarget) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, swoopTarget, swoopSpeed * Time.deltaTime);
            yield return null;
        }

        // Attack the player
        Attack();

        // Swoop back up to a position above the player
        Vector2 returnTarget = new Vector2(player.position.x, player.position.y + swoopHeight);
        while (Vector2.Distance(transform.position, returnTarget) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, returnTarget, swoopSpeed * Time.deltaTime);
            yield return null;
        }

        isSwooping = false;
    }

    protected override void Attack()
    {
        if (!canAttack) return;

        // Check if the player is within attack range
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            Debug.Log("Flying enemy attacks the player!");
            player.GetComponent<PlayerHealth>()?.TakeDamage(); // Deal damage to the player

            // Start attack cooldown
            StartCoroutine(AttackCooldown());
        }
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && canAttack)
        {
            Debug.Log("Flying enemy triggered with the player!");
            collision.collider.GetComponent<Player_Health>()?.TakeDamage(1); // Deal damage to the player

            // Start attack cooldown
            StartCoroutine(AttackCooldown());
        }
        if (collision.collider.CompareTag("Projectile")){
            health--;
            Debug.Log("bird damaged");
            if (health <= 0)
            {
                Debug.Log("bird dead");
                Destroy(gameObject);
            }
        }
    }
}
