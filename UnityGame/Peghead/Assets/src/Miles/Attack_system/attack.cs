using UnityEngine;

public class attack : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) // implemented in joshes enemy ui
    {
        if (collision.CompareTag("Projectile"))
        {
            //TakeDamage(1); // Apply damage (tweak value as needed)
            Destroy(collision.gameObject); // Destroy the projectile
        }
    }
}
