using System.Collections;
using UnityEngine;

public class RangedEnemy : EnemyAI
{
    public GameObject projectilePrefab; // Assign the projectile prefab in the Unity Editor
    public float projectileSpeed = 5f;

    protected override void Attack()
    {
        if (!isAttacking)
        {
            StartCoroutine(ShootProjectile());
        }
    }

    private IEnumerator ShootProjectile()
    {
        isAttacking = true;

        yield return new WaitForSeconds(0.5f); // Shoot delay

        if (player != null)
        {
            // Calculate the direction to the player
            Vector2 direction = (player.position - transform.position).normalized;

            // Instantiate the projectile at the enemy's position
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            // Set the projectile's velocity
            Rigidbody2D projRb = projectile.GetComponent<Rigidbody2D>();
            if (projRb != null)
            {
                projRb.linearVelocity = direction * projectileSpeed;
            }
        }

        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }
}
