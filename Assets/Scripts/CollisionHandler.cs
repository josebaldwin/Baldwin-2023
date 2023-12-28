using System.Collections;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public int maxResistance = 5; // Set the maximum resistance value
    private int resistanceCounter = 0;
    private Rigidbody enemyRigidbody;

    void Start()
    {
        // Get the reference to the Rigidbody component
        enemyRigidbody = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collision involves a Missile and hasn't been hit yet
        if (collision.gameObject.CompareTag("Missile"))
        {
            // Destroy the missile immediately upon impact
            Destroy(collision.gameObject);

            if (resistanceCounter < maxResistance)
            {
                // Enemy is resistant, do not get affected
                resistanceCounter++;

                // Optionally, you can play a sound or add other effects for resistant hits
                Debug.Log("Missile collided with a resistant enemy!");

                if (resistanceCounter >= maxResistance)
                {
                    // Set the enemy Rigidbody to be affected by physics forces
                    if (enemyRigidbody != null)
                    {
                        enemyRigidbody.isKinematic = false;
                    }

                    // Start a coroutine to destroy the object after a delay (e.g., 2 seconds)
                    StartCoroutine(DestroyAfterDelay(2f)); // Adjust the delay as needed
                }
            }
        }
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Destroy the object this script is attached to (enemy)
        Destroy(gameObject);
    }
}
