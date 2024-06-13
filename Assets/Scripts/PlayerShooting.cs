using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject missilePrefab; // Assign the Missile Prefab in the Inspector
    public KeyCode shootKey = KeyCode.Space;
    public float shootingCooldown = 0.5f; // Adjust this to control the shooting rate
    public float missileSpeed = 10f; // Adjust this to control missile speed

    private float nextShootTime;
    private bool isDoubleFireRateActive = false;
    private bool isHomingActive = false;
    private bool isShooting = false;
    private float homingMissileAngle = 0f;

    [SerializeField]
    private float homingMissileAnglePositive = 90f; // Angle for the first additional missile
    [SerializeField]
    private float homingMissileAngleNegative = -90f; // Angle for the second additional missile

    private AudioManager audioManager;

    void Start()
    {
        audioManager = AudioManager.Instance;
        if (audioManager == null)
        {
            Debug.LogError("AudioManager instance not found.");
        }
    }

    void Update()
    {
        if (Input.GetKey(shootKey))
        {
            if (Time.time > nextShootTime)
            {
                ShootMissile();
                nextShootTime = Time.time + (isDoubleFireRateActive ? shootingCooldown / 2 : shootingCooldown);
            }
            if (!isShooting)
            {
                isShooting = true;
                audioManager?.PlayPlayerShootingSound();
            }
        }
        else
        {
            if (isShooting)
            {
                isShooting = false;
                audioManager?.StopPlayerShootingSound();
            }
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
        GameObject missileInstance = Instantiate(missilePrefab, transform.position, Quaternion.identity);
        SetupMissile(missileInstance);

        if (isHomingActive)
        {
            SpawnHomingMissiles(transform.forward);
        }
    }

    private void SpawnHomingMissiles(Vector3 initialDirection)
    {
        SpawnMissileWithAngle(homingMissileAnglePositive, initialDirection);
        SpawnMissileWithAngle(homingMissileAngleNegative, initialDirection);
    }

    private void SpawnMissileWithAngle(float angle, Vector3 direction)
    {
        GameObject missileInstance = Instantiate(missilePrefab, transform.position, Quaternion.identity);
        SetupMissile(missileInstance);
        missileInstance.GetComponent<Rigidbody>().velocity = Quaternion.Euler(0f, 0f, angle) * direction * missileSpeed;
        missileInstance.transform.position = new Vector3(missileInstance.transform.position.x, missileInstance.transform.position.y, 0f);
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
