using UnityEngine;
using System.Collections;

public class ShieldBehavior : MonoBehaviour
{
    [Header("Materials")]
    public Material regularMaterial; // Regular shield material
    public Material glowingMaterial; // Glowing shield material for when hit

    [Header("Glow Effect Settings")]
    public float glowDuration = 0.2f; // Duration of the glow effect

    [Header("Shield Health")]
    public float maxShieldHealth = 50f; // Maximum shield health
    private float shieldHealth; // Current shield health

    private Renderer shieldRenderer;
    private Coroutine glowCoroutine; // Store the glow coroutine
    private bool isHit = false; // Flag to track if the shield is hit

    void Start()
    {
        shieldRenderer = GetComponent<Renderer>();
        shieldRenderer.material = regularMaterial;
        shieldHealth = maxShieldHealth; // Initialize shield health to maximum
    }

    public void TakeDamage(float damageAmount)
    {
        if (!isHit)
        {
            shieldHealth -= damageAmount;
            Debug.Log("Shield Health: " + shieldHealth);

            if (shieldHealth <= 0f)
            {
                // If shield health reaches zero or below, destroy the shield
                Destroy(gameObject);
            }
            else
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
}
