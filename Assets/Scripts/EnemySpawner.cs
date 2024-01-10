using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Array of enemy prefabs
    public float spawnInterval = 3f;
    public float spawnRadius = 10f;
    public float enemySpeed = 5f; // Enemy movement speed
    public float minY = -5f; // Min Y position for spawning
    public float maxY = 5f; // Max Y position for spawning
    

    private void Start()
    {
        // InvokeRepeating calls the SpawnEnemy method at regular intervals
        InvokeRepeating("SpawnEnemy", 0f, spawnInterval);
    }

   
    private void SpawnEnemy()
    {
        // Calculate a random y position within the specified range
        float randomY = Random.Range(minY, maxY);

        // Set the desired position and rotation values
        Vector3 spawnPosition = new Vector3(20f, randomY, 0f);
        Quaternion spawnRotation = Quaternion.Euler(0f, -90f, 90f);

        // Randomly choose one of the enemy prefabs from the array
        GameObject selectedEnemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        // Instantiate the selected enemy prefab at the specified position and rotation
        GameObject enemyInstance = Instantiate(selectedEnemyPrefab, spawnPosition, spawnRotation);

        // Use the speed defined in the enemy's own EnemyBehavior script
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
