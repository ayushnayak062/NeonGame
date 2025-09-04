using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Sound Effects")]
    public AudioClip flipClip;
    public AudioClip matchClip;
    public AudioClip mismatchClip;
    public AudioClip gameOverClip;

    [Header("Settings")]
    public float sfxVolume = 1f;

    private AudioSource sfxSource;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
        sfxSource.volume = sfxVolume;
    }

    public void PlayFlip() => PlaySFX(flipClip);
    public void PlayMatch() => PlaySFX(matchClip);
    public void PlayMismatch() => PlaySFX(mismatchClip);
    public void PlayGameOver() => PlaySFX(gameOverClip);

    private void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip, sfxVolume);
    }
}
