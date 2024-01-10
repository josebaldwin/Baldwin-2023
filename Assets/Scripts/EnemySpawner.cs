using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Array of enemy prefabs
    public float spawnInterval = 3f;
    public float minY = -5f; // Min Y position for spawning
    public float maxY = 5f; // Max Y position for spawning

    private void Start()
    {
        // InvokeRepeating calls the SpawnEnemy method at regular intervals
        InvokeRepeating("SpawnEnemy", 0f, spawnInterval);
    }

    private void SpawnEnemy()
    {
        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPosition = new Vector3(55f, randomY, 0f); // Adjust spawn X-coordinate as needed
        Quaternion spawnRotation = Quaternion.Euler(0f, -90f, 90f);

        GameObject selectedEnemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        GameObject enemyInstance = Instantiate(selectedEnemyPrefab, spawnPosition, spawnRotation);

        EnemyBehavior enemyBehavior = enemyInstance.GetComponent<EnemyBehavior>();
        if (enemyBehavior != null)
        {
            Rigidbody enemyRigidbody = enemyInstance.GetComponent<Rigidbody>();
            if (enemyRigidbody != null)
            {
                enemyRigidbody.velocity = new Vector3(-enemyBehavior.speed, 0f, 0f);
            }
        }
    }
}
