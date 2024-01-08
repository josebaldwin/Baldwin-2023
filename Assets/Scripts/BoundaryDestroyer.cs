using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryDestroyer : MonoBehaviour
{
    public float minX = -20f;
    public float maxX = 20f;

    void Update()
    {
        // Check if the object's position is outside the specified boundaries
        if (transform.position.x < minX || transform.position.x > maxX)
        {
            // Destroy the object
            Destroy(gameObject);
        }
    }
}
