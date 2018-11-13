using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class MusicController : MonoBehaviour
{
    [SerializeField] AudioClip titleTheme;
    [SerializeField] AudioClip characterSelectMusic;
    [SerializeField] AudioClip[] combatMusic;
    [SerializeField] AudioClip victoryMusic;
    [SerializeField] float maxVolume = 1;
    // Used for both fade in and fade out of music
    float fadeStep;

    AudioSource audioSource;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
    }

    public void SetFade(float fadeTime)
    {
        fadeStep = fadeTime;
    }

    public void PlayTitleMusic()
    {
        audioSource.clip = titleTheme;
        audioSource.Play();
    }

    public void PlayCombatMusic()
    {
        audioSource.clip = combatMusic[Random.Range(0, combatMusic.Length)];
        StartCoroutine(VolumeFadeIn());
        audioSource.Play();
    }

    // Used to interrupt combat music when a match ends. Called externally
    public void PlayVictoryMusic()
    {
        audioSource.clip = victoryMusic;
        StartCoroutine(VolumeFadeIn());
        audioSource.Play();
    }

    public void PlayCharacterSelectMusic()
    {
        audioSource.clip = characterSelectMusic;
        StartCoroutine(VolumeFadeIn());
        audioSource.Play();
    }
    // Used to call music fade out externally from GameManager
    public void MusicFadeOut()
    {
        StartCoroutine(VolumeFadeOut());
    }

    // Fades volume out from set volume to 0
    IEnumerator VolumeFadeOut()
    {
        while (audioSource.volume > 0)
        {
            audioSource.volume -= fadeStep;
            yield return new WaitForSeconds(fadeStep);
        }
    }

    // Fades volume in from 0 to max volume
    IEnumerator VolumeFadeIn()
    {
        audioSource.volume = 0;
        while (audioSource.volume < maxVolume)
        {
            audioSource.volume += fadeStep;
            yield return new WaitForSeconds(fadeStep);
        }
    }
}