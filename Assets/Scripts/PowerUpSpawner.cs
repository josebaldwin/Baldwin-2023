using UnityEngine;
using System.Collections;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject[] powerUpPrefabs; // Assign your 4 power-up prefabs here in the Inspector
    public float[] spawnTimes; // Times after which each power-up should spawn

    private void Start()
    {
        if (powerUpPrefabs.Length != spawnTimes.Length)
        {
            Debug.LogError("The number of power-ups and spawn times must be equal");
            return;
        }

        for (int i = 0; i < spawnTimes.Length; i++)
        {
            StartCoroutine(SpawnPowerUp(i, spawnTimes[i]));
        }
    }

    private IEnumerator SpawnPowerUp(int powerUpIndex, float delay)
    {
        yield return new WaitForSeconds(delay);

        GameObject powerUp = Instantiate(powerUpPrefabs[powerUpIndex], GetRandomSpawnPosition(), Quaternion.identity);

        // Set the scale of the power-up. Adjust these values as needed.
        //float scaleFactor = 3.0f; // Example scale factor (2 times the original size)
        //powerUp.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

        // If you need non-uniform scaling, set the scale for each axis separately:
        // powerUp.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
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
//