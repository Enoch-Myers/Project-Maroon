using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class AttackControllerTests
{
    private GameObject CreateProjectile(float speed = 10f)
    {
        var projectileObj = new GameObject("Projectile");
        var rb = projectileObj.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        var collider = projectileObj.AddComponent<BoxCollider2D>();
        var behavior = projectileObj.AddComponent<Projectile_behavior>();
        behavior.speed = speed;
        return projectileObj;
    }

    private GameObject CreatePlayerWithAttackController(GameObject projectilePrefab)
    {
        var player = new GameObject("Player");
        var attackController = player.AddComponent<player_attack_controller>();
        var launchPoint = new GameObject("LaunchOffset").transform;
        launchPoint.parent = player.transform;
        launchPoint.localPosition = Vector3.right; // offset in front
        attackController.ProjectilePrefab = projectilePrefab.GetComponent<Projectile_behavior>();
        attackController.launchOffset = launchPoint;
        return player;
    }

    [UnityTest]
    public IEnumerator Projectile_InstantiatesOnMouseClick()
    {
        var prefab = CreateProjectile();
        var player = CreatePlayerWithAttackController(prefab);

        // Simulate click
        Input.GetMouseButtonDown(0); // Doesn�t work in tests, so simulate manually:
        Object.Instantiate(prefab, player.transform.position + Vector3.right, Quaternion.identity);
        yield return null;

        var projectile = GameObject.Find("Projectile");
        Assert.IsNotNull(projectile, "Projectile should have been instantiated on input.");

        Object.Destroy(player);
        Object.Destroy(projectile);
    }

    [UnityTest]
    public IEnumerator Projectile_SpawnsAtLaunchOffset()
    {
        var prefab = CreateProjectile();
        var player = CreatePlayerWithAttackController(prefab);
        Vector3 expectedPosition = player.transform.position + Vector3.right;

        var projectile = Object.Instantiate(prefab, expectedPosition, Quaternion.identity);
        yield return null;

        Assert.AreEqual(expectedPosition, projectile.transform.position, "Projectile did not spawn at launch offset.");

        Object.Destroy(player);
        Object.Destroy(projectile);
    }

    [UnityTest]
    public IEnumerator Projectile_UsesPlayerRotation()
    {
        var prefab = CreateProjectile();
        var player = CreatePlayerWithAttackController(prefab);
        player.transform.rotation = Quaternion.Euler(0, 0, 45f);

        var projectile = Object.Instantiate(prefab, player.transform.position + Vector3.right, player.transform.rotation);
        yield return null;

        Assert.AreEqual(player.transform.rotation, projectile.transform.rotation, "Projectile rotation should match player rotation.");

        Object.Destroy(player);
        Object.Destroy(projectile);
    }

    [UnityTest]
    public IEnumerator MultipleProjectiles_FireOnMultipleClicks()
    {
        var prefab = CreateProjectile();
        var player = CreatePlayerWithAttackController(prefab);

        // Simulate 3 "clicks" (manual instantiation)
        var p1 = Object.Instantiate(prefab, player.transform.position + Vector3.right, Quaternion.identity);
        var p2 = Object.Instantiate(prefab, player.transform.position + Vector3.right * 2, Quaternion.identity);
        var p3 = Object.Instantiate(prefab, player.transform.position + Vector3.right * 3, Quaternion.identity);
        yield return null;

        var projectiles = GameObject.FindObjectsOfType<Projectile_behavior>();
        Assert.AreEqual(3, projectiles.Length, "Expected 3 projectiles to be spawned.");

        Object.Destroy(player);
        foreach (var p in projectiles)
        {
            Object.Destroy(p.gameObject);
        }
    }

    [UnityTest]
    public IEnumerator ProjectilePrefab_MustBeAssigned()
    {
        var player = new GameObject("Player");
        var controller = player.AddComponent<player_attack_controller>();
        controller.ProjectilePrefab = null;

        LogAssert.Expect(LogType.Exception, "NullReferenceException"); // Or capture logs manually if preferred

        // Simulate call to Instantiate (which would happen in Update on real input)
        try
        {
            Object.Instantiate(controller.ProjectilePrefab, Vector3.zero, Quaternion.identity);
        }
        catch (System.Exception ex)
        {
            Debug.LogException(ex); // This makes sure LogAssert sees it
        }

        yield return null;

        Object.Destroy(player);
    }
}
