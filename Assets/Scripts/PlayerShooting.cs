using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject missilePrefab; // Assign the Missile Prefab in the Inspector
    public KeyCode shootKey = KeyCode.Space;
    public float shootingCooldown = 0.5f; // Adjust this to control the shooting rate
    public float missileSpeed = 10f; // Adjust this to control missile speed

    private float nextShootTime;

    void Update()
    {
        // Check if enough time has passed since the last shot
        if (Time.time > nextShootTime)
        {
            // Check for the shoot key input
            if (Input.GetKeyDown(shootKey))
            {
                // Shoot the missile
                ShootMissile();

                // Set the next allowed shoot time
                nextShootTime = Time.time + shootingCooldown;
            }
        }
    }

    void ShootMissile()
    {
        // Instantiate the Missile Prefab at the player's position without rotation
        GameObject missileInstance = Instantiate(missilePrefab, transform.position, Quaternion.identity);

        // Access the Collider of the missile and set it as not a trigger (assuming you're using a Collider)
        Collider missileCollider = missileInstance.GetComponent<Collider>();
        if (missileCollider != null)
        {
            missileCollider.isTrigger = false;
        }

        // Access the Rigidbody of the missile and set its velocity along the global X-axis
        Rigidbody missileRigidbody = missileInstance.GetComponent<Rigidbody>();
        if (missileRigidbody != null)
        {
            missileRigidbody.velocity = new Vector3(missileSpeed, 0f, 0f); // Constant velocity in the X direction
        }
    }
}