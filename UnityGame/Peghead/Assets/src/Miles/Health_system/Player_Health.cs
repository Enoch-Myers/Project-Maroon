using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Health : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    // basic player health script mainly used for testing while waiting for rest of player to be made
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    // damages player
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Player took damageCurrent health:" + currentHealth);
        if (currentHealth <= 0)
        {
            // Player death // Respawn or game over screen
            Debug.Log("Player dead");
            SceneManager.LoadScene("TitleScreen");

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
