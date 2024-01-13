using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Array of enemy prefabs
    public float spawnInterval = 3f; // Initial spawn interval
    public float minY = -5f; // Min Y position for spawning
    public float maxY = 5f; // Max Y position for spawning
    private int enemiesDestroyed = 0; // Counter for the number of enemies destroyed
    private int thresholdForFasterSpawn = 10; // Threshold to increase spawn rate

    private void Start()
    {
        // Invoke repeating SpawnEnemy method at the start of the game
        InvokeRepeating("SpawnEnemy", 0f, spawnInterval);
    }

    private void SpawnEnemy()
    {
        // Calculate a random y position within the specified range
        float randomY = Random.Range(minY, maxY);

        // Set the desired position and rotation values
        Vector3 spawnPosition = new Vector3(55f, randomY, 0f); // Adjust spawn X-coordinate as needed
        Quaternion spawnRotation = Quaternion.Euler(0f, -90f, 90f);

        // Randomly choose one of the enemy prefabs from the array
        GameObject selectedEnemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        GameObject enemyInstance = Instantiate(selectedEnemyPrefab, spawnPosition, spawnRotation);

        // Set additional properties or behaviors of the enemy here if needed
        EnemyBehavior enemyBehavior = enemyInstance.GetComponent<EnemyBehavior>();
        if (enemyBehavior != null)
        {
            Rigidbody enemyRigidbody = enemyInstance.GetComponent<Rigidbody>();
            if (enemyRigidbody != null)
            {
                enemyRigidbody.velocity = new Vector3(-enemyBehavior.speed, 0f, 0f);
            }
        }

        SubscribeToEnemyDestruction(enemyInstance);

    }

    // Method to be called when an enemy is destroyed
    public void EnemyDestroyed()
    {
        enemiesDestroyed++;
        if (enemiesDestroyed % thresholdForFasterSpawn == 0)
        {
            IncreaseSpawnRate();
        }
    }

    // Method to increase the spawn rate of enemies
    private void IncreaseSpawnRate()
    {
        if (spawnInterval > 0.015f) // New lower limit for spawn interval
        {
            spawnInterval /= 1.2f;
            CancelInvoke("SpawnEnemy");
            InvokeRepeating("SpawnEnemy", 0f, spawnInterval);
        }
        else
        {
            Debug.Log("Spawn interval has reached its minimum limit.");
        }
    }
    private void SubscribeToEnemyDestruction(GameObject enemy)
    {
        EnemyExplosion explosionComponent = enemy.GetComponent<EnemyExplosion>();
        if (explosionComponent != null)
        {
            explosionComponent.OnKill += EnemyDestroyed;
        }
    }
}
