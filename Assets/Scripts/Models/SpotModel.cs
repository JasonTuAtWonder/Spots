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
}
