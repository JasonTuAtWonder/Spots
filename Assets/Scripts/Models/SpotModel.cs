using UnityEngine;

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

    public Vector2 DesiredWorldPosition
    {
        get
        {
            return Convert.BoardToWorldSpace(BoardPosition);
        }
    }
}
