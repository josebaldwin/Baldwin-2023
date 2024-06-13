using System.Collections;
using UnityEngine;


public class SeamlessLoop : MonoBehaviour
{
    public AudioClip loopClip;
    private AudioSource audioSource1;
    private AudioSource audioSource2;
    private bool isPlayingFirst = true;

    void Start()
    {
        audioSource1 = gameObject.AddComponent<AudioSource>();
        audioSource2 = gameObject.AddComponent<AudioSource>();

        audioSource1.clip = loopClip;
        audioSource2.clip = loopClip;

        audioSource1.loop = false;
        audioSource2.loop = false;

        audioSource1.Play();
        isPlayingFirst = true;

        StartCoroutine(PlayLoop());
    }

    private IEnumerator PlayLoop()
    {
        while (true)
        {
            if (isPlayingFirst)
            {
                yield return new WaitForSeconds(loopClip.length - 0.1f);
                audioSource2.Play();
                isPlayingFirst = false;
            }
            else
            {
                yield return new WaitForSeconds(loopClip.length - 0.1f);
                audioSource1.Play();
                isPlayingFirst = true;
            }
        }
    }
}
