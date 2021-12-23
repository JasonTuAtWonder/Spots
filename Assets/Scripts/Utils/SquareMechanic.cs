using UnityEngine;

public enum Sign
{ 
	POSITIVE,
    NEGATIVE,
    ZERO,
}

public enum Direction
{ 
    UP,
    DOWN,
    LEFT,
    RIGHT,
    INVALID,
}

/// <summary>
/// SquareMechanic contains game logic for detecting "squares" on the game board.
/// </summary>
public static class SquareMechanic
{
    /// <summary>
    /// Check whether a number is a "square" number.
    ///
    /// That is, the number is a number of spots that form a square outline shape.
    ///
    /// For example:
    ///
    ///     o o
    ///     o o
    ///
    /// Or:
    ///
    ///     o o o o
    ///     o     o
    ///     o     o
    ///     o o o o
    ///
    /// </summary>
    public static bool IsSquare(int num)
    {
        return num > 0 && num % 4 == 0;
    }

    /// <summary>
    /// Get the sign of the position difference between spot1 and spot2.
    ///
    /// Unlike Mathf.Sign, this method returns an enum representing positive, negative, or zero sign.
    /// </summary>
    public static Sign Sign(int num)
    {
        if (num == 0)
            return global::Sign.ZERO;

        var sign = Mathf.Sign(num);
        if (sign < 0)
            return global::Sign.NEGATIVE;

        return global::Sign.POSITIVE;
    }
}
