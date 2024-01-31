using UnityEngine;
using System.Collections;

public class PowersUpManager : MonoBehaviour
{
    public GameObject shieldPrefab; // Assign the shield prefab in the inspector
    public float doubleFireRateDuration = 10f; // Duration for double fire rate
    public float overkillDuration = 10f; // Duration for overkill power-up

    private PlayerShooting[] playerShootings; // Array to hold multiple PlayerShooting instances
    private GameObject shieldInstance;

    private void Start()
    {
        playerShootings = GetComponentsInChildren<PlayerShooting>(true);

        if (playerShootings == null || playerShootings.Length == 0)
        {
            Debug.LogError("PlayerShooting components not found on the children of the GameObject.");
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
            float desiredShieldScale = 3.5f; // Desired scale factor for the shield
            shieldInstance.transform.localScale = new Vector3(desiredShieldScale / parentScale.x, desiredShieldScale / parentScale.y, desiredShieldScale / parentScale.z);
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
    }

    private void ActivateHomingMissile()
    {
        // Activate homing missiles for all player shootings
        foreach (var shooting in playerShootings)
        {
            shooting.EnableHoming(true);
        }
    }
}