using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BlowUpScript_DirectionTests
{
    [UnityTest]
    public IEnumerator RigidbodyReceivesForce_AwayFromExplosion()
    {
        // Arrange
        var explosionObject = new GameObject("Explosion");
        var blowUpScript = explosionObject.AddComponent<Blow_up_script>();
        explosionObject.transform.position = Vector2.zero;

        var target = new GameObject("Target");
        Vector2 objectPosition = new Vector2(5f, 3f); // place it top-right from explosion
        target.transform.position = objectPosition;

        var rb = target.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0;

        // Act
        blowUpScript.Invoke("Explode", 0f);
        yield return new WaitForFixedUpdate(); // physics update

        // Get direction vectors
        Vector2 explosionToTarget = (Vector2)target.transform.position - (Vector2)explosionObject.transform.position;
        Vector2 velocity = rb.linearVelocity;

        // Assert force direction is generally away in both axes
        Assert.IsTrue(Mathf.Sign(velocity.x) == Mathf.Sign(explosionToTarget.x),
            $"X direction should be away. ExplosionToTarget.x = {explosionToTarget.x}, Velocity.x = {velocity.x}");

        Assert.IsTrue(Mathf.Sign(velocity.y) == Mathf.Sign(explosionToTarget.y),
            $"Y direction should be away. ExplosionToTarget.y = {explosionToTarget.y}, Velocity.y = {velocity.y}");
    }
}
