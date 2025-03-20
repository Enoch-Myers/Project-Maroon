using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerMovementTests
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
        playerMovementScript.minX = -10f;
        playerMovementScript.maxX = 10f;
        playerMovementScript.minY = -5f;
        playerMovementScript.maxY = 5f;
    }

    [Test]
    public void PlayerStartsWithinBoundaries()
    {
        // make sure the player starts inside the defined boundary
        Vector3 startPosition = player.transform.position;
        Assert.IsTrue(startPosition.x >= playerMovementScript.minX && startPosition.x <= playerMovementScript.maxX);
        Assert.IsTrue(startPosition.y >= playerMovementScript.minY && startPosition.y <= playerMovementScript.maxY);
    }

    [UnityTest]
    public IEnumerator PlayerStaysWithinBoundaries()
    {
        player.transform.position = new Vector3(-15f, 0f, 0f); // Position outside left boundary
        yield return null;
        Assert.AreEqual(player.transform.position.x, playerMovementScript.minX);

        player.transform.position = new Vector3(15f, 0f, 0f); // Position outside right boundary
        yield return null;
        Assert.AreEqual(player.transform.position.x, playerMovementScript.maxX);

        player.transform.position = new Vector3(0f, -10f, 0f); // Position outside bottom boundary
        yield return null;
        Assert.AreEqual(player.transform.position.y, playerMovementScript.minY);

        player.transform.position = new Vector3(0f, 10f, 0f); // Position outside top boundary
        yield return null;
        Assert.AreEqual(player.transform.position.y, playerMovementScript.maxY);
    }

    [UnityTest]
    public IEnumerator PlayerDashesCorrectly()
    {
        // Set the player to dash and verify the dash behavior
        playerMovementScript.StartDash();
        yield return new WaitForSeconds(playerMovementScript.dashTime);

        Assert.IsFalse(playerMovementScript.isDashing);

        Assert.AreEqual(0f, playerMovementScript.rb.linearVelocity.x, 0.1f);
    }

    [TearDown]
    public void TearDown()
    {
        GameObject.Destroy(player);
    }
}
