using UnityEngine;
using System.Collections;

public class EnemyGun : MonoBehaviour
{
    public GameObject missilePrefab; // Assign the missile prefab in the inspector
    public float fireRate = 2f; // Time between shots, set in the inspector
    public float additionalMissileSpeed = 5f; // Additional speed to add to the missile
    private float nextFireTime;
    private float enemySpeed; // Speed of the enemy
    private EnemyBehavior enemyBehavior; // Reference to the EnemyBehavior script

    void Start()
    {
        nextFireTime = Time.time + fireRate;
        enemyBehavior = GetComponentInParent<EnemyBehavior>();
        if (enemyBehavior != null)
        {
            enemySpeed = enemyBehavior.speed; // Get the speed of the enemy
        }
        else
        {
            Debug.LogError("Enemy behavior script not found on " + gameObject.name);
        }
    }

    void Update()
    {
        if (Time.time >= nextFireTime && !enemyBehavior.isResistanceDepleted)
        {
            FireMissile();
            nextFireTime = Time.time + fireRate;
        }
    }

    void FireMissile()
    {
        if (missilePrefab != null)
        {
            GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.identity);

            Rigidbody rb = missile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 velocityDirection = Vector3.left; // Assumes missile moves leftwards along the X-axis
                rb.velocity = velocityDirection * (enemySpeed + additionalMissileSpeed);

                // Start correcting the missile's Z position
                StartCoroutine(CorrectMissileZPosition(missile));
            }
        }
        else
        {
            Debug.LogError("Missile prefab not assigned for " + gameObject.name);
        }
    }

    IEnumerator CorrectMissileZPosition(GameObject missile)
    {
        float correctionSpeed = 5f; // Adjust this value as needed

        while (missile != null && Mathf.Abs(missile.transform.position.z) > Mathf.Epsilon)
        {
            float correctedZ = Mathf.Lerp(missile.transform.position.z, 0, correctionSpeed * Time.deltaTime);
            missile.transform.position = new Vector3(missile.transform.position.x, missile.transform.position.y, correctedZ);
            yield return null;
        }
    }
}
