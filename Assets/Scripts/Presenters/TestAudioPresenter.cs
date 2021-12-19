using UnityEngine;

public class TestAudioPresenter : MonoBehaviour
{
    [Header("Services")]
    [NotNull] public AudioService AudioService;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AudioService.PlayOneShot(SoundEffect.NOTE_0);
		}

        if (Input.GetKeyDown(KeyCode.W))
        {
            AudioService.PlayOneShot(SoundEffect.NOTE_1);
		}

        if (Input.GetKeyDown(KeyCode.E))
        {
            AudioService.PlayOneShot(SoundEffect.NOTE_2);
		}

        if (Input.GetKeyDown(KeyCode.R))
        {
            AudioService.PlayOneShot(SoundEffect.NOTE_3);
		}

        if (Input.GetKeyDown(KeyCode.T))
        {
            AudioService.PlayOneShot(SoundEffect.NOTE_4);
		}

        if (Input.GetKeyDown(KeyCode.Y))
        {
            AudioService.PlayOneShot(SoundEffect.NOTE_5);
		}

        if (Input.GetKeyDown(KeyCode.U))
        {
            AudioService.PlayOneShot(SoundEffect.NOTE_6);
		}

        if (Input.GetKeyDown(KeyCode.I))
        {
            AudioService.PlayOneShot(SoundEffect.NOTE_7);
		}

        if (Input.GetKeyDown(KeyCode.A))
        {
            AudioService.PlayOneShot(SoundEffect.CHIME);
		}
    }
}
