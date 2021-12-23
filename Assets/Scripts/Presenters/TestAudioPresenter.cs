using UnityEngine;

public class TestAudioPresenter : MonoBehaviour
{
    [Header("Services")]
    [NotNull] public AudioService AudioService;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            AudioService.PlayOneShot(SoundEffect.NOTE_0);

        if (Input.GetKeyDown(KeyCode.W))
            AudioService.PlayOneShot(SoundEffect.NOTE_1);

        if (Input.GetKeyDown(KeyCode.S))
            AudioService.PlayOneShot(SoundEffect.NOTE_2);

        if (Input.GetKeyDown(KeyCode.E))
            AudioService.PlayOneShot(SoundEffect.NOTE_3);

        if (Input.GetKeyDown(KeyCode.D))
            AudioService.PlayOneShot(SoundEffect.NOTE_4);

        if (Input.GetKeyDown(KeyCode.R))
            AudioService.PlayOneShot(SoundEffect.NOTE_5);

        if (Input.GetKeyDown(KeyCode.F))
            AudioService.PlayOneShot(SoundEffect.NOTE_6);

        if (Input.GetKeyDown(KeyCode.T))
            AudioService.PlayOneShot(SoundEffect.NOTE_7);

        if (Input.GetKeyDown(KeyCode.G))
            AudioService.PlayOneShot(SoundEffect.NOTE_8);

        if (Input.GetKeyDown(KeyCode.Y))
            AudioService.PlayOneShot(SoundEffect.NOTE_9);

        if (Input.GetKeyDown(KeyCode.H))
            AudioService.PlayOneShot(SoundEffect.NOTE_10);

        if (Input.GetKeyDown(KeyCode.U))
            AudioService.PlayOneShot(SoundEffect.NOTE_11);

        if (Input.GetKeyDown(KeyCode.J))
            AudioService.PlayOneShot(SoundEffect.NOTE_12);

        if (Input.GetKeyDown(KeyCode.Z))
            AudioService.PlayOneShot(SoundEffect.CHIME);
    }
}
