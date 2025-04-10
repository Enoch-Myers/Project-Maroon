using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 1; // Damage dealt by the projectile
    public float lifetime = 5f; // Time before the projectile is destroyed

    private void Start()
    {
        // Destroy the projectile after a certain time to avoid clutter
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Projectile hit the player!");
            collision.GetComponent<PlayerHealth>()?.TakeDamage(damage); // Pass the damage value
            Destroy(gameObject); // Destroy the projectile on impact
        }
        else if (collision.CompareTag("Obstacle"))
        {
            Destroy(gameObject); // Destroy the projectile if it hits an obstacle
        }
    }
}