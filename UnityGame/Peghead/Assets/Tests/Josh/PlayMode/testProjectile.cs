using NUnit.Framework;
using UnityEngine;

public class ProjectileTests
{
    private GameObject projectileGameObject;
    private Projectile projectile;
    private GameObject playerGameObject;

    [SetUp]
    public void Setup()
    {
        projectileGameObject = new GameObject("Projectile");
        projectile = projectileGameObject.AddComponent<Projectile>();

        playerGameObject = new GameObject("Player");
        playerGameObject.tag = "Player";
        playerGameObject.AddComponent<PlayerHealth>();
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(projectileGameObject);
        Object.Destroy(playerGameObject);
    }

    [Test]
    public void OnTriggerEnter2D_DealsDamageToPlayer()
    {
        PlayerHealth playerHealth = playerGameObject.GetComponent<PlayerHealth>();
        int initialHealth = playerHealth.health;

        projectile.OnTriggerEnter2D(playerGameObject.GetComponent<Collider2D>());
        Assert.AreEqual(initialHealth - 1, playerHealth.health);
    }
}