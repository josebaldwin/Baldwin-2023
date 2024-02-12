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
    private bool isHomingActive = false;
    private float homingMissileAngle = 0f;

    [SerializeField]
    private float homingMissileAnglePositive = 90f; // Angle for the first additional missile
    [SerializeField]
    private float homingMissileAngleNegative = -90f; // Angle for the second additional missile

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

    public void EnableHoming(bool enable, float homingAngle = 0f)
    {
        isHomingActive = enable;
        homingMissileAngle = homingAngle; // Set the homing angle
    }

    private void ShootMissile()
    {
        // Original missile
        GameObject missileInstance = Instantiate(missilePrefab, transform.position, Quaternion.identity);
        SetupMissile(missileInstance);

        if (isHomingActive)
        {
            // Calculate the initial direction based on the player's forward direction
            Vector3 initialDirection = transform.forward;

            // First additional missile with positive Y angle
            GameObject missileInstancePositive = Instantiate(missilePrefab, transform.position, Quaternion.identity);
            SetupMissile(missileInstancePositive);

            // Modify the Y component of the velocity to add an angle globally
            missileInstancePositive.GetComponent<Rigidbody>().velocity = Quaternion.Euler(0f, 0f, homingMissileAnglePositive) * initialDirection * missileSpeed;

            // Keep Z coordinate at 0
            Vector3 newPositionPositive = missileInstancePositive.transform.position;
            newPositionPositive.z = 0f;
            missileInstancePositive.transform.position = newPositionPositive;

            // Second additional missile with negative Y angle
            GameObject missileInstanceNegative = Instantiate(missilePrefab, transform.position, Quaternion.identity);
            SetupMissile(missileInstanceNegative);

            // Modify the Y component of the velocity to add a negative angle globally
            missileInstanceNegative.GetComponent<Rigidbody>().velocity = Quaternion.Euler(0f, 0f, homingMissileAngleNegative) * initialDirection * missileSpeed;

            // Keep Z coordinate at 0
            Vector3 newPositionNegative = missileInstanceNegative.transform.position;
            newPositionNegative.z = 0f;
            missileInstanceNegative.transform.position = newPositionNegative;
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
}
