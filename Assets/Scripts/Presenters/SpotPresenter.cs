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

    /// <summary>
    /// Note: OnEnable() is used instead of Awake() so the callback is called after domain reload.
    /// </summary>
    void OnEnable()
    {
        spotMaterial = SpotView.material;
        SetColor();
    }

    /// <summary>
    /// Destroy the GameObject so it doesn't appear after domain reload.
    /// </summary>
    void OnDisable()
    {
        Destroy(gameObject);
    }

    void SetColor()
    {
        spotMaterial.color = SpotModel.Color;
    }

    public void SetWorldPosition()
    { 
		var worldPos = Convert.BoardToWorldSpace(SpotModel.BoardPosition);
        worldPos += new Vector2(0f, 100f);
        transform.position = worldPos;
    }

    private IEnumerator FallAndBounce(float duration)
    {
        var pos = transform.position;
        var from = new Vector2(pos.x, pos.y);
        var to = SpotModel.DesiredWorldPosition;
        var journey = 0f;

        // Do not run animation if we are already at the desired position.
        //
        // Apparently '==' tests for approximate equality (https://docs.unity3d.com/ScriptReference/Vector3-operator_eq.html):
        if (from == to)
            yield break;

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
