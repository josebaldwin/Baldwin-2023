using UnityEngine;
using System.Collections;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject[] powerUpPrefabs; // Assign your power-up prefabs here in the Inspector
    public float[] initialSpawnTimes; // Initial spawn times for each power-up type
    public float[] respawnTimes; // Respawn times for each power-up type

    private GameObject shieldInstance; // Reference to the spawned shield instance

    private void Start()
    {
        if (powerUpPrefabs.Length != initialSpawnTimes.Length ||
            powerUpPrefabs.Length != respawnTimes.Length)
        {
            Debug.LogError("The number of power-ups, initial spawn times, and respawn times must be equal");
            return;
        }

        // Spawn each power-up type initially and then continuously respawn them
        for (int i = 0; i < powerUpPrefabs.Length; i++)
        {
            StartCoroutine(SpawnPowerUp(powerUpPrefabs[i], initialSpawnTimes[i], respawnTimes[i]));
        }
    }

    private IEnumerator SpawnPowerUp(GameObject powerUpPrefab, float initialSpawnTime, float respawnTime)
    {
        yield return new WaitForSeconds(initialSpawnTime);

        while (true)
        {
            // Spawn the power-up
            GameObject powerUp = Instantiate(powerUpPrefab, GetRandomSpawnPosition(), Quaternion.identity);

            // Check if the power-up is a shield
            if (powerUpPrefab.tag == "PowerUpShield")
            {
                // Store the shield instance reference
                shieldInstance = powerUp;
            }

            // Wait for the specified respawn time before spawning the next power-up
            yield return new WaitForSeconds(respawnTime);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        // Define ranges for X and Y coordinates
        float minX = -30f, maxX = 30f; // X coordinates range from -39 to 39
        float minY = -12f, maxY = 14f; // Y coordinates range from -11 to 13

        // Generate random positions within the specified ranges
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        // Return a position within the specified ranges at Z = 0
        return new Vector3(randomX, randomY, 0f);
    }
}
