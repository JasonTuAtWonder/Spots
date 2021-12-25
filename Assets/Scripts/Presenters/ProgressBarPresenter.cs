using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ProgressBarPresenter presents the progress bar,
/// which provides feedback on the players' number of connected dots.
/// </summary>
[DefaultExecutionOrder((int)ExecutionOrder.ProgressBarFeedback)]
public class ProgressBarPresenter : MonoBehaviour
{
    [Header("Models")]
    [NotNull] public BoardViewModel BoardModel;

    [Header("Views")]
    [NotNull] public MeshRenderer MeshRenderer;

    /// <summary>
    /// The material instance of the rendered object.
    /// </summary>
    Material mat;

    /// <summary>
    /// The currently-running progress bar animation, if any.
    /// </summary>
	Coroutine currentAnimation = null;

    void OnEnable()
    {
        // Create instance of the rendered object's material.
        mat = MeshRenderer.material;
        Assert.IsNotNull(mat, "Progress bar material should not be null.");

        // Kick off coroutine that updates progress.
        StartCoroutine(UpdateProgress(this, mat, BoardModel));
    }

    void OnDestroy()
    {
        // Clean up material to avoid memory leak.
        Destroy(mat);
    }

    void OnDisable()
    {
        // Ensure all coroutines are stopped on domain reload.
        StopAllCoroutines();
    }

    void Update()
    {
        UpdateColor(mat, BoardModel.ConnectedSpots);
        UpdateBackgroundColor(mat, BoardModel);
    }

    /// <summary>
    /// Update the color of a material based on the color of connectedSpots.
    ///
    /// No assumption is made about whether each spot in connectedSpots has the same color.
    /// </summary>
    static void UpdateColor(Material progressBarMaterial, List<SpotPresenter> connectedSpots)
    {
        var count = connectedSpots.Count;
        if (count == 0)
            return;

        var color = connectedSpots[0].SpotModel.Color;
        progressBarMaterial.SetColor("_Color", color);
    }

    static void UpdateBackgroundColor(Material progressBarMaterial, BoardViewModel boardModel)
    {
        var spots = boardModel.ConnectedSpots;
        if (boardModel.IsClosedSquare && spots.Count > 0)
        {
            // Grab the color of the connected spots.
            var col = spots[0].SpotModel.Color;

            // Set background color to a shade of the selected color.
            var partiallyTransparentColor = new Color(col.r, col.g, col.b, .25f);
            progressBarMaterial.SetColor("_BackgroundColor", partiallyTransparentColor);
        }
        else
        {
            // Set background color to transparent.
            var transparent = new Color(0, 0, 0, 0);
            progressBarMaterial.SetColor("_BackgroundColor", transparent);
        }
    }

    static void StopCurrentAnimation(ProgressBarPresenter self)
    {
        if (self.currentAnimation != null)
        {
            self.StopCoroutine(self.currentAnimation);
            self.currentAnimation = null;
        }
    }

    /// <summary>
    /// Update the progress indicator.
    /// </summary>
    static IEnumerator UpdateProgress(ProgressBarPresenter self, Material progressBarMaterial, BoardViewModel boardModel)
    {
        int lastConnectedSpotsCount = -1;

        while (true)
        {
            var count = boardModel.ConnectedSpots.Count;

            // If we saw a square loop, set the progress to 100%.
            if (boardModel.IsClosedSquare)
            {
                StopCurrentAnimation(self);
                progressBarMaterial.SetFloat("_Health", 1);
            }
            // If the player disconnected some spots this frame, set the progress to 0%.
            else if (lastConnectedSpotsCount > 0 && count == 0)
            {
                StopCurrentAnimation(self);
                progressBarMaterial.SetFloat("_Health", 0);
            }
            // If we *previously* saw a square loop, but no longer see one, set the progress to the desired value.
            else if (boardModel.WasClosedSquare && !boardModel.IsClosedSquare)
            {
                StopCurrentAnimation(self);
                progressBarMaterial.SetFloat("_Health", count * .1f);
            }
            // Otherwise, animate the progress bar to the desired value.
            else if (lastConnectedSpotsCount != count)
            {
                var asyncOp = ToDesiredProgress(self, progressBarMaterial, count * .1f, .1f);
                self.currentAnimation = self.StartCoroutine(asyncOp);
            }

            // Update bookkeeping variable, and complete 1 step of coroutine execution.
            lastConnectedSpotsCount = count;
            yield return null;
        }
    }

    /// <summary>
    /// Animate to the `desiredProgress` value over `duration` seconds.
    /// </summary>
    static IEnumerator ToDesiredProgress(ProgressBarPresenter self, Material progressBarMaterial, float desiredProgress, float duration)
    {
        var from = progressBarMaterial.GetFloat("_Health");
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
            progressBarMaterial.SetFloat("_Health", value);

            // Complete one step of coroutine execution.
            yield return null;
        }

        // Coroutine is done, remember to clear reference.
        self.currentAnimation = null;
    }
}
