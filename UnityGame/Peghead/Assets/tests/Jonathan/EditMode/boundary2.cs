using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class boundary2
{
    private GameObject Boss;
    private NewMonoBehaviourScript BossLogic;

    [SetUp]
    public void Setup(){
        Boss = new GameObject("BossTestObject");
        BossLogic = Boss.AddComponent<NewMonoBehaviourScript>();
        BossLogic.maxHealth = 100;
        BossLogic.Start();
    }

    [TearDown]
    public void Teardown(){
        Object.DestroyImmediate(Boss);
    }

    /// <summary>
    /// Boundary Test #2: Tests that zero damage does not reduce health.
    /// </summary>
    [Test]
    public void BossHealth_ZeroOrNegativeDamageShouldNotReduceHealth(){
        int initialHealth = BossLogic.CurrentHealth;

        BossLogic.TakeDamage(0);
        Assert.AreEqual(initialHealth, BossLogic.CurrentHealth);
    }
}
