using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EnemyDamageTests
{
    GameObject enemyGO;
    GameObject projectileGO;

    private GameObject SpawnEnemy(int health = 3)
    {
        enemyGO = new GameObject("Enemy");
        var rb = enemyGO.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        enemyGO.AddComponent<BoxCollider2D>().isTrigger = true;
        var ai = enemyGO.AddComponent<EnemyAI>();
        ai.health = health;
        return enemyGO;
    }

    private GameObject SpawnProjectile(string tag = "Projectile", bool isTrigger = true)
    {
        projectileGO = new GameObject("Projectile");
        var rb = projectileGO.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        var collider = projectileGO.AddComponent<BoxCollider2D>();
        collider.isTrigger = isTrigger;
        projectileGO.tag = tag;
        projectileGO.AddComponent<Projectile_behavior>();
        return projectileGO;
    }

    [UnityTearDown]
    public IEnumerator Cleanup()
    {
        if (enemyGO) Object.Destroy(enemyGO);
        if (projectileGO) Object.Destroy(projectileGO);
        yield return null;
    }

    [UnityTest]
    public IEnumerator Enemy_Takes_Damage_On_Projectile_Collision()
    {
        var enemy = SpawnEnemy();
        var projectile = SpawnProjectile();

        yield return null;
        enemy.transform.position = Vector2.zero;
        projectile.transform.position = Vector2.zero;

        yield return new WaitForFixedUpdate();

        Assert.Less(enemy.GetComponent<EnemyAI>().health, 3);
    }

    [UnityTest]
    public IEnumerator Enemy_Dies_When_Health_Reaches_Zero()
    {
        var enemy = SpawnEnemy(1);
        var projectile = SpawnProjectile();

        enemy.transform.position = Vector2.zero;
        projectile.transform.position = Vector2.zero;

        yield return new WaitForSeconds(1f);

        Assert.IsTrue(enemy == null || enemy.GetComponent<EnemyAI>()?.health <= 0);
    }

    [UnityTest]
    public IEnumerator Projectile_Is_Destroyed_On_Impact()
    {
        var enemy = SpawnEnemy();
        var projectile = SpawnProjectile();

        enemy.transform.position = Vector2.zero;
        projectile.transform.position = Vector2.zero;

        yield return new WaitForSeconds(1f);

        Assert.IsTrue(projectile == null);
    }

    [UnityTest]
    public IEnumerator Enemy_Ignores_Damage_After_Death()
    {
        var enemy = SpawnEnemy(1);
        var projectile1 = SpawnProjectile();
        enemy.transform.position = Vector2.zero;
        projectile1.transform.position = Vector2.zero;

        yield return new WaitForSeconds(0.5f);

        var projectile2 = SpawnProjectile();
        projectile2.transform.position = Vector2.zero;

        yield return new WaitForSeconds(0.5f);

        Assert.IsTrue(enemy == null || enemy.GetComponent<EnemyAI>()?.health <= 0);
    }

    [UnityTest]
    public IEnumerator Multiple_Projectiles_Reduce_Health_Cumulatively()
    {
        var enemy = SpawnEnemy(3);
        for (int i = 0; i < 3; i++)
        {
            var p = SpawnProjectile();
            p.transform.position = Vector2.zero;
            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(0.5f);
        Assert.IsTrue(enemy == null || enemy.GetComponent<EnemyAI>().health <= 0);
    }

    [UnityTest]
    public IEnumerator Projectile_With_Wrong_Tag_Does_Not_Damage()
    {
        var enemy = SpawnEnemy(3);
        var projectile = SpawnProjectile("NotAProjectile");
        enemy.transform.position = Vector2.zero;
        projectile.transform.position = Vector2.zero;

        yield return new WaitForSeconds(0.5f);

        Assert.AreEqual(3, enemy.GetComponent<EnemyAI>().health);
    }

    [UnityTest]
    public IEnumerator Projectile_Must_Have_Trigger_To_Damage()
    {
        var enemy = SpawnEnemy(3);
        var projectile = SpawnProjectile("Projectile", isTrigger: false);
        enemy.transform.position = Vector2.zero;
        projectile.transform.position = Vector2.zero;

        yield return new WaitForSeconds(0.5f);

        Assert.AreEqual(3, enemy.GetComponent<EnemyAI>().health);
    }
}
