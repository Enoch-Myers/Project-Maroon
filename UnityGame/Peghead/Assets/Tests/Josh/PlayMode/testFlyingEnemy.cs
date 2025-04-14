using NUnit.Framework;
using UnityEngine;

public class FlyingEnemyTests
{
    private GameObject flyingEnemyGameObject;
    private FlyingEnemy flyingEnemy;
    private GameObject playerGameObject;

    [SetUp]
    public void Setup()
    {
        flyingEnemyGameObject = new GameObject("FlyingEnemy");
        flyingEnemy = flyingEnemyGameObject.AddComponent<FlyingEnemy>();
        flyingEnemy.pointA = new GameObject("PointA").transform;
        flyingEnemy.pointB = new GameObject("PointB").transform;
        flyingEnemy.pointA.position = new Vector3(0, 0, 0);
        flyingEnemy.pointB.position = new Vector3(5, 0, 0);

        playerGameObject = new GameObject("Player");
        playerGameObject.tag = "Player";
        playerGameObject.AddComponent<PlayerHealth>();
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(flyingEnemyGameObject);
        Object.Destroy(playerGameObject);
    }

    [Test]
    public void SwoopDownAndAttack_AttacksPlayer()
    {
        flyingEnemy.player = playerGameObject.transform;
        flyingEnemy.StartCoroutine(flyingEnemy.SwoopDownAndAttack());
        Assert.IsFalse(flyingEnemy.canAttack); // Ensure cooldown starts
    }
}