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
            // Assuming the player has a script with a TakeDamage method
            //collision.GetComponent<PlayerHealth>()?.TakeDamage(damage);
            Destroy(gameObject); // Destroy the projectile on impact
        }
        else if (collision.CompareTag("Obstacle"))
        {
            // Destroy the projectile if it hits an obstacle / platform;
            Destroy(gameObject);
        }
    }
}