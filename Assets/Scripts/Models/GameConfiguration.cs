using UnityEngine;

[CreateAssetMenu]
public class GameConfiguration : ScriptableObject
{
    [Tooltip("Width of the board, in spots.")]
    public float Width;

    [Tooltip("Height of the board, in spots.")]
    public float Height;

    [Tooltip("Prefab for a Spot.")]
    [NotNull] public SpotModel SpotPrefab;

    [Tooltip("Set of possible colors for spots.")]
    public Color[] Colors;

    /// <summary>
    /// Choose a random color from GameConfiguration's set of possible colors.
    /// </summary>
    public Color RandomColor()
    {
        var len = Colors.Length;
#if UNITY_EDITOR
        // Temporarily choose 1 color only so I can get squares easily.
        len = 2;
#endif

        var i = Random.Range(0, len);
        return Colors[i];
    }
}
