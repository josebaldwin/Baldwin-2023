using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Array of enemy prefabs
    public float spawnInterval = 3f;
    public float spawnRadius = 10f;
    public float enemySpeed = 5f; // Adjust this to set the enemy movement speed

    private void Start()
    {
        // InvokeRepeating calls the SpawnEnemy method at regular intervals
        InvokeRepeating("SpawnEnemy", 0f, spawnInterval);
    }

    private void SpawnEnemy()
    {
        // Calculate a random y position within the specified range
        float randomY = Random.Range(-3.5f, 5.5f);

        // Set the desired position and rotation values
        Vector3 spawnPosition = new Vector3(14f, randomY, 0f);
        Quaternion spawnRotation = Quaternion.Euler(0f, -90f, 90f);

        // Randomly choose one of the enemy prefabs from the array
        GameObject selectedEnemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        // Instantiate the selected enemy prefab at the specified position and rotation
        GameObject enemyInstance = Instantiate(selectedEnemyPrefab, spawnPosition, spawnRotation);

        // Set the constant velocity along the X-axis for the enemy
        Rigidbody enemyRigidbody = enemyInstance.GetComponent<Rigidbody>();
        if (enemyRigidbody != null)
        {
            enemyRigidbody.velocity = new Vector3(-enemySpeed, 0f, 0f);
        }
    }
}
