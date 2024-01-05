using System.Collections;
using UnityEngine;

public class EnemyExplosion : MonoBehaviour
{
    public GameObject explosionParticlePrefab; // Assign the explosion particle prefab in the Inspector

    private void OnDestroy()
    {
        PlayExplosionParticle();
    }

    void PlayExplosionParticle()
    {
        if (explosionParticlePrefab != null)
        {
            // Instantiate the explosion particle prefab at the enemy's position
            Instantiate(explosionParticlePrefab, transform.position, Quaternion.identity);
        }
    }
}
//