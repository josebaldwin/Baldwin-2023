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

    private Coroutine doubleFireRateCoroutine;
    private Coroutine homingCoroutine;

    private void Start()
    {
        playerShootings = GetComponentsInChildren<PlayerShooting>(true);

        if (playerShootings == null || playerShootings.Length == 0)
        {
            Debug.LogError("PlayerShooting components not found on the children of the GameObject.");
            this.enabled = false;
            return;
        }

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
                ActivateDoubleFireRateCoroutine(doubleFireRateDuration);
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
        if (shieldInstance != null)
        {
            shieldHealth = 100f;
            UpdateShieldHealthBar();
        }
        else
        {
            shieldInstance = Instantiate(shieldPrefab, transform.position, Quaternion.identity, transform);
            Vector3 parentScale = transform.lossyScale;
            float desiredShieldScale = 3.8f;
            shieldInstance.transform.localScale = new Vector3(desiredShieldScale / parentScale.x, desiredShieldScale / parentScale.y, desiredShieldScale / parentScale.z);
            shieldHealth = 100f;
            UpdateShieldHealthBar();
        }

        if (ShipBarsManager.Instance != null)
        {
            ShipBarsManager.Instance.SetShieldInstance(shieldInstance);
            ShipBarsManager.Instance.PickupShield();
        }
        else
        {
            Debug.LogError("ShipBarsManager instance not found.");
        }
    }

    private void ActivateDoubleFireRateCoroutine(float duration)
    {
        if (doubleFireRateCoroutine != null)
        {
            StopCoroutine(doubleFireRateCoroutine);
        }
        doubleFireRateCoroutine = StartCoroutine(DoubleFireRate(duration));
    }

    private IEnumerator DoubleFireRate(float duration)
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
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            EnemyExplosion explosion = enemy.GetComponent<EnemyExplosion>();
            if (explosion != null)
            {
                explosion.OnKillMethod();
            }
        }

        GameObject[] missiles = GameObject.FindGameObjectsWithTag("EnemyMissile");
        foreach (var missile in missiles)
        {
            Destroy(missile);
        }
    }

    private void ActivateHomingMissile()
    {
        if (homingCoroutine != null)
        {
            StopCoroutine(homingCoroutine);
        }
        homingCoroutine = StartCoroutine(ActivateHomingCoroutine(homingDuration));
    }

    private IEnumerator ActivateHomingCoroutine(float duration)
    {
        foreach (var shooting in playerShootings)
        {
            shooting.EnableHoming(true);
        }
        yield return new WaitForSeconds(duration);
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
