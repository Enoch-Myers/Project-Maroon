using NUnit.Framework;
using UnityEngine;

public class RangedEnemyTests
{
    private GameObject rangedEnemyGameObject;
    private RangedEnemy rangedEnemy;
    private GameObject playerGameObject;

    [SetUp]
    public void Setup()
    {
        rangedEnemyGameObject = new GameObject("RangedEnemy");
        rangedEnemy = rangedEnemyGameObject.AddComponent<RangedEnemy>();
        rangedEnemy.projectilePrefab = new GameObject("Projectile");

        playerGameObject = new GameObject("Player");
        playerGameObject.tag = "Player";
        playerGameObject.AddComponent<PlayerHealth>();
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(rangedEnemyGameObject);
        Object.Destroy(playerGameObject);
    }

    [Test]
    public void ShootProjectile_SpawnsProjectile()
    {
        rangedEnemy.player = playerGameObject.transform;
        rangedEnemy.StartCoroutine(rangedEnemy.ShootProjectile());
        Assert.IsNotNull(GameObject.Find("Projectile"));
    }
}