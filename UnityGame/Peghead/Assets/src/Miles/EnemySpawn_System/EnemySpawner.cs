using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField]
    private GameObject GroundEnemy;
    [SerializeField]
    private GameObject FlyingEnemy;

    [SerializeField]
    private float groundSpawnInterval = 60f;
    [SerializeField]
    private float flyingSpawnInterval = 30f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(spawnEnemy(groundSpawnInterval, GroundEnemy));
        StartCoroutine(spawnEnemy(flyingSpawnInterval, FlyingEnemy));

    }

    private IEnumerator spawnEnemy(float spawnInterval, GameObject enemy)
    {
        GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0), Quaternion.identity);
        StartCoroutine(spawnEnemy(spawnInterval, enemy));
        yield return new WaitForSeconds(spawnInterval);
    }
}
