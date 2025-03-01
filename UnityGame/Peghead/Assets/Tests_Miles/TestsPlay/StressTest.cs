using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;

public class StressTest_SpawnEnemies
{
    private GameObject enemyPrefab; // Reference to the enemy prefab
    private int totalEnemies = 10000; // Number of enemies to spawn
    private float spawnAreaRadius = 50f; // Area radius in which enemies are spawned

    [SetUp]
    public void SetUp()
    {
        // Load the enemy prefab from the specific filepath
        string prefabPath = "Assets/src/Josh/Enemy/Enemy Behaviors/Enemy.prefab";
        enemyPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

        // Assert that the prefab was successfully loaded
        Assert.IsNotNull(enemyPrefab, "Enemy prefab not found at path: " + prefabPath);
    }

    [UnityTest]
    public IEnumerator SpawnEnemiesStressTest()
    {
        // Start measuring the time
        float startTime = Time.realtimeSinceStartup;

        // Spawn enemies at random positions
        for (int i = 0; i < totalEnemies; i++)
        {
            Vector3 spawnPosition = new Vector3(
                Random.Range(-spawnAreaRadius, spawnAreaRadius),
                0,
                Random.Range(-spawnAreaRadius, spawnAreaRadius)
            );

            // Instantiate the enemy prefab
            GameObject enemy = GameObject.Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            // Optionally, destroy enemies after spawning for cleanup
            if (i % 100 == 0) // Only clean up some enemies to reduce memory load
            {
                GameObject.Destroy(enemy);
            }

            if (i % 1000 == 0)
            {
                yield return null; // Yield every 1000 enemies to avoid freezing the editor
            }
        }

        // Measure how long it took to spawn all enemies
        float endTime = Time.realtimeSinceStartup;
        float timeTaken = endTime - startTime;

        // Output the time for the test
        Debug.Log($"Time taken to spawn {totalEnemies} enemies: {timeTaken} seconds");

        // Optional: Assert time doesn't exceed a certain threshold
        Assert.Less(timeTaken, 5f, "It took too long to spawn all enemies!");

        // Ensure no errors occurred during the spawning process
        Assert.Pass("Stress Test Passed");
    }

    [TearDown]
    public void TearDown()
    {
        // Optionally destroy all remaining enemies or reset the scene
        GameObject[] remainingEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in remainingEnemies)
        {
            GameObject.Destroy(enemy);
        }
    }
}
