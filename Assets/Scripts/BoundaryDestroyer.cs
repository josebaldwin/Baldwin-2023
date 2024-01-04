using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryDestroyer : MonoBehaviour
{
    public float minX = -20f;
    public float maxX = 20f;
    public float minY = -6f;
    public float maxY = 8f;

    void Update()
    {
        // Check if the object's position is outside the specified boundaries
        if (transform.position.x < minX || transform.position.x > maxX ||
            transform.position.y < minY || transform.position.y > maxY)
        {
            // Destroy the object
            Destroy(gameObject);
        }
    }
}
