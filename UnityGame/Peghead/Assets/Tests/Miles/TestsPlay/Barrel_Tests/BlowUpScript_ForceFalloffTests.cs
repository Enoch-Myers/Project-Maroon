using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BlowUpScript_ForceFalloffTests
{
    [UnityTest]
    public IEnumerator CloserObjectReceivesMoreForce()
    {
        // Arrange
        var explosionObject = new GameObject("Explosion");
        var blowUpScript = explosionObject.AddComponent<Blow_up_script>();
        explosionObject.transform.position = Vector2.zero;

        // Close target
        var closeObj = new GameObject("CloseObject");
        closeObj.transform.position = new Vector2(2f, 0);
        var rbClose = closeObj.AddComponent<Rigidbody2D>();
        rbClose.bodyType = RigidbodyType2D.Dynamic;
        rbClose.gravityScale = 0;

        // Far target
        var farObj = new GameObject("FarObject");
        farObj.transform.position = new Vector2(8f, 0);
        var rbFar = farObj.AddComponent<Rigidbody2D>();
        rbFar.bodyType = RigidbodyType2D.Dynamic;
        rbFar.gravityScale = 0;

        // Act
        blowUpScript.Invoke("Explode", 0f);
        yield return new WaitForFixedUpdate(); // physics step

        float closeSpeed = rbClose.linearVelocity.magnitude;
        float farSpeed = rbFar.linearVelocity.magnitude;

        // Assert
        Assert.Greater(closeSpeed, farSpeed,
            $"Closer object should receive more force. Close speed: {closeSpeed}, Far speed: {farSpeed}");
    }
}
