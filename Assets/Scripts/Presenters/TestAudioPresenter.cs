using UnityEngine;

public class TestAudioPresenter : MonoBehaviour
{
    [Header("Services")]
    [NotNull] public AudioService AudioService;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            var audioSource = AudioService.GetAudioSource(SoundEffect.NOTE_0);
            AudioService.PlayOneShot(audioSource);
		}

        if (Input.GetKeyDown(KeyCode.W))
        {
            var audioSource = AudioService.GetAudioSource(SoundEffect.NOTE_1);
            AudioService.PlayOneShot(audioSource);
		}

        if (Input.GetKeyDown(KeyCode.E))
        {
            var audioSource = AudioService.GetAudioSource(SoundEffect.NOTE_2);
            AudioService.PlayOneShot(audioSource);
		}

        if (Input.GetKeyDown(KeyCode.R))
        {
            var audioSource = AudioService.GetAudioSource(SoundEffect.NOTE_3);
            AudioService.PlayOneShot(audioSource);
		}

        if (Input.GetKeyDown(KeyCode.T))
        {
            var audioSource = AudioService.GetAudioSource(SoundEffect.NOTE_4);
            AudioService.PlayOneShot(audioSource);
		}

        if (Input.GetKeyDown(KeyCode.A))
        {
            var audioSource = AudioService.GetAudioSource(SoundEffect.CHIME);
            AudioService.PlayOneShot(audioSource);
		}
    }
}
