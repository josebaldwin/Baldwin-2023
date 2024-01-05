using System.Collections;
using UnityEngine;

public class EnemyExplosion : MonoBehaviour
{
    public GameObject explosionParticlePrefab; // Assign the explosion particle prefab in the Inspector
    private GameObject explosionInstance;

    private void OnDestroy()
    {
        PlayExplosionParticle();
    }

    void PlayExplosionParticle()
    {
        if (explosionParticlePrefab != null)
        {
            // Instantiate the explosion particle prefab at the enemy's position
            explosionInstance = Instantiate(explosionParticlePrefab, transform.position, Quaternion.identity);

            // Ensure proper cleanup by destroying the instantiated particle effect after a delay
            Destroy(explosionInstance, 2f); // Adjust the delay as needed
        }
    }
}
