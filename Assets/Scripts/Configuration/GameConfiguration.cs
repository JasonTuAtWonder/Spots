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
    public Color[] Colors;

    [Tooltip("Whether test mode is enabled. If enabled, the number of spot colors is restricted for ease of matching. Note that this only applies in the Unity Editor.")]
    public bool IsTestMode;

    Color[] ShuffledColors;

    void OnEnable()
    {
        ShuffledColors = Shuffle(Colors);
    }

    /// <summary>
    /// Shuffle an array of colors and return the result.
    /// </summary>
    static Color[] Shuffle(Color[] colors)
    {
        var shuffledColors = new Color[colors.Length];

        // Copy over colors first.
        for (var i = 0; i < colors.Length; i++)
        {
            shuffledColors[i] = colors[i];
		}

        // Then shuffle the colors.
        for (var i = colors.Length - 1; i >= 1; i--)
        {
            var j = Random.Range(0, i+1);

            // Shuffle.
            var temp = shuffledColors[i];
            shuffledColors[i] = shuffledColors[j];
            shuffledColors[j] = temp;
		}

        return shuffledColors;
    }

    /// <summary>
    /// Choose a random color from GameConfiguration's set of possible colors.
    /// </summary>
    public Color RandomColor()
    {
        var len = ShuffledColors.Length;
#if UNITY_EDITOR
        if (IsTestMode)
        {
            // Temporarily choose fewer colors so it's easier to test matching spots.
            len = 2;
		}
#endif

        var i = Random.Range(0, len);
        return ShuffledColors[i];
    }
}
