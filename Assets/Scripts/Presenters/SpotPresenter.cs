using UnityEngine;
using System.Collections;

[DefaultExecutionOrder((int)ExecutionOrder.SpotPresenter)]
public class SpotPresenter : MonoBehaviour
{
    [Header("Models")]
    [NotNull] public SpotModel SpotModel;

    [Header("Views")]
    [NotNull] public MeshRenderer SpotView;
    [NotNull] public GameObject Scaler;
    public AnimationCurve DisconnectAnimation;

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
		var worldPos = Convert.BoardToWorldPosition(SpotModel.BoardPosition);
        worldPos += new Vector2(0f, 60f);
        transform.position = worldPos;
    }

    public IEnumerator AnimateToDesired(float duration)
    {
        var from = transform.position;
        var to = SpotModel.DesiredPosition;
        var journey = 0f;

        while (journey <= duration)
        {
            // Update elapsed time.
            journey += Time.deltaTime;

            // Update position using elapsed time.
            float t = Mathf.Clamp01(journey / duration);
            t = Easing.EaseOutBounce(t); // Make animation bouncy.
            if (this != null)
            { 
			    transform.position = Vector3.Lerp(from, to, t);
		    }

            // Complete one step of coroutine execution.
            yield return null;
        }
    }

    /// <summary>
    /// Kick off a "disconnect" animation, then destroy the GameObject once the animation is done.
    /// </summary>
    public IEnumerator AnimateDestroy(SpotPresenter spotPresenter, float duration)
    {
        // Animate the scale with bounce:

        var from = Vector3.one;
        var to = Vector3.zero;
        var journey = 0f;

        while (journey <= duration)
        {
            // Update elapsed time.
            journey += Time.deltaTime;

            // Update position using elapsed time.
            float t = Mathf.Clamp01(journey / duration);
            // t = Easing.EaseInOutElastic(t); // Make animation bouncy.
            // Easing.GetPoint(t);
            t = DisconnectAnimation.Evaluate(t);
            if (this != null)
                spotPresenter.Scaler.transform.localScale = Vector3.LerpUnclamped(from, to, t); // Update scale.

            // Complete one step of coroutine execution.
            yield return null;
        }

        // Once animation is complete, destroy the GameObject.
        Destroy(spotPresenter.gameObject);

        yield return null;
    }
}
