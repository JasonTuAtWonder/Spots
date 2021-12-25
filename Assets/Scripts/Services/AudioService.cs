using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// AudioService holds all of the game's AudioClips, and provides helper methods for playing them.
/// </summary>
public class AudioService : MonoBehaviour
{
    /// <summary>
    /// Sounds that are played upon connecting dots.
    /// For example, NOTE_0 is played upon connecting 1 dot, NOTE_1 is played upon connecting 2 dots, etc.
    /// </summary>
    [Header("AudioClips")]
    [NotNull] public AudioClip[] Notes;

    /// <summary>
    /// Chime sound that is played when a square is detected.
    ///
    /// The chime that is played when a *rectangle* is detected sounds different,
    /// but as of this writing I don't have that sound effect available.
    /// </summary>
    [NotNull] public AudioClip Chime;

    /// <summary>
    /// Silence clip used to tell Unity/WebGL to resume AudioContext.
    /// </summary>
    [NotNull] public AudioClip Silence;

    void Awake()
    {
        // Sanity check to ensure I loaded all the AudioClips I have available.
        Assert.IsTrue(Notes.Length == 13);
    }

    void Start()
    {
        FixWebGLSound();
    }

    void FixWebGLSound()
    {
        // Play a dummy, zero-volume sound at app start to avoid blips in-game.
        PlayOneShot(Silence);
    }

    /// <summary>
    /// Fetch a newly created AudioSource for a specified SoundEffect.
    ///
    /// This allows you to customize the AudioSource settings before playing with AudioPresenter.PlayOneShot().
    /// </summary>
    public AudioSource GetAudioSource(AudioClip audioClip)
    {
        var gameObject = new GameObject();
        var audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        return audioSource;
    }

    /// <summary>
    /// Play an AudioSource.
    ///
    /// This method will destroy the AudioSource's corresponding GameObject
    /// once the AudioSource is done playing.
    /// </summary>
    public void PlayOneShot(AudioSource audioSource)
    {
        audioSource.Play();
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }

    /// <summary>
    /// An alternative version of PlayOneShot.
    ///
    /// Used in the case where you just want to play the AudioSource's AudioClip without customization.
    /// </summary>
    public void PlayOneShot(AudioClip audioClip)
    {
        PlayOneShot(GetAudioSource(audioClip));
    }
}
