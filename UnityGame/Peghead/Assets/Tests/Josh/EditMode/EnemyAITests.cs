/*using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

public class EnemyAITests
{
    private GameObject enemyObj;
    private EnemyAI enemy;
    private GameObject playerObj;
    
    [SetUp]
    public void SetUp()
    {
        enemyObj = new GameObject();
        enemy = enemyObj.AddComponent<EnemyAI>();
        enemyObj.AddComponent<Rigidbody2D>();
        enemyObj.AddComponent<BoxCollider2D>();
        
        playerObj = new GameObject();
        playerObj.tag = "Player";
        playerObj.transform.position = new Vector2(0, 0);
        
        enemyObj.transform.position = new Vector2(-5, 0);
        enemy.patrolRange = 5f;
        enemy.detectionRange = 4f;
        enemy.attackRange = 1f;
    }

    [TearDown]
    public void TearDown()
    {
        GameObject.DestroyImmediate(enemyObj);
        GameObject.DestroyImmediate(playerObj);
    }
    
    [UnityTest]
    public IEnumerator PatrolBoundaryTest()
    {
        float initialX = enemyObj.transform.position.x;
        yield return new WaitForSeconds(2f); // Allow time for movement
        
        float currentX = enemyObj.transform.position.x;
        Assert.IsTrue(currentX >= initialX - enemy.patrolRange && currentX <= initialX + enemy.patrolRange, "Enemy exceeded patrol boundary!");
    }

    [UnityTest]
    public IEnumerator AttackRangeBoundaryTest()
    {
        enemyObj.transform.position = new Vector2(0, 0);
        playerObj.transform.position = new Vector2(1.1f, 0); // Slightly beyond attack range
        
        yield return new WaitForSeconds(1f);
        
        Assert.IsFalse(enemy.PlayerDetected() && Vector2.Distance(enemyObj.transform.position, playerObj.transform.position) <= enemy.attackRange, "Enemy incorrectly detected attack range!");
    }

    [UnityTest]
    public IEnumerator StressTestRapidAttacks()
    {
        enemyObj.transform.position = new Vector2(0, 0);
        playerObj.transform.position = new Vector2(0.5f, 0); // Within attack range

        for (int i = 0; i < 50; i++) // Simulate 50 rapid attacks
        {
            if (enemy != null)
            {
                enemy.TakeDamage(1);
            }
            yield return null;
        }

        // Wait for the enemy to be destroyed
        yield return new WaitForSeconds(1.1f);

        if (enemyObj == null || enemy == null)
        {
            Assert.Pass("Enemy successfully destroyed.");
        }
        else
        {
            Assert.Fail("Enemy object still exists when it should be destroyed.");
        }
    }
}
*/