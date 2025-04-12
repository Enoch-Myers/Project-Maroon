using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

public class CoinCollectionPerformanceTests
{
    private const int CoinCount = 1000; // Test with 1000 coins

    [UnityTest]
    public IEnumerator CollectingManyCoins_PerformanceTest()
    {
        // Setup Coin Manager
        var coinManagerObj = new GameObject("CoinManager");
        var coinManager = coinManagerObj.AddComponent<Coin_despawner>();

        // Setup Player with coinPickup script
        var playerObj = new GameObject("Player");
        var pickupScript = playerObj.AddComponent<coinPickup>();
        pickupScript.CM = coinManager;
        var playerCollider = playerObj.AddComponent<BoxCollider2D>();
        playerCollider.isTrigger = true;
        playerObj.transform.position = Vector2.zero;

        // Create a large number of coins in the scene
        for (int i = 0; i < CoinCount; i++)
        {
            var coinObj = new GameObject("Coin_" + i);
            coinObj.tag = "Coins";
            coinObj.transform.position = new Vector2(i % 50, i / 50); // Spread coins in grid pattern
            var coinCollider = coinObj.AddComponent<BoxCollider2D>();
            coinCollider.isTrigger = true;
        }

        // Simulate collecting coins one by one
        float startTime = Time.realtimeSinceStartup;
        for (int i = 0; i < CoinCount; i++)
        {
            var coinObj = GameObject.Find("Coin_" + i);
            if (coinObj != null)
            {
                // Manually simulate trigger by calling OnTriggerEnter2D
                pickupScript.OnTriggerEnter2D(coinObj.GetComponent<Collider2D>());
                Object.Destroy(coinObj); // Simulate coin destruction
            }
        }

        // Wait for physics to update
        yield return new WaitForFixedUpdate();

        // Measure time taken to collect all coins
        float endTime = Time.realtimeSinceStartup;
        float totalTime = endTime - startTime;

        // Assert: Make sure the performance is acceptable (adjust threshold as needed)
        Assert.Less(totalTime, 5f, $"Performance Test Failed! It took {totalTime} seconds to collect {CoinCount} coins.");
    }
}

