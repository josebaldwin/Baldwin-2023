using System.Collections;
using UnityEngine;

public class EnemyExplosion : MonoBehaviour
{
    public GameObject explosionParticlePrefab; // Assign in Inspector

    public void OnKill()
    {
        PlayExplosionParticle();
        Destroy(gameObject); // Destroy the enemy GameObject
    }

    private void PlayExplosionParticle()
    {
        if (explosionParticlePrefab != null)
        {
            GameObject explosionInstance = Instantiate(explosionParticlePrefab, transform.position, Quaternion.identity);
            Destroy(explosionInstance, 2f); // Adjust as needed
        }
        else
        {
            Debug.LogError("Explosion prefab not assigned for " + gameObject.name);
        }
    }
}
