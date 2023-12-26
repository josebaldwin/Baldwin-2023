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
            // Destroy the missile and the enemy after a short delay
            Destroy(gameObject);
            Destroy(other.gameObject, 0.2f); // Adjust the delay as needed
        }
    }
}