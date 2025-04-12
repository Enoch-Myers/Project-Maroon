using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BarrelBlowUpTestPlayMode
{
    [UnityTest]
    public IEnumerator Explode_AppliesForceToNearbyRigidbody()
    {
        // Arrange
        var explosionObject = new GameObject("Explosion");
        var blowUpScript = explosionObject.AddComponent<Blow_up_script>();
        explosionObject.transform.position = Vector2.zero;

        var targetObject = new GameObject("Target");
        targetObject.transform.position = new Vector2(1, 0);
        var rb2D = targetObject.AddComponent<Rigidbody2D>();
        rb2D.bodyType = RigidbodyType2D.Dynamic;
        rb2D.gravityScale = 0;

        Vector2 initialVelocity = rb2D.linearVelocity;

        // Act
        blowUpScript.Invoke("Explode", 0f);
        yield return new WaitForFixedUpdate(); // wait for physics update

        // Assert
        Assert.AreNotEqual(initialVelocity, rb2D.linearVelocity, "Rigidbody2D should have been affected by explosion force.");
    }
}
