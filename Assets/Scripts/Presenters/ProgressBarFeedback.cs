using UnityEngine;
using System.Collections;

/// <summary>
/// ProgressBarFeedback presents the progress bar,
/// which provides feedback on the players' number of connected dots.
/// </summary>
[DefaultExecutionOrder((int)ExecutionOrder.ProgressBarFeedback)]
public class ProgressBarFeedback : MonoBehaviour
{
    [Header("Models")]
    [NotNull] public BoardViewModel BoardModel;

    [Header("Views")]
    [NotNull] public MeshRenderer MeshRenderer;

    /// <summary>
    /// The number of connected spots during the previous frame.
    /// </summary>
    int lastConnectedSpotsCount = -1;

    /// <summary>
    /// The currently-running progress bar animation.
    /// </summary>
    Coroutine currentAnimation;

    Material mat;

    void Awake()
    {
        mat = MeshRenderer.material;
    }

    void OnDestroy()
    {
        Destroy(mat);
    }

    void Update()
    {
        UpdateColor();
        UpdateProgress(BoardModel.IsClosedSquare);
    }

    /// <summary>
    /// Update the color of the progress bar.
    /// </summary>
    void UpdateColor()
    { 
        var count = BoardModel.ConnectedSpots.Count;
        if (count > 0)
        {
            var color = BoardModel.ConnectedSpots[0].SpotModel.Color;
			mat.SetColor("_Color", color);
		}
    }

    void StopAnimation()
    { 
        if (currentAnimation != null)
        { 
			StopCoroutine(currentAnimation);
			currentAnimation = null;
		}
    }

    /// <summary>
    /// Update the progress indicator.
    ///
    /// TODO: Logic needs work for rectangles and squares.
    /// </summary>
    void UpdateProgress(bool didSeeRectangle)
    {
        if (didSeeRectangle)
        {
            // Set background color to a shade of the selected color.
            var color = BoardModel.ConnectedSpots[0].SpotModel.Color;
            var partiallyTransparentColor = new Color(color.r, color.g, color.b, .25f);
            mat.SetColor("_BackgroundColor", partiallyTransparentColor);

            // Immediately set progress to full.
            StopAnimation();
            mat.SetFloat("_Health", 1);

            // Early return.
            return;
		}

	    // Set background color to transparent.
	    var transparent = new Color(0, 0, 0, 0);
	    mat.SetColor("_BackgroundColor", transparent);

        // Update the progress bar to reflect the current value.
        var count = BoardModel.ConnectedSpots.Count;
        if (lastConnectedSpotsCount > 0 && count == 0)
        {
            // Immediately set progress to zero.
            StopAnimation();
            mat.SetFloat("_Health", 0);
		}
        else if (lastConnectedSpotsCount != count)
        {
            // Clear last animation.
			StopAnimation();

            // Animate progress bar to desired progress value.
            currentAnimation = StartCoroutine(ToDesiredProgress(count * .1f, .1f));

            // Update bookkeeping variable.
            lastConnectedSpotsCount = count;
		}
    }

    /// <summary>
    /// Animate to the `desiredProgress` value over `duration` seconds.
    /// </summary>
    IEnumerator ToDesiredProgress(float desiredProgress, float duration)
    {
        var from = mat.GetFloat("_Health");
        var to = desiredProgress;
        var journey = 0f;

        while (journey <= duration)
        {
            // Update elapsed time.
            journey += Time.deltaTime;

            // Compute lerp value.
            float t = Mathf.Clamp01(journey / duration);
            float value = Mathf.Lerp(from, to, t);

            // Update fields using elapsed time.
			mat.SetFloat("_Health", value);

            // Complete one step of coroutine execution.
            yield return null;
        }
    }
}
