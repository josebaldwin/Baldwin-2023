using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour
{
    public GameObject missilePrefab; // Assign the Missile Prefab in the Inspector
    public KeyCode shootKey = KeyCode.Space;
    public float shootingCooldown = 0.5f; // Adjust this to control the shooting rate
    public float missileSpeed = 10f; // Adjust this to control missile speed

    private float nextShootTime;
    private bool isDoubleFireRateActive = false;
    private bool isHomingActive = false; // Remove homing missile related fields

    public float homingTurnSpeed = 5f; // Turn speed of homing missiles
    public float homingMissileSpeed = 15f; // Speed of homing missiles


    void Update()
    {
        if (Input.GetKey(shootKey) && Time.time > nextShootTime)
        {
            ShootMissile();
            nextShootTime = Time.time + (isDoubleFireRateActive ? shootingCooldown / 2 : shootingCooldown);
        }
    }

    public void DoubleFireRate(bool enable)
    {
        isDoubleFireRateActive = enable;
    }

    public void EnableHoming(bool enable)
    {
        isHomingActive = enable;
    }

    private void ShootMissile()
    {
        GameObject missileInstance = Instantiate(missilePrefab, transform.position, Quaternion.identity);

        if (isHomingActive)
        {
            GameObject closestEnemy = FindClosestEnemyToRight();
            if (closestEnemy != null)
            {
                Debug.Log("Found closest enemy: " + closestEnemy.name);

                // Calculate the direction to the closest enemy
                Vector3 direction = (closestEnemy.transform.position - missileInstance.transform.position).normalized;

                // Rotate the missile to face the closest enemy
                missileInstance.transform.rotation = Quaternion.LookRotation(direction);

                // Modify the missile's speed to track the closest enemy
                missileInstance.GetComponent<Rigidbody>().velocity = direction * missileSpeed;
            }
            else
            {
                Debug.Log("No target found for homing missile");
                SetupMissile(missileInstance);
            }
        }
        else
        {
            SetupMissile(missileInstance); // Set up the missile's behavior when homing is not active
        }
    }


    private void SetupMissile(GameObject missile)
    {
        missile.layer = LayerMask.NameToLayer("Missile");
        Collider missileCollider = missile.GetComponent<Collider>();
        if (missileCollider != null)
        {
            missileCollider.isTrigger = false;
        }
        Rigidbody missileRigidbody = missile.GetComponent<Rigidbody>();
        if (missileRigidbody != null)
        {
            missileRigidbody.velocity = transform.forward * missileSpeed;
        }
    }

    private GameObject FindClosestEnemyToRight()
    {
        // Get all enemies with the "Enemy" tag
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Initialize variables to track the closest enemy and its distance
        GameObject closestEnemy = null;
        float minDistance = Mathf.Infinity;

        // Get the player's position
        Vector3 playerPosition = transform.position;

        // Iterate through all enemies
        foreach (GameObject enemy in enemies)
        {
            // Calculate the enemy's position
            Vector3 enemyPosition = enemy.transform.position;

            // Check if the enemy is to the right of the player
            if (enemyPosition.x > playerPosition.x)
            {
                // Calculate the distance between the player and the enemy
                float distance = Vector3.Distance(playerPosition, enemyPosition);

                // Check if this enemy is closer than the current closest enemy
                if (distance < minDistance)
                {
                    // Update the closest enemy and the minimum distance
                    closestEnemy = enemy;
                    minDistance = distance;
                }
            }
        }

        return closestEnemy;
    }
}
