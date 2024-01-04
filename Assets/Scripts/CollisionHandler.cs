using System.Collections;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public int maxResistance = 30; // Set the maximum resistance value
    private int resistanceCounter = 0;
    private Rigidbody enemyRigidbody; // Reference to the Rigidbody component
    public Material glowMaterial; // Assign the glow material in the Inspector
    private Renderer[] childRenderers;
    public float glowDuration = 0.5f; // Adjust the glow duration

    void Start()
    {
        // Get the reference to the Rigidbody component
        enemyRigidbody = GetComponent<Rigidbody>();

        // Get all child renderers
        childRenderers = GetComponentsInChildren<Renderer>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collision involves a Missile and hasn't been hit yet
        if (collision.gameObject.CompareTag("Missile"))
        {
            // Destroy the missile immediately upon impact
            Destroy(collision.gameObject);

            // Activate the glow effect immediately upon missile impact
            ActivateGlowEffect();

            if (resistanceCounter < maxResistance)
            {
                // Enemy is resistant, do not get affected
                resistanceCounter++;

                // Freeze the enemy ship in the Z-axis
                if (enemyRigidbody != null)
                {
                    enemyRigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
                }
            }
            else
            {
                // Start a coroutine to destroy the object after a delay (e.g., 2 seconds)
                StartCoroutine(DestroyAfterDelay(2f)); // Adjust the delay as needed

                // Set the enemy Rigidbody to be affected by physics forces
                if (enemyRigidbody != null)
                {
                    enemyRigidbody.isKinematic = false;
                }
            }
        }
    }

    void ActivateGlowEffect()
    {
        // Check if the enemy has child renderers and a glow material
        if (childRenderers != null && glowMaterial != null)
        {
            // Enable the glow effect on all child renderers
            foreach (Renderer childRenderer in childRenderers)
            {
                // Enable emission on the glow material for each child renderer
                glowMaterial.EnableKeyword("_EMISSION");
                childRenderer.material = glowMaterial;
            }

            // Start the coroutine to disable the glow effect after a specified duration
            StartCoroutine(DisableGlowEffectAfterDelay(glowDuration));
        }
    }

    IEnumerator DisableGlowEffectAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Disable the glow effect on all child renderers
        foreach (Renderer childRenderer in childRenderers)
        {
            // Disable emission on the material for each child renderer
            childRenderer.material.DisableKeyword("_EMISSION");
        }
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Destroy the object this script is attached to (enemy)
        Destroy(gameObject);
    }
}
