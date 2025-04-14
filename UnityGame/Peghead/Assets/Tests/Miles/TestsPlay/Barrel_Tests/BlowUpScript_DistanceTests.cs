using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BlowUpScript_DistanceTests
{
    [UnityTest]
    public IEnumerator RigidbodyOutsideRadius_DoesNotMove()
    {
        // Arrange
        var explosionObject = new GameObject("Explosion");
        var blowUpScript = explosionObject.AddComponent<Blow_up_script>();
        explosionObject.transform.position = Vector2.zero;

        // Create a target object outside the explosion radius
        var targetObject = new GameObject("Target");
        targetObject.transform.position = new Vector2(100, 0); // way outside default radius of 15
        var rb2D = targetObject.AddComponent<Rigidbody2D>();
        rb2D.bodyType = RigidbodyType2D.Dynamic;
        rb2D.gravityScale = 0;

        Vector2 initialVelocity = rb2D.linearVelocity;

        // Act
        blowUpScript.Invoke("Explode", 0f);
        yield return new WaitForFixedUpdate(); // Wait for physics to apply (if any)

        // Assert
        Assert.AreEqual(initialVelocity, rb2D.linearVelocity,
            "Rigidbody2D outside explosion radius should not be affected.");
    }
}
