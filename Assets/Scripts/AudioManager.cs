using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource sfxSource;

    public AudioClip backgroundMusic;
    public AudioClip pickupSound;
    public AudioClip deliverySound;

    public static AudioManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayPickup()
    {
        sfxSource.PlayOneShot(pickupSound);
    }

    public void PlayDelivery()
    {
        sfxSource.PlayOneShot(deliverySound);
    }
}