using UnityEngine;

public class Player_Health : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    // damages player
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if(currentHealth <= 0)
            {
                // Player death // Respawn or game over screen
                Debug.Log("Player dead");

            }
    }
    // Heals player
    public void HealPlayer(int amount)
    {
        currentHealth += amount;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        
    }
}
