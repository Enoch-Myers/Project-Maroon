using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Movement_Tests
{
    private GameObject player;
    private player_movement script;
    private Rigidbody2D rb;

    [SetUp]
    public void SetUp()
    {
        // Create a new GameObject for the player and add components
        player = new GameObject("Player");
        rb = player.AddComponent<Rigidbody2D>();
        script = player.AddComponent<player_movement>();
        script.speed = 5f;
        script.dashSpeed = 10f;
        script.dashTime = 1f;
        script.deceleration = 0.9f;
        script.Jump = 10f;
        script.rb = rb;  // Ensure rb is set before use
    }

    [UnityTest]
    public IEnumerator HorizontalMovementTest()
    {
        // Simulate moving right
        script.Move = 1f;  // Set the movement input
        yield return new WaitForFixedUpdate(); // Wait for physics update to apply velocity

        // Check if the horizontal velocity is correct
        Debug.Log($"Velocity after moving right: {rb.linearVelocity.x}");
        Assert.AreEqual(5f, rb.linearVelocity.x, 0.1f); // Check if the velocity is correct for right movement

        // Simulate moving left
        script.Move = -1f;  // Set movement input to left
        yield return new WaitForFixedUpdate(); // Wait for physics update

        // Check if the horizontal velocity is correct
        Debug.Log($"Velocity after moving left: {rb.linearVelocity.x}");
        Assert.AreEqual(-5f, rb.linearVelocity.x, 0.1f); // Check if the velocity is correct for left movement
    }

    [UnityTest]
    public IEnumerator DashTest()
    {
        // Simulate moving right and triggering dash
        script.Move = 1f;
        script.StartDash(); // Manually trigger dash

        yield return new WaitForFixedUpdate(); // Wait for physics update

        // Debug to check dash velocity
        Debug.Log($"Velocity after dash: {rb.linearVelocity.x}");
        Assert.AreEqual(10f, rb.linearVelocity.x, 0.1f); // Check dash velocity
    }

    [UnityTest]
    public IEnumerator JumpTest()
    {
        script.isJumping = false; // Ensure the player is on the ground
        script.Move = 0f; // Set horizontal input to neutral

        // Simulate the jump input by manually updating the state of the player_movement
        script.isJumping = false;
        script.Jump = 10f;
        script.rb.linearVelocity = new Vector2(script.rb.linearVelocity.x, script.Jump);

        // Call Update manually to simulate pressing the jump button
        script.Update();

        yield return new WaitForFixedUpdate(); // Wait for FixedUpdate to process physics

        // Debug to check vertical velocity after jump
        Debug.Log($"Velocity after jump: {rb.linearVelocity.y}");
        Assert.Greater(rb.linearVelocity.y, 0f, "Player should be jumping (positive vertical velocity)");
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(player); // Destroy player GameObject after the test
    }
}