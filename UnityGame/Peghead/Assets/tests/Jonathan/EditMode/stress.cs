using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class stress
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
    /// Stress Test: Repeated hits eventually reduce health to 0.
    /// </summary>
    [Test]
    public void BossHealth_RepeatedDamageStressTest()
    {
        int count = 0;
        for(int i = 0; i < 1000; i++, count++)BossLogic.TakeDamage(1);
        Assert.AreEqual(0, BossLogic.CurrentHealth);
    }
}
