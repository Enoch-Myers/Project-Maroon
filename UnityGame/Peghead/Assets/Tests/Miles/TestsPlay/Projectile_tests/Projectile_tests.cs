using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ProjectileBehaviorTests
{
    private GameObject CreateProjectile(float speed = 10f)
    {
        var projectileObj = new GameObject("Projectile");
        var rb = projectileObj.AddComponent<Rigidbody2D>(); // Needed for physics
        rb.gravityScale = 0; // Prevent falling
        var collider = projectileObj.AddComponent<BoxCollider2D>(); // For collisions
        var behavior = projectileObj.AddComponent<Projectile_behavior>();
        behavior.speed = speed;
        return projectileObj;
    }

    [UnityTest]
    public IEnumerator Projectile_MovesInCorrectDirection()
    {
        var projectile = CreateProjectile();
        Vector3 startPos = projectile.transform.position;
        yield return new WaitForSeconds(0.1f);
        Vector3 endPos = projectile.transform.position;

        Assert.Greater(endPos.x, startPos.x, "Projectile should move left along X.");

        Object.Destroy(projectile);
    }

    [UnityTest]
    public IEnumerator Projectile_MovesAtCorrectSpeed()
    {
        float speed = 5f;
        var projectile = CreateProjectile(speed);
        Vector3 startPos = projectile.transform.position;
        yield return new WaitForSeconds(1f);
        Vector3 endPos = projectile.transform.position;

        float distanceMoved = Vector3.Distance(startPos, endPos);
        Assert.That(distanceMoved, Is.EqualTo(speed).Within(0.5f), $"Expected to move ~{speed} units in 1s, but moved {distanceMoved}");

        Object.Destroy(projectile);
    }

    [UnityTest]
    public IEnumerator Projectile_IsDestroyedOnCollision()
    {
        var projectile = CreateProjectile();
        var wall = new GameObject("Wall");
        wall.transform.position = projectile.transform.position + Vector3.left * 0.1f;
        wall.AddComponent<BoxCollider2D>();
        var rb = wall.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;

        yield return new WaitForSeconds(0.2f);

        Assert.IsTrue(projectile == null || projectile.Equals(null), "Projectile should be destroyed on collision.");

        Object.Destroy(wall);
    }

    [UnityTest]
    public IEnumerator Projectile_DoesNotMoveIfSpeedZero()
    {
        var projectile = CreateProjectile(0f);
        Vector3 startPos = projectile.transform.position;
        yield return new WaitForSeconds(0.2f);
        Vector3 endPos = projectile.transform.position;

        Assert.AreEqual(startPos, endPos, "Projectile should not move when speed is 0.");

        Object.Destroy(projectile);
    }

    [UnityTest]
    public IEnumerator Projectile_IsMovingOverMultipleFrames()
    {
        var projectile = CreateProjectile();
        Vector3 initial = projectile.transform.position;
        yield return new WaitForSeconds(0.05f);
        Vector3 mid = projectile.transform.position;
        yield return new WaitForSeconds(0.05f);
        Vector3 final = projectile.transform.position;

        Assert.AreNotEqual(initial, mid, "Projectile did not move between frame 1 and 2.");
        Assert.AreNotEqual(mid, final, "Projectile did not move between frame 2 and 3.");
        Assert.AreNotEqual(initial, final, "Projectile did not move overall.");

        Object.Destroy(projectile);
    }
}


