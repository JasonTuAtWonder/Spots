using UnityEngine;
using System.Collections;

[DefaultExecutionOrder((int)ExecutionOrder.SpotModel)]
public class SpotModel : MonoBehaviour
{
    [NotNull] public GameConfiguration GameConfiguration;

    public Color Color
    {
        get;
        private set;
    }

    void Awake()
    {
        Color = GameConfiguration.RandomColor();
    }

    public Vector2Int BoardPosition
    {
        get;
        set;
    }

    public Vector2 DesiredPosition
    {
        get
        {
            return Convert.BoardToWorldPosition(BoardPosition);
		}
    }

    public IEnumerator AnimateToDesired(float duration)
    {
        var from = transform.position;
        var to = DesiredPosition;
        var journey = 0f;

        while (journey <= duration)
        {
            // Update elapsed time.
            journey += Time.deltaTime;

            // Update position using elapsed time.
            float t = Mathf.Clamp01(journey / duration);
            t = Easing.EaseOutBounce(t); // Make animation bouncy.
            transform.position = Vector3.Lerp(from, to, t);

            // Complete one step of coroutine execution.
            yield return null;
        }
    }
}
