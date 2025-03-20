using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class boundary1
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
    /// Boundary Test #1: Makes sure health never drops below zero even with large damage.
    /// </summary>
    [Test]
    public void BossHealth_NeverBelowZero(){
  
        BossLogic.TakeDamage(9999);

        Assert.AreEqual(0, BossLogic.CurrentHealth);
    }
}
