using UnityEngine;
using System.Collections;

public class SpotPresenter : MonoBehaviour
{
    [Header("Models")]
    [NotNull] public SpotModel SpotModel;

    [Header("Views")]
    [NotNull] public MeshRenderer SpotView;
    [NotNull] public GameObject Scaler;
    public AnimationCurve DisappearAnimation;

    Material spotMaterial;

    void Awake()
    {
        spotMaterial = SpotView.material;
        SetColor();
    }

    void SetColor()
    {
        spotMaterial.color = SpotModel.Color;
    }

    public void SetWorldPosition()
    { 
		var worldPos = Convert.BoardToWorldSpace(SpotModel.BoardPosition);
        worldPos += new Vector2(0f, 60f);
        transform.position = worldPos;
    }

    private IEnumerator FallAndBounce(float duration)
    {
        var from = transform.position;
        var to = SpotModel.DesiredWorldPosition;
        var journey = 0f;

        while (journey <= duration)
        {
            // Update elapsed time.
            journey += Time.deltaTime;

            // Compute lerp value.
            float t = Mathf.Clamp01(journey / duration);
            t = Easing.EaseOutBounce(t);

            // Update position.
		    transform.position = Vector3.Lerp(from, to, t);

            // Complete one step of coroutine execution.
            yield return null;
        }
    }

    public void AnimateFallAndBounce(float duration)
    {
        StartCoroutine(FallAndBounce(duration));
    }

    /// <summary>
    /// Kick off a "disappear" animation, then destroy the GameObject once the animation is done.
    /// </summary>
    private IEnumerator Disappear(float duration)
    {
        var from = Vector3.one;
        var to = Vector3.zero;
        var journey = 0f;

        while (journey <= duration)
        {
            // Update elapsed time.
            journey += Time.deltaTime;

            // Compute lerp value.
            float t = Mathf.Clamp01(journey / duration);
            t = DisappearAnimation.Evaluate(t);

            // Update scale.
			Scaler.transform.localScale = Vector3.LerpUnclamped(from, to, t);

            // Complete one step of coroutine execution.
            yield return null;
        }

        // Once animation is complete, destroy the GameObject.
        Destroy(gameObject);
    }

    public void AnimateDisappear(float duration)
    {
        StartCoroutine(Disappear( duration));
    }
}
