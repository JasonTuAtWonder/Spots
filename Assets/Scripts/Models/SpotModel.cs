using UnityEngine;

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
        get
        {
			return Convert.WorldToBoardPosition(transform.position);
		}
    }

    // desired position

    // current position (transform.position)

    // current board position (BoardPosition)

    // easing curve (ease in out
}
