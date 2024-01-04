using System.Collections;
using UnityEngine;

public class ParticleControl : MonoBehaviour
{
    private ParticleSystem particleSystem;

    void Start()
    {
        // Get the ParticleSystem component
        particleSystem = GetComponent<ParticleSystem>();
    }

    public void RestartParticleEmission(float delay)
    {
        StartCoroutine(RestartParticleEmissionCoroutine(delay));
    }

    IEnumerator RestartParticleEmissionCoroutine(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Restart particle emission
        if (particleSystem != null)
        {
            particleSystem.Play();
        }
    }
}
