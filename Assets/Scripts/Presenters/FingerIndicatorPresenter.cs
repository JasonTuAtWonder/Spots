using UnityEngine;

/// <summary>
/// FingerIndicatorPresenter draws a semi-transparent circle
/// where the player clicks, so that there's some visual feedback
/// when recording videos of the game.
/// </summary>
public class FingerIndicatorPresenter : MonoBehaviour
{
    [Header("Views")]
    [NotNull] public GameObject FingerIndicator;
    [NotNull] public Camera Camera;

    void Update()
    {
        // Update visibility.
        var isMouseDown = Input.GetMouseButton(0);
        FingerIndicator.SetActive(isMouseDown);

        // If visible, also update position.
        if (isMouseDown)
        {
            // Convert mouse position to world space.
            var mouseWorldPos = Camera.ScreenToWorldPoint(Input.mousePosition);

            // Position the finger indicator on top
            FingerIndicator.transform.position = new Vector3(mouseWorldPos.x, mouseWorldPos.y, -1f);
		}
    }
}
