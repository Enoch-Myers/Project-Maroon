using NUnit.Framework;
using UnityEngine;

public class MeleeEnemyTests
{
    private GameObject meleeEnemyGameObject;
    private MeleeEnemy meleeEnemy;
    private GameObject playerGameObject;

    [SetUp]
    public void Setup()
    {
        meleeEnemyGameObject = new GameObject("MeleeEnemy");
        meleeEnemy = meleeEnemyGameObject.AddComponent<MeleeEnemy>();
        meleeEnemy.playerLayer = LayerMask.GetMask("Player");

        playerGameObject = new GameObject("Player");
        playerGameObject.tag = "Player";
        playerGameObject.AddComponent<PlayerHealth>();
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(meleeEnemyGameObject);
        Object.Destroy(playerGameObject);
    }

    [Test]
    public void AttackPlayer_DealsDamageToPlayer()
    {
        PlayerHealth playerHealth = playerGameObject.GetComponent<PlayerHealth>();
        int initialHealth = playerHealth.health;

        meleeEnemy.AttackPlayer(playerGameObject.GetComponent<Collider2D>());
        Assert.AreEqual(initialHealth - 1, playerHealth.health);
    }

    [Test]
    public void AttackCooldown_PreventsImmediateReattack()
    {
        meleeEnemy.AttackPlayer(playerGameObject.GetComponent<Collider2D>());
        Assert.IsFalse(meleeEnemy.canAttack);
    }
}