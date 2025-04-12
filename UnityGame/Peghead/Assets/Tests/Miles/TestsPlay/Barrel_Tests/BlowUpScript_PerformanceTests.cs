using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BlowUpScript_PerformanceTests
{
    [UnityTest]
    public IEnumerator Explosion_Affects_ManyRigidbodies()
    {
        // Arrange
        var explosionObject = new GameObject("Explosion");
        var blowUpScript = explosionObject.AddComponent<Blow_up_script>();
        explosionObject.transform.position = Vector2.zero;

        int objectCount = 100;
        float radius = 10f;
        List<Rigidbody2D> rbs = new List<Rigidbody2D>();

        for (int i = 0; i < objectCount; i++)
        {
            var obj = new GameObject($"Object_{i}");
            float angle = i * Mathf.PI * 2 / objectCount;
            Vector2 pos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * (radius * 0.8f); // keep inside radius
            obj.transform.position = pos;

            var rb = obj.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 0;
            rbs.Add(rb);
        }

        // Act
        blowUpScript.Invoke("Explode", 0f);
        yield return new WaitForFixedUpdate(); // let physics run

        // Assert
        int movedCount = 0;
        foreach (var rb in rbs)
        {
            if (rb.linearVelocity.magnitude > 0.01f) movedCount++;
        }

        Assert.AreEqual(objectCount, movedCount,
            $"All {objectCount} rigidbodies should be affected by the explosion, but only {movedCount} moved.");
    }
}
