using UnityEngine;
using UnityEngine.Assertions;

public enum SoundEffect
{ 
    NOTE_0,
    NOTE_1,
    NOTE_2,
    NOTE_3,
    NOTE_4,
    NOTE_5,
    NOTE_6,
    NOTE_7,
    NOTE_8,
    NOTE_9,
    NOTE_10,
    NOTE_11,
    NOTE_12,
    CHIME,
}

public class AudioService : MonoBehaviour
{
    [Header("Views")]
    [NotNull] public AudioClip[] Notes;
    [NotNull] public AudioClip Chime;

    void Awake()
    {
        // Safeguard to ensure we update the code when loading new sounds.
        Assert.IsTrue(Notes.Length == 13);
    }

    AudioClip GetAudioClip(SoundEffect soundEffect)
    {
        return soundEffect switch
        {
            SoundEffect.NOTE_0 => Notes[0],
            SoundEffect.NOTE_1 => Notes[1],
            SoundEffect.NOTE_2 => Notes[2],
            SoundEffect.NOTE_3 => Notes[3],
            SoundEffect.NOTE_4 => Notes[4],
            SoundEffect.NOTE_5 => Notes[5],
            SoundEffect.NOTE_6 => Notes[6],
            SoundEffect.NOTE_7 => Notes[7],
            SoundEffect.NOTE_8 => Notes[8],
            SoundEffect.NOTE_9 => Notes[9],
            SoundEffect.NOTE_10 => Notes[10],
            SoundEffect.NOTE_11 => Notes[11],
            SoundEffect.NOTE_12 => Notes[12],
            SoundEffect.CHIME => Chime,
            _ => throw new System.Exception($"Unimplemented sound effect: {soundEffect}")
        };
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

    /// <summary>
    /// An alternative version of PlayOneShot in the case where you don't want to customize
    /// the fields on an AudioSource, and just want to play the default AudioClip for a sound effect.
    /// </summary>
    public void PlayOneShot(SoundEffect soundEffect)
    {
		PlayOneShot(GetAudioSource(soundEffect));

        // Unused hack to work around not having all the sound effects:
#if false
        var index = (int)soundEffect;
        if (5 <= index && index <= 7)
        {
            // Grab the AudioClip that is one octave down.
            index -= 4;
            var sfx = (SoundEffect)index;
            var audioSource = GetAudioSource(sfx);

            // Transpose it one octave up.
            audioSource.pitch = 3;

            // Then play the transposed AudioClip.
			PlayOneShot(audioSource);
		}
        else
        { 
            // Otherwise, play the AudioClip as normal.
			PlayOneShot(GetAudioSource(soundEffect));
		}
#endif
    }
}
