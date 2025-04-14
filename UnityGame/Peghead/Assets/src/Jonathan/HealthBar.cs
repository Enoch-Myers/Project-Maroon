using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public BossStats bossStats;
    public Image healthBarImage;

    void Update()
    {
        if(bossStats != null && healthBarImage != null){
            // Calculate the health fraction (a number between 0 and 1)
            float healthFraction = (float)bossStats.currentHealth / bossStats.maxHealth;
            
            // Update the health bar's fill amount to visually represent current health.
            healthBarImage.fillAmount = healthFraction;
        }
    }
}
