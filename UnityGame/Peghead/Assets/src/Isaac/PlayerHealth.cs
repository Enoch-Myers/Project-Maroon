using UnityEngine;
using System;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxLives = 3;
    private int currentLives;

    public static event Action<int> OnLivesChanged;
    public static event Action OnPlayerDied;

    private void Awake()
    {
        currentLives = maxLives;
        NotifyUI();
    }

    public void TakeDamage()
    {
        if (currentLives <= 0) return;

        currentLives--;

        Debug.Log($"Player took damage! Current lives: {currentLives}");

        NotifyUI();

        if (currentLives <= 0)
        {
            Die();
        }
    }

    public void Heal()
    {
        if (currentLives < maxLives)
        {
            currentLives++;
            NotifyUI();
        }
    }

    public void ResetLives()
    {
        currentLives = maxLives;
        NotifyUI();
    }

    private void Die()
    {
        Debug.Log("Player died!");
        OnPlayerDied?.Invoke();
    }

    private void NotifyUI()
    {
        OnLivesChanged?.Invoke(currentLives);
    }

    public int GetCurrentLives()
    {
        return currentLives;
    }

    public int GetMaxLives()
    {
        return maxLives;
    }
}
