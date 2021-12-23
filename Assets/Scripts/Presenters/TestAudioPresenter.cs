using UnityEngine;

/// <summary>
/// TestAudioPresenter is used for testing sound effects.
///
/// It does nothing in the final build.
/// </summary>
public class TestAudioPresenter : MonoBehaviour
{
    [Header("Services")]
    [NotNull] public AudioService AudioService;

#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            AudioService.PlayOneShot(AudioService.Notes[0]);

        if (Input.GetKeyDown(KeyCode.W))
            AudioService.PlayOneShot(AudioService.Notes[1]);

        if (Input.GetKeyDown(KeyCode.S))
            AudioService.PlayOneShot(AudioService.Notes[2]);

        if (Input.GetKeyDown(KeyCode.E))
            AudioService.PlayOneShot(AudioService.Notes[3]);

        if (Input.GetKeyDown(KeyCode.D))
            AudioService.PlayOneShot(AudioService.Notes[4]);

        if (Input.GetKeyDown(KeyCode.R))
            AudioService.PlayOneShot(AudioService.Notes[5]);

        if (Input.GetKeyDown(KeyCode.F))
            AudioService.PlayOneShot(AudioService.Notes[6]);

        if (Input.GetKeyDown(KeyCode.T))
            AudioService.PlayOneShot(AudioService.Notes[7]);

        if (Input.GetKeyDown(KeyCode.G))
            AudioService.PlayOneShot(AudioService.Notes[8]);

        if (Input.GetKeyDown(KeyCode.Y))
            AudioService.PlayOneShot(AudioService.Notes[9]);

        if (Input.GetKeyDown(KeyCode.H))
            AudioService.PlayOneShot(AudioService.Notes[10]);

        if (Input.GetKeyDown(KeyCode.U))
            AudioService.PlayOneShot(AudioService.Notes[11]);

        if (Input.GetKeyDown(KeyCode.J))
            AudioService.PlayOneShot(AudioService.Notes[12]);

        if (Input.GetKeyDown(KeyCode.Z))
            AudioService.PlayOneShot(AudioService.Chime);
    }
#endif
}
