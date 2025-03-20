using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerMovementDashStressTest
{
    private GameObject player;
    private player_movement playerMovementScript;

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
    public IEnumerator StressTestRepeatedDashes()
    {
        int dashCount = 10000; // Number of dashes to simulate
        for (int i = 0; i < dashCount; i++)
        {
            // Trigger the dash
            playerMovementScript.StartDash();
            yield return null;
            Assert.IsTrue(playerMovementScript.isDashing);
            yield return new WaitForSeconds(playerMovementScript.dashTime);
            Assert.IsFalse(playerMovementScript.isDashing);
            playerMovementScript.rb.linearVelocity = Vector2.zero;
        }

        Debug.Log($"Stress test completed: {dashCount} dashes successfully executed.");
    }

    [TearDown]
    public void TearDown()
    {
        GameObject.Destroy(player);
    }
}
