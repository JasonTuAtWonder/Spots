using UnityEngine;

/// <summary>
/// GameConfiguration provides options for configuring the game's variables.
/// </summary>
[CreateAssetMenu]
public class GameConfiguration : ScriptableObject
{
    [Tooltip("Width of the board, in spots.")]
    public float Width;

    [Tooltip("Height of the board, in spots.")]
    public float Height;

    [Tooltip("Set of possible colors for spots.")]
    [NotNull] public Color[] Colors;

    [Tooltip("Whether test mode is enabled. If enabled, the number of spot colors is restricted for ease of matching.")]
    public bool IsTestMode;

    /// <summary>
    /// Choose a random color from GameConfiguration's set of possible colors.
    /// </summary>
    public Color RandomColor()
    {
        var len = Colors.Length;
        if (IsTestMode)
        { 
			// Temporarily choose fewer colors so it's easier to test matching spots.
			len = 2;
		}

        var i = Random.Range(0, len);
        return Colors[i];
    }
}
