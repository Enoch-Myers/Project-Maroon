using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EnemyAITests
{
    private GameObject enemyGameObject;
    private EnemyAI enemyAI;

    [SetUp]
    public void Setup()
    {
        enemyGameObject = new GameObject("Enemy");
        enemyAI = enemyGameObject.AddComponent<MeleeEnemy>(); // Use MeleeEnemy for testing EnemyAI behavior
        enemyAI.patrolPointA = new GameObject("PointA").transform;
        enemyAI.patrolPointB = new GameObject("PointB").transform;
        enemyAI.patrolPointA.position = new Vector3(0, 0, 0);
        enemyAI.patrolPointB.position = new Vector3(5, 0, 0);
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(enemyGameObject);
    }

    [Test]
    public void Patrol_SwitchesBetweenPoints()
    {
        enemyAI.transform.position = enemyAI.patrolPointA.position;
        enemyAI.Update(); // Simulate one frame
        Assert.AreEqual(enemyAI.patrolPointB.position, enemyAI.transform.position);
    }

    [Test]
    public void TakeDamage_ReducesHealth()
    {
        int initialHealth = enemyAI.health;
        enemyAI.TakeDamage(1);
        Assert.AreEqual(initialHealth - 1, enemyAI.health);
    }

    [Test]
    public void Die_SetsIsDeadToTrue()
    {
        enemyAI.TakeDamage(enemyAI.health); // Reduce health to 0
        Assert.IsTrue(enemyAI.isDead);
    }
}