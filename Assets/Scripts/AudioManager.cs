using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioClip playerShootingSound; // Looping sound for player shooting
    public AudioClip playerDoubleFireRateShootingSound; // Looping sound for double fire rate
    public AudioClip enemyShootingSound;  // One-shot sound for enemy shooting
    private AudioSource playerAudioSource;
    private List<AudioSource> enemyAudioSources = new List<AudioSource>();
    private int maxEnemyAudioSources = 10; // Max number of enemy shooting sounds playing at the same time

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAudioSources()
    {
        playerAudioSource = gameObject.AddComponent<AudioSource>();
        playerAudioSource.loop = true;

        for (int i = 0; i < maxEnemyAudioSources; i++)
        {
            AudioSource enemyAudioSource = gameObject.AddComponent<AudioSource>();
            enemyAudioSources.Add(enemyAudioSource);
        }
    }

    public void PlayPlayerShootingSound(bool isDoubleFireRate)
    {
        AudioClip clipToPlay = isDoubleFireRate ? playerDoubleFireRateShootingSound : playerShootingSound;

        if (playerAudioSource.clip != clipToPlay)
        {
            playerAudioSource.Stop();
            playerAudioSource.clip = clipToPlay;
            playerAudioSource.Play();
            Debug.Log("Player shooting sound started.");
        }

        if (!playerAudioSource.isPlaying)
        {
            playerAudioSource.Play();
            Debug.Log("Player shooting sound resumed.");
        }
    }

    public void StopPlayerShootingSound()
    {
        if (playerAudioSource.isPlaying)
        {
            playerAudioSource.Stop();
            Debug.Log("Player shooting sound stopped.");
        }
    }

    public void PlayEnemyShootingSound()
    {
        foreach (var audioSource in enemyAudioSources)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(enemyShootingSound);
                Debug.Log("Enemy shooting sound played.");
                return;
            }
        }
        Debug.Log("All enemy audio sources are busy.");
    }

    public void StopAllEnemyShootingSounds()
    {
        foreach (var audioSource in enemyAudioSources)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
        Debug.Log("All enemy shooting sounds stopped.");
    }
}
