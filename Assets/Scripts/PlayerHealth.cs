using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public int maxHits = 10; // Maximum number of hits before the player is destroyed
    private int currentHits;
    public Material glowMaterial; // Assign a glow material in the inspector
    public GameObject explosionParticlePrefab; // Assign the explosion particle prefab in the inspector
    private Renderer[] playerRenderers;
    public float glowDuration = 0.5f; // Duration of the glow effect
    private Material[] originalMaterials; // To store original materials of the player

    // Reference to the ShipBarsManager script
    private ShipBarsManager shipBarsManager;

    // Flag to indicate if the shield is active
    private bool isShieldActive = false;

    void Start()
    {
        currentHits = 0;
        playerRenderers = GetComponentsInChildren<Renderer>();
        StoreOriginalMaterials();

        // Find the ShipBarsManager script in the scene
        shipBarsManager = FindObjectOfType<ShipBarsManager>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the shield is active and ignore damage if it is
        if (isShieldActive)
        {
            Debug.Log("Shield is active, no damage taken.");
            return;
        }

        if (collision.gameObject.CompareTag("EnemyMissile") || collision.gameObject.CompareTag("Enemy"))
        {
            currentHits++;
            ActivateGlowEffect();
            // Calculate ship health percentage and trigger event
            float shipHealthPercentage = 1f - (float)currentHits / maxHits;

            // Update the ship health bar in the ShipBarsManager
            shipBarsManager.UpdateHealthBar(shipHealthPercentage);

            if (currentHits >= maxHits)
            {
                OnKill();
            }
        }
    }

    void StoreOriginalMaterials()
    {
        foreach (Renderer renderer in playerRenderers)
        {
            if (renderer.gameObject.layer != LayerMask.NameToLayer("PlayerEngine"))
            {
                originalMaterials = renderer.materials;
            }
        }
    }

    void ActivateGlowEffect()
    {
        foreach (Renderer renderer in playerRenderers)
        {
            if (renderer.gameObject.layer != LayerMask.NameToLayer("PlayerEngine"))
            {
                Material[] glowMaterials = new Material[renderer.materials.Length];
                for (int i = 0; i < glowMaterials.Length; i++)
                {
                    glowMaterials[i] = glowMaterial;
                }
                renderer.materials = glowMaterials;
            }
        }

        Invoke(nameof(ResetGlowEffect), glowDuration);
    }

    void ResetGlowEffect()
    {
        foreach (Renderer renderer in playerRenderers)
        {
            if (renderer.gameObject.layer != LayerMask.NameToLayer("PlayerEngine"))
            {
                renderer.materials = originalMaterials;
            }
        }
    }

    void OnKill()
    {
        Debug.Log("Player killed. Instantiating explosion and disabling player.");
        Instantiate(explosionParticlePrefab, transform.position, Quaternion.identity);

        // Play player explosion sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayPlayerExplosionSound();
            AudioManager.Instance.StopPlayerShootingSound();
        }

        // Instead of destroying, just disable the player GameObject.
        gameObject.SetActive(false);

        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.GameOver();
        }
        else
        {
            Debug.LogError("GameManager not found in the scene.");
        }
    }

    // Method to activate the shield
    public void ActivateShield()
    {
        isShieldActive = true;
    }

    // Method to deactivate the shield
    public void DeactivateShield()
    {
        isShieldActive = false;
    }
}
