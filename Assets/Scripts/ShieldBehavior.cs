using UnityEngine;
using System.Collections;

public class ShieldBehavior : MonoBehaviour
{
    [Header("Materials")]
    public Material regularMaterial; // Regular shield material
    public Material glowingMaterial; // Glowing shield material for when hit

    [Header("Glow Effect Settings")]
    public float glowDuration = 0.2f; // Duration of the glow effect

    private Renderer shieldRenderer;

    void Start()
    {
        shieldRenderer = GetComponent<Renderer>();
        shieldRenderer.material = regularMaterial;
    }

    public void HitByMissile()
    {
        StartCoroutine(TempChangeMaterial());
    }

    private IEnumerator TempChangeMaterial()
    {
        shieldRenderer.material = glowingMaterial;
        yield return new WaitForSeconds(glowDuration); // Use the customizable duration
        shieldRenderer.material = regularMaterial;
    }
}
