using UnityEngine;
using System.Collections;

public class LineBarProgressPresenter : MonoBehaviour
{
    [Header("Models")]
    [NotNull] public BoardModel BoardModel;

    [Header("Views")]
    [NotNull] public MeshRenderer MeshRenderer;

    Material mat;
    int lastConnectedSpotsCount = -1;
    Coroutine currentAnimation;

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
        UpdateProgress();
    }

    void UpdateProgress()
    {
        var count = BoardModel.ConnectedSpots.Count;
        if (lastConnectedSpotsCount != count)
        {
            // Update bookkeeping variable.
            lastConnectedSpotsCount = count;

            // Clear last animation.
            if (currentAnimation != null)
            { 
                StopCoroutine(currentAnimation);
                currentAnimation = null;
		    }

            // Animate progress bar to desired progress value.
            currentAnimation = StartCoroutine(ToDesiredProgress(count * .1f, .1f));
		}
    }

    void UpdateColor()
    { 
        var count = BoardModel.ConnectedSpots.Count;
        if (count > 0)
            SetColor(BoardModel.ConnectedSpots[0].Color);
    }

    void SetColor(Color color)
    {
        mat.SetColor("_Color", color);
    }

    IEnumerator ToDesiredProgress(float desiredProgress, float duration)
    {
        var from = mat.GetFloat("_Health");
        var to = desiredProgress;
        var journey = 0f;

        while (journey <= duration)
        {
            // Update elapsed time.
            journey += Time.deltaTime;

            // Update fields using elapsed time.
            float t = Mathf.Clamp01(journey / duration);
            float value = Mathf.Lerp(from, to, t);
			mat.SetFloat("_Health", value);

            // Complete one step of coroutine execution.
            yield return null;
        }
    }
}
