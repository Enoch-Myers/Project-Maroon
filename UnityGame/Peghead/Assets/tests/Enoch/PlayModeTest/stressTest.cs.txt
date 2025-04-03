using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class DashingStressTest
{
    public GameObject player;
    public player_movement playerMovementScript;

    [SetUp]
    public void SetUp()
    {
        player = new GameObject("TestPlayer");
        playerMovementScript = player.AddComponent<player_movement>();

        Rigidbody2D rb = player.AddComponent<Rigidbody2D>();
        playerMovementScript.rb = rb;

        playerMovementScript.speed = 5f;
        playerMovementScript.dashSpeed = 10f;
        playerMovementScript.dashTime = 0.5f;
        playerMovementScript.deceleration = 0.9f;
    }

    [UnityTest]
    public IEnumerator StressTestDashing()
    {
        const int testIterations = 100;
        const float testInterval = 0.1f;

        for (int i = 0; i < testIterations; i++)
        {
            playerMovementScript.StartDash();
            Debug.Log($"Dash {i + 1} executed");

            // Wait for the dash duration plus the interval
            yield return new WaitForSeconds(playerMovementScript.dashTime + testInterval);
            Assert.IsFalse(playerMovementScript.isDashing); // Confirm dash state resets
        }
    }

    [TearDown]
    public void TearDown()
    {
        GameObject.Destroy(player);
    }
}
