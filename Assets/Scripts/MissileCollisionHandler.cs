using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileCollisionHandler : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to an enemy
        if (other.CompareTag("Enemy"))
        {
            // Destroy the missile immediately upon impact
            Destroy(gameObject);

            // Access the Rigidbody of the enemy and set it as not kinematic (if using Rigidbody)
            Rigidbody enemyRigidbody = other.GetComponent<Rigidbody>();
            if (enemyRigidbody != null)
            {
                enemyRigidbody.isKinematic = false;
            }

            // Start a coroutine to destroy the enemy after a delay (e.g., 0.2 seconds)
            StartCoroutine(DestroyEnemyAfterDelay(other.gameObject, 0.2f)); // Adjust the delay as needed
        }
    }

    IEnumerator DestroyEnemyAfterDelay(GameObject enemy, float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Destroy the enemy object
        Destroy(enemy);
    }
}