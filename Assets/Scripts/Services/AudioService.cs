using UnityEngine;
using UnityEngine.Assertions;

public enum SoundEffect
{ 
    NOTE_0,
    NOTE_1,
    NOTE_2,
    NOTE_3,
    NOTE_4,
    CHIME,
}

public class AudioService : MonoBehaviour
{
    [Header("Views")]
    [NotNull] public AudioClip[] Notes;
    [NotNull] public AudioClip Chime;

    void Awake()
    {
        Assert.IsTrue(Notes.Length == 5);
    }

    AudioClip GetAudioClip(SoundEffect soundEffect)
    {
		#pragma warning disable CS8524
        return soundEffect switch
        {
            SoundEffect.NOTE_0 => Notes[0],
            SoundEffect.NOTE_1 => Notes[1],
            SoundEffect.NOTE_2 => Notes[2],
            SoundEffect.NOTE_3 => Notes[3],
            SoundEffect.NOTE_4 => Notes[4],
            SoundEffect.CHIME => Chime,
        };
		#pragma warning restore CS8524
    }

    /// <summary>
    /// Fetch a newly created AudioSource for a specified SoundEffect.
    ///
    /// This allows you to customize the AudioSource settings before playing with
    /// AudioPresenter.PlayOneShot().
    /// </summary>
    public AudioSource GetAudioSource(SoundEffect soundEffect)
    {
        var gameObject = new GameObject();
        var audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = GetAudioClip(soundEffect);
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
}
