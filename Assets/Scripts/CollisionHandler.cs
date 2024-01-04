using System.Collections;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public int maxResistance = 30;
    private int resistanceCounter = 0;
    private Rigidbody enemyRigidbody;
    public Material glowMaterial;
    private Renderer[] childRenderers;
    public float glowDuration = 0.5f;
    private ParticleControl particleControl;

    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody>();
        childRenderers = GetComponentsInChildren<Renderer>();

        // Add the ParticleControl script to the same GameObject if not present
        particleControl = GetComponent<ParticleControl>();
        if (particleControl == null)
        {
            particleControl = gameObject.AddComponent<ParticleControl>();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Missile"))
        {
            Destroy(collision.gameObject);

            ActivateGlowEffect();

            if (resistanceCounter < maxResistance)
            {
                resistanceCounter++;
                if (enemyRigidbody != null)
                {
                    enemyRigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
                }

                // Restart particle emission after 2 seconds
                particleControl.RestartParticleEmission(2f);
            }
            else
            {
                StartCoroutine(DestroyAfterDelay(2f));

                if (enemyRigidbody != null)
                {
                    enemyRigidbody.isKinematic = false;
                }
            }
        }
    }

    void ActivateGlowEffect()
    {
        if (childRenderers != null && glowMaterial != null)
        {
            foreach (Renderer childRenderer in childRenderers)
            {
                glowMaterial.EnableKeyword("_EMISSION");
                childRenderer.material = glowMaterial;
            }

            StartCoroutine(DisableGlowEffectAfterDelay(glowDuration));
        }
    }

    IEnumerator DisableGlowEffectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (Renderer childRenderer in childRenderers)
        {
            childRenderer.material.DisableKeyword("_EMISSION");
        }
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
