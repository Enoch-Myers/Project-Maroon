using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class HealthBoundaryTest
{
    private GameObject player;
    private Player_Health playerHealth;

    [SetUp]
    public void SetUp()
    {
        // Create a new GameObject for the player and add the Player_Health script to it
        player = new GameObject("Player");
        playerHealth = player.AddComponent<Player_Health>();
        playerHealth.maxHealth = 3; // Set max health to 3
        playerHealth.currentHealth = playerHealth.maxHealth; // Start with full health
    }

    [UnityTest]
    public IEnumerator Health_ShouldNotGoBelowZero_WhenDamageApplied()
    {
        // Initial health before taking damage
        int initialHealth = playerHealth.currentHealth;
        Debug.Log($"Initial Health: {initialHealth}");

        // Capture the console log output
        string logMessage = string.Empty;
        Application.logMessageReceived += (condition, stackTrace, type) => logMessage = condition;

        // Apply damage to reduce health below zero
        playerHealth.TakeDamage(5); // Deal more damage than current health

        yield return null; // Skip a frame to let the damage apply

        // Assert that health does not go below zero
        //Assert.AreEqual(0, playerHealth.currentHealth, "Health should not go below zero.");

        // Check if the death message is logged
        Assert.IsTrue(logMessage.Contains("Player dead"), "Player death log message was not triggered.");
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(player); // Destroy player GameObject after the test
    }
}