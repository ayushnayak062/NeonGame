using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Game SFX")]
    public AudioClip flipClip;
    public AudioClip matchClip;
    public AudioClip mismatchClip;
    public AudioClip gameOverClip;

    [Header("UI SFX")]
    public AudioClip saveClip;
    public AudioClip loadClip;
    public AudioClip newGameClip;

    [Header("Difficulty SFX")]
    public AudioClip easyClip;
    public AudioClip mediumClip;
    public AudioClip hardClip;
    public AudioClip insaneClip;

    [Header("Settings")]
    [Range(0f, 1f)]
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

    // Game events
    public void PlayFlip() => PlaySFX(flipClip);
    public void PlayMatch() => PlaySFX(matchClip);
    public void PlayMismatch() => PlaySFX(mismatchClip);
    public void PlayGameOver() => PlaySFX(gameOverClip);

    // UI events
    public void PlaySave() => PlaySFX(saveClip);
    public void PlayLoad() => PlaySFX(loadClip);
    public void PlayNewGame() => PlaySFX(newGameClip);

    // Difficulty events
    public void PlayDifficulty(int difficultyIndex)
    {
        switch (difficultyIndex)
        {
            case 0: PlaySFX(easyClip); break;
            case 1: PlaySFX(mediumClip); break;
            case 2: PlaySFX(hardClip); break;
            case 3: PlaySFX(insaneClip); break;
        }
    }

    private void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip, sfxVolume);
    }
}
