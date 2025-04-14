using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BlowUpScript_RadiusEdgeTests
{
    [UnityTest]
    public IEnumerator RigidbodyJustInsideRadius_ReceivesForce()
    {
        // Arrange
        var explosionObject = new GameObject("Explosion");
        var blowUpScript = explosionObject.AddComponent<Blow_up_script>();
        explosionObject.transform.position = Vector2.zero;

        float explosionRadius = 15f; // match default from your script
        float justInsideDistance = explosionRadius - 0.01f;

        var targetObject = new GameObject("Target");
        targetObject.transform.position = new Vector2(justInsideDistance, 0); // just inside the radius
        var rb2D = targetObject.AddComponent<Rigidbody2D>();
        rb2D.bodyType = RigidbodyType2D.Dynamic;
        rb2D.gravityScale = 0;

        Vector2 initialVelocity = rb2D.linearVelocity;

        // Act
        blowUpScript.Invoke("Explode", 0f);
        yield return new WaitForFixedUpdate(); // Wait for physics

        // Assert
        Assert.AreNotEqual(initialVelocity, rb2D.linearVelocity,
            "Rigidbody2D just inside explosion radius should have been affected by force.");
    }
}
