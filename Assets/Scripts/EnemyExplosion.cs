using System.Collections;
using UnityEngine;

public class EnemyExplosion : MonoBehaviour
{
    public GameObject explosionParticlePrefab; // Assign in Inspector

    // Method to be called when resistance is depleted
    public void StartDestructionSequence()
    {
        StartCoroutine(DelayedOnKill(2f)); // Wait for 3 seconds before calling OnKill
    }

    private IEnumerator DelayedOnKill(float delay)
    {
        yield return new WaitForSeconds(delay);
        OnKill();
    }

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
