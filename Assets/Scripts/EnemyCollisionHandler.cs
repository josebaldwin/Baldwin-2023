using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisionHandler : MonoBehaviour
{
    private bool hitByMissile = false;

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collision involves a Missile and hasn't been hit yet
        if (collision.gameObject.CompareTag("Missile") && !hitByMissile)
        {
            // Set the flag to true to avoid multiple hits
            hitByMissile = true;

            // Destroy the missile immediately upon impact
            Destroy(collision.gameObject);

            // Start a coroutine to destroy the enemy after a delay (e.g., 2 seconds)
            StartCoroutine(DestroyAfterDelay(2f)); // Adjust the delay as needed
        }
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Destroy the enemy object
        Destroy(gameObject);
    }
}