using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;         // Accès global (singleton)

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;      // Pour la musique de fond
    [SerializeField] private AudioSource ambienceSource;   // Pour le bruit permanent
    [SerializeField] private AudioSource sfxSource;        // Pour les sons courts

    private void Awake()
    {
        // Singleton basique
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    

    /// <summary>
    /// Lance la musique de fond (boucle automatiquement si loop = true).
    /// </summary>
    public void PlayBackgroundMusic(AudioClip clip, float volume = 1f, bool loop = true)
    {
        musicSource.clip = clip;
        musicSource.volume = volume;
        musicSource.loop = loop;
        musicSource.Play();
    }

    /// <summary>
    /// Lance un bruit de fond permanent (ambience).
    /// </summary>
    public void PlayAmbience(AudioClip clip, float volume = 0.5f, bool loop = true)
    {
        ambienceSource.clip = clip;
        ambienceSource.volume = volume;
        ambienceSource.loop = loop;
        ambienceSource.Play();
    }

    /// <summary>
    /// Joue un son court (SFX) une seule fois.
    /// </summary>
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        sfxSource.PlayOneShot(clip, volume);
    }
}

