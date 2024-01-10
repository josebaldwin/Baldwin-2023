using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryDestroyer : MonoBehaviour
{
    public float minX = -100f;
    public float maxX = 100f;
    public float missileBoundaryX = 42f; // Boundary on the X-axis to destroy missiles

    void Update()
    {
        // Check if the object's position is outside the X boundaries
        if (transform.position.x < minX || transform.position.x > maxX)
        {
            // Destroy the object
            Destroy(gameObject);
        }

        // Additional check for missiles
        if (gameObject.CompareTag("Missile") && Mathf.Abs(transform.position.x) > missileBoundaryX)
        {
            // Destroy the missile if it's beyond the missile boundary
            Destroy(gameObject);
        }
    }
}
