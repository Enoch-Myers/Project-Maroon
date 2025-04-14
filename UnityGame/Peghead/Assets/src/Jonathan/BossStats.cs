using UnityEngine;

public class BossStats : MonoBehaviour
{
    [Header("Boss Stats")]
    public int maxHealth = 100;
    public int currentHealth;
    // Amount of damage the boss deals to the player on collision.
    public int attackDamage = 10;

    // Called when the boss is first enabled.
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Called automatically when a collision happens.
    // This method checks if the boss collides with the player,
    // then "attacks" the player and takes a bit of damage itself.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // For demonstration/troubleshooting, let the boss also take damage on collision.
            int selfDamage = 5;
            TakeDamage(selfDamage);
            Debug.Log("Boss took " + selfDamage + " damage due to collision.");
        }
    }

    // Call this method to apply damage to the boss.
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
            currentHealth = 0;
        
        Debug.Log("Boss current health: " + currentHealth);
        
        if (currentHealth == 0)
        {
            Debug.Log("Boss defeated: " + gameObject.name);
            // Optionally, add additional boss-death logic (such as playing a death animation, dropping loot, etc.)
        }
    }
}
