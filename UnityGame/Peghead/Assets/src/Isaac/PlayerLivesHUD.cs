using UnityEngine;
using UnityEngine.UI;

public class PlayerLivesHUD : MonoBehaviour
{
    public Text livesText;
    private int lives = 3;

    private void OnEnable()
    {
        PlayerHealth.OnLivesChanged += SetLivesFromEvent;
        UpdateUI(); // Ensure UI starts with correct value when enabled
    }

    private void OnDisable()
    {
        PlayerHealth.OnLivesChanged -= SetLivesFromEvent;
    }

    private void SetLivesFromEvent(int newLives)
    {
        Debug.Log("Setting lives from UI: " + newLives);
        lives = Mathf.Clamp(newLives, 0, 3);
        UpdateUI();
    }

    public void LoseLife()
    {
        lives = Mathf.Max(0, lives - 1);
        UpdateUI();
    }

    public void GainLife()
    {
        lives = Mathf.Min(3, lives + 1);
        UpdateUI();
    }

    public void ResetLives()
    {
        lives = 3;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (livesText != null)
        {
            livesText.text = $"❤️ {lives}";
        }
    }
}
