using UnityEngine;
using System.Collections;

public class PowersUpManager : MonoBehaviour
{
    public GameObject shieldPrefab; // Assign the shield prefab in the inspector
    public float doubleFireRateDuration = 15f; // Duration for double fire rate
    public float homingDuration = 15f; // Duration for homing power-up (changed to 15 seconds)

    private PlayerShooting[] playerShootings; // Array to hold multiple PlayerShooting instances
    private GameObject shieldInstance; // Reference to the spawned shield instance
    private ProgressBar shieldHealthBar; // Reference to the shield health bar
    private float shieldHealth = 100f; // Initial shield health

    private void Start()
    {
        playerShootings = GetComponentsInChildren<PlayerShooting>(true);

        if (playerShootings == null || playerShootings.Length == 0)
        {
            Debug.LogError("PlayerShooting components not found on the children of the GameObject.");
            this.enabled = false;
            return;
        }

        // Find and assign the shield health bar
        shieldHealthBar = GameObject.FindWithTag("ShieldHealthBar").GetComponent<ProgressBar>();
        if (shieldHealthBar == null)
        {
            Debug.LogError("Shield health bar not found.");
            this.enabled = false;
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "PowerUpHealth":
                ActivateShield();
                break;
            case "PowerUpDouble":
                StartCoroutine(ActivateDoubleFireRateCoroutine(doubleFireRateDuration));
                break;
            case "PowerUpOverkill":
                ActivateOverkill();
                break;
            case "PowerUpHoming":
                ActivateHomingMissile();
                break;
        }
        Destroy(other.gameObject); // Destroy the power-up immediately
    }

    private void ActivateShield()
    {
        if (shieldInstance == null)
        {
            shieldInstance = Instantiate(shieldPrefab, transform.position, Quaternion.identity, transform);

            // Adjust the scale of the shield relative to the parent's scale
            Vector3 parentScale = transform.lossyScale; // Gets the absolute scale of the parent
            float desiredShieldScale = 3.8f; // Desired scale factor for the shield
            shieldInstance.transform.localScale = new Vector3(desiredShieldScale / parentScale.x, desiredShieldScale / parentScale.y, desiredShieldScale / parentScale.z);

            if (ShipBarsManager.Instance != null)
            {
                ShipBarsManager.Instance.SetShieldInstance(shieldInstance);
                ShipBarsManager.Instance.PickupShield(); // Now it should correctly set the shield active and initialize the bar to 100%
            }
            else
            {
                Debug.LogError("ShipBarsManager instance not found.");
            }
        }
    }

    private IEnumerator ActivateDoubleFireRateCoroutine(float duration)
    {
        foreach (var shooting in playerShootings)
        {
            shooting.DoubleFireRate(true);
        }
        yield return new WaitForSeconds(duration);
        foreach (var shooting in playerShootings)
        {
            shooting.DoubleFireRate(false);
        }
    }

    private void ActivateOverkill()
    {
        // Find all enemies with the tag "Enemy"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Activate the OnKillMethod for each enemy
        foreach (var enemy in enemies)
        {
            EnemyExplosion explosion = enemy.GetComponent<EnemyExplosion>();
            if (explosion != null)
            {
                explosion.OnKillMethod();
            }
        }

        // Find all enemy missiles and destroy them
        GameObject[] missiles = GameObject.FindGameObjectsWithTag("EnemyMissile");
        foreach (var missile in missiles)
        {
            Destroy(missile);
        }
    }

    private void ActivateHomingMissile()
    {
        // Activate homing missiles for all player shootings
        foreach (var shooting in playerShootings)
        {
            shooting.EnableHoming(true, 0f); // Enable homing with a default angle of 0 degrees
        }

        // Start a coroutine to deactivate homing after the specified duration
        StartCoroutine(DeactivateHomingCoroutine(homingDuration));
    }

    private IEnumerator DeactivateHomingCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);

        // Deactivate homing after the specified duration
        foreach (var shooting in playerShootings)
        {
            shooting.EnableHoming(false);
        }
    }

    private void UpdateShieldHealthBar()
    {
        if (shieldHealthBar != null)
        {
            shieldHealthBar.BarValue = shieldHealth;
        }
    }

    public void TakeShieldDamage(float damage)
    {
        shieldHealth -= damage;
        if (shieldHealth <= 0f)
        {
            Destroy(shieldInstance);
            shieldInstance = null;
        }
        UpdateShieldHealthBar();
    }
}