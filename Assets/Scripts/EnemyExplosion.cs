using System;
using System.Collections;
using UnityEngine;

public class EnemyExplosion : MonoBehaviour
{
    public GameObject explosionParticlePrefab; // Assign in Inspector
    public event Action OnKill; // Event to be called when the enemy is killed
    public int scoreValue = 10; // Score value for this enemy

    // Method to be called when resistance is depleted
    public void StartDestructionSequence()
    {
        StartCoroutine(DelayedOnKill(2f)); // Wait for 3 seconds before calling OnKill
    }

    private IEnumerator DelayedOnKill(float delay)
    {
        yield return new WaitForSeconds(delay);
        OnKillMethod();
    }

    public void OnKillMethod()
    {
        // Update the score before destroying the enemy
        ScoreManager.Instance.AddScore(scoreValue);

        OnKill?.Invoke(); // Invoke the OnKill event
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
