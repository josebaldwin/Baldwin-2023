using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryDestroyer : MonoBehaviour
{
    public float minX = -20f;
    public float maxX = 20f;
    public float minY = -3.5f; // Min Y position boundary
    public float maxY = 5f;    // Max Y position boundary
    private float fixedZ = 0f;  // Fixed Z position for all enemies
    public float missileBoundaryX = 16f; // Boundary on the X-axis to destroy missiles

    void Update()
    {
        // Clamp the Y position within the specified boundaries
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);
        transform.position = new Vector3(transform.position.x, clampedY, fixedZ);

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
