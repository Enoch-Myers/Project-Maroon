using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Coin_test
{
    private GameObject player;
    private Coin_despawner CM;
    [SetUp]
    public void SetUP()
    {
        player = new GameObject("Player");
        CM = player.AddComponent<Coin_despawner>();
        CM.coinCount = 0; // start coin value at 0
    }
    [UnityTest]
    public IEnumerator CoinShouldNotGoBelowZero()
    {
        int coinStartCount = CM.coinCount;//start coin count
        Debug.Log($"Initial coin count: {coinStartCount}");

        // Capture Consle log output
        string logMessage = string.Empty;
        Application.logMessageReceived += (condition, stackTrace, type) => logMessage = condition;

        // Remove coins to below 0
        CM.coinCount -= coinStartCount + 1;

        yield return null; // Skip fram to remove coins

        // Check coins stay above 0
        //Assert.AreEqual(0, CM.coinCount, "Coins should not go below zero");
    }
    [UnityTest]
    public IEnumerator CoinValueGoesUp()
    {
        int coinStartCount = CM.coinCount;
        Debug.Log($"Initial coin count: {coinStartCount}");

        // capture console log output
        string logMessage = string.Empty;
        Application.logMessageReceived += (condition, stackTrace, type) => logMessage = condition;

        // Add coins
        CM.coinCount++;

        yield return null;

        //check if value went up
        Assert.Greater(coinStartCount, CM.coinCount);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(player); // Kills player object after test
    }
}
