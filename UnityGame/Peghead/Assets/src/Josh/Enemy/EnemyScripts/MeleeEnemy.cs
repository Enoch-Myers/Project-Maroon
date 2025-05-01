using System.Collections;
using UnityEngine;

public class MeleeEnemy : EnemyAI
{
    public float attackRange = 1f;
    public float attackRadius = 0.5f;
    public LayerMask playerLayer;
    public float attackCooldown = 1.5f; // Cooldown between attacks
    private bool canAttack = true; // Tracks if the enemy can attack

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && canAttack)
        {
            AttackPlayer(collision.collider);
        }
        if (collision.collider.CompareTag("Projectile"))
        {
            health--;
            Debug.Log("Enemy Damaged");

            if (health <= 0)
            {
                Debug.Log("Enemy killed");
                Destroy(gameObject);
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && canAttack)
        {
            AttackPlayer(collision.collider);
        }
    }

    private void AttackPlayer(Collider2D playerCollider)
    {
        // Deal damage to the player
        Debug.Log("Player hit by melee attack!");
        playerCollider.GetComponent<Player_Health>()?.TakeDamage(1);

        // Start cooldown
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    protected override void Attack()
    {
        // Implement the Attack logic for the melee enemy
        Collider2D hitPlayer = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);
        if (hitPlayer != null && hitPlayer.CompareTag("Player") && canAttack)
        {
            AttackPlayer(hitPlayer);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the attack range in the Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
