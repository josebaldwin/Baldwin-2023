using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    public GameObject missilePrefab; // Assign the missile prefab in the inspector
    public float fireRate = 2f; // Time between shots, set in the inspector
    public float missileSpeed = 10f; // Speed of the missile
    private float nextFireTime;

    void Start()
    {
        nextFireTime = Time.time + fireRate;
    }

    void Update()
    {
        // Check if it's time to fire
        if (Time.time >= nextFireTime)
        {
            FireMissile();
            nextFireTime = Time.time + fireRate;
        }
    }

    void FireMissile()
    {
        if (missilePrefab != null)
        {
            // Instantiate the missile
            GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.identity);

            // Set the missile's velocity
            Rigidbody rb = missile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 velocityDirection = Vector3.left; // This ensures the missile moves leftwards along the X-axis

                // Correct the Z position of the missile to remain at 0
                missile.transform.position = new Vector3(missile.transform.position.x, missile.transform.position.y, 0f);

                rb.velocity = velocityDirection * missileSpeed;
            }
        }
        else
        {
            Debug.LogError("Missile prefab not assigned for " + gameObject.name);
        }
    }
}
