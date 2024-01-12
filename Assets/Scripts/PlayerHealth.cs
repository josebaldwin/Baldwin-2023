using UnityEngine;
using System.Collections.Generic;

public class PlayerHealth : MonoBehaviour
{
    public int maxHits = 10; // Number of hits the player can resist
    private int currentHits;
    public Material glowMaterial; // Assign a glow material in the inspector
    private Dictionary<Renderer, Material[]> originalMaterials;
    public float glowDuration = 0.5f; // Duration of the glow effect
    private bool isGlowing = false; // To track if the glow effect is active

    void Start()
    {
        currentHits = 0;
        originalMaterials = new Dictionary<Renderer, Material[]>();
        StoreOriginalMaterials();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnemyMissile"))
        {
            Destroy(collision.gameObject); // Destroy the enemy missile

            currentHits++;
            if (currentHits >= maxHits)
            {
                DestroyPlayer();
            }
            else if (!isGlowing)
            {
                ActivateGlowEffect();
            }
        }
    }

    void StoreOriginalMaterials()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            if (renderer.gameObject.layer != LayerMask.NameToLayer("PlayerEngine"))
            {
                originalMaterials[renderer] = renderer.materials;
            }
        }
    }

    void ActivateGlowEffect()
    {
        isGlowing = true;
        foreach (var kvp in originalMaterials)
        {
            Material[] glowMaterials = new Material[kvp.Value.Length];
            for (int i = 0; i < glowMaterials.Length; i++)
            {
                glowMaterials[i] = glowMaterial;
            }
            kvp.Key.materials = glowMaterials;
        }

        Invoke("ResetGlowEffect", glowDuration);
    }

    void ResetGlowEffect()
    {
        foreach (var kvp in originalMaterials)
        {
            kvp.Key.materials = kvp.Value;
        }
        isGlowing = false;
    }

    void DestroyPlayer()
    {
        // Add any additional destruction logic here (like explosion effect)
        Destroy(gameObject);
    }
}
