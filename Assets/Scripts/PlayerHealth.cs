using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHits = 10; // Maximum number of hits before the player is destroyed
    private int currentHits;
    public Material glowMaterial; // Assign a glow material in the inspector
    public GameObject explosionParticlePrefab; // Assign the explosion particle prefab in the inspector
    private Renderer[] playerRenderers;
    public float glowDuration = 0.5f; // Duration of the glow effect
    private Material[] originalMaterials; // To store original materials of the player

    void Start()
    {
        currentHits = 0;
        playerRenderers = GetComponentsInChildren<Renderer>();
        StoreOriginalMaterials();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnemyMissile") || collision.gameObject.CompareTag("Enemy"))
        {
            currentHits++;
            ActivateGlowEffect();

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
        // Play the explosion particle effect
        if (explosionParticlePrefab != null)
        {
            Instantiate(explosionParticlePrefab, transform.position, Quaternion.identity);
        }

        // Destroy the player GameObject
        Destroy(gameObject);
    }
}
