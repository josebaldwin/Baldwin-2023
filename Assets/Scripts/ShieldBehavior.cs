using UnityEngine;
using System;
using System.Collections;

public class ShieldBehavior : MonoBehaviour
{
    // Define an event that will be triggered whenever the shield's health changes
    public event Action<float> ShieldHealthChanged;

    [Header("Materials")]
    public Material regularMaterial; // Regular shield material
    public Material glowingMaterial; // Glowing shield material for when hit

    [Header("Glow Effect Settings")]
    public float glowDuration = 0.2f; // Duration of the glow effect

    [Header("Shield Health")]
    public float maxShieldHealth = 50f; // Maximum shield health
    public float shieldHealth; // Current shield health (made public for visibility)

    private Renderer shieldRenderer;
    private Coroutine glowCoroutine; // Store the glow coroutine
    private bool isHit = false; // Flag to track if the shield is hit

    private PlayerHealth playerHealth; // Reference to the PlayerHealth script

    void Start()
    {
        shieldRenderer = GetComponent<Renderer>();
        shieldRenderer.material = regularMaterial;
        shieldHealth = maxShieldHealth; // Initialize shield health to maximum

        // Find the PlayerHealth script in the scene
        playerHealth = FindObjectOfType<PlayerHealth>();

        // Notify the PlayerHealth script that the shield is active
        if (playerHealth != null)
        {
            playerHealth.ActivateShield();
        }
    }

    // Method to handle when the shield is picked up
    public void PickupShield()
    {
        shieldHealth = maxShieldHealth; // Set to maximum when picked up
        ShieldHealthChanged?.Invoke(1f); // Ensure it's invoked with 100% health
    }

    public void TakeDamage(float damageAmount)
    {
        if (!isHit)
        {
            shieldHealth -= damageAmount;
            Debug.Log("Shield Health after damage: " + shieldHealth);

            if (shieldHealth <= 0f)
            {
                DestroyShield();
            }

            // Trigger the event regardless of the hit flag.
            ShieldHealthChanged?.Invoke(shieldHealth / maxShieldHealth);
            Debug.Log("Shield health changed triggered with: " + (shieldHealth / maxShieldHealth));

            if (shieldHealth > 0f)
            {
                // If the shield is not already hit, start the glow effect
                isHit = true;

                if (glowCoroutine != null)
                {
                    // If a previous glow is still active, stop it
                    StopCoroutine(glowCoroutine);
                }

                // Start the glow effect
                glowCoroutine = StartCoroutine(TempChangeMaterial());
            }
        }
    }

    private IEnumerator TempChangeMaterial()
    {
        shieldRenderer.material = glowingMaterial;

        // Wait for the glow duration
        yield return new WaitForSeconds(glowDuration);

        // Revert back to the original material
        shieldRenderer.material = regularMaterial;

        isHit = false; // Reset the hit flag
        glowCoroutine = null; // Reset the coroutine reference
    }

    private void DestroyShield()
    {
        Debug.Log("Shield destroyed.");
        Destroy(gameObject);

        // Notify the PlayerHealth script that the shield is deactivated
        if (playerHealth != null)
        {
            playerHealth.DeactivateShield();
        }
    }
}
