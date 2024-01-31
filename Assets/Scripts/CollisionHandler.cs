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

    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody>();
        childRenderers = GetComponentsInChildren<Renderer>();
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
            }
            else
            {
                EnemyBehavior enemyBehavior = GetComponent<EnemyBehavior>();
                if (enemyBehavior != null)
                {
                    enemyBehavior.DepleteResistance(); // Stop movement and rotation correction
                }

                EnemyExplosion explosionScript = GetComponent<EnemyExplosion>();
                if (explosionScript != null)
                {
                    explosionScript.StartDestructionSequence(); // Start delayed destruction
                }
                else
                {
                    Debug.LogError("EnemyExplosion script not found on " + gameObject.name);
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
                if (!childRenderer.CompareTag("JetEngine"))
                {
                    glowMaterial.EnableKeyword("_EMISSION");
                    childRenderer.material = glowMaterial;
                }
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


}
