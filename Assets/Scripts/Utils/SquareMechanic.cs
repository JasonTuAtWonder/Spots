using UnityEngine;
using System.Linq;
using System.Collections.Generic;

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

    /// <summary>
    /// Check whether a line of spots follows a certain direction,
    /// with each spot 1 board unit after the other.
    ///
    /// For example, a line (with numbers indicating order) such as:
    ///
    ///     1 2 3
    ///
    /// Would satisfy the Direction.RIGHT criteria.
    ///
    /// Whereas a line such as:
    ///
    ///     3
    ///     2
    ///     1
    ///
    /// Would satisfy the Direction.UP criteria.
    /// </summary>
    private static bool LineFollowsDirection(List<SpotPresenter> lineOfSpots, Direction direction)
    {
        if (direction == Direction.INVALID)
            return false;

        for (var i = 0; i < lineOfSpots.Count - 1; i++)
        {
            var current = lineOfSpots[i].SpotModel.BoardPosition;
            var next = lineOfSpots[i + 1].SpotModel.BoardPosition;

            if (direction == Direction.UP)
            {
                if (next.y != current.y + 1)
                    return false;
            }
            else if (direction == Direction.DOWN)
            {
                if (next.y != current.y - 1)
                    return false;
            }
            else if (direction == Direction.LEFT)
            {
                if (next.x != current.x - 1)
                    return false;
            }
            else if (direction == Direction.RIGHT)
            {
                if (next.x != current.x + 1)
                    return false;
            }
            else
            {
                throw new System.Exception($"Unsupported direction: {direction}");
            }
        }

        return true;
    }

    /// <summary>
    /// Get the direction of spot2.BoardPosition - spot1.BoardPosition.
    ///
    /// If the 2 spots are not adjacent, then this method will return Direction.INVALID.
    /// </summary>
    private static Direction GetDirection(SpotPresenter spot1, SpotPresenter spot2)
    {
        var boardPos1 = spot1.SpotModel.BoardPosition;
        var boardPos2 = spot2.SpotModel.BoardPosition;

        var xDiff = Mathf.Abs(boardPos1.x - boardPos2.x);
        var yDiff = Mathf.Abs(boardPos1.y - boardPos2.y);

        if (xDiff + yDiff != 1)
            return Direction.INVALID;

        if (Sign(boardPos2.x - boardPos1.x) == global::Sign.ZERO)
        {
            // Then this is a vertical direction.
            var ySign = Sign(boardPos2.y - boardPos1.y);
            if (ySign == global::Sign.POSITIVE)
                return Direction.UP;
            else if (ySign == global::Sign.NEGATIVE)
                return Direction.DOWN;
        }
        else
        {
            // Then this is a horizontal direction.
            var xSign = Sign(boardPos2.x - boardPos1.x);
            if (xSign == global::Sign.POSITIVE)
                return Direction.RIGHT;
            else if (xSign == global::Sign.NEGATIVE)
                return Direction.LEFT;
        }

        return Direction.INVALID;
    }

    /// <summary>
    /// Flip a Direction enum.
    /// </summary>
    private static Direction Flip(Direction dir)
    {
        if (dir == Direction.UP)
        {
            return Direction.DOWN;
        }
        else if (dir == Direction.DOWN)
        {
            return Direction.UP;
        }
        else if (dir == Direction.LEFT)
        {
            return Direction.RIGHT;
        }
        else if (dir == Direction.RIGHT)
        {
            return Direction.LEFT;
        }
        else if (dir == Direction.INVALID)
        {
            return Direction.INVALID;
        }
        else
        {
            throw new System.Exception($"Unsupported direction: {dir}");
        }
    }

    /// <summary>
    /// Check whether a list of spots is in a square arrangement.
    ///
    /// NOTE: This method only detects squares – not rectangles.
    /// </summary>
    public static bool IsSquare(List<SpotPresenter> _spots)
    {
        var spots = new List<SpotPresenter>(_spots);
        var first = spots[0];
        var last = spots[spots.Count - 1];
        if (first != null && last != null && first == last)
        {
            // Remove the duplicate last element before running the IsSquare() check.
            spots.RemoveAt(spots.Count - 1);
		}

        if (!IsSquare(spots.Count))
        {
            return false;
        }

        // Compute the length of a square side.
        var squareLen = spots.Count / 4 + 1;

        // Ensure the direction of the first line segment is correct.
        var firstLineDirection = GetDirection(spots[0], spots[1]);
        var ok = LineFollowsDirection(spots.Take(squareLen).ToList(), firstLineDirection);
        if (!ok)
        {
            return false;
        }

        // Ensure the direction of the third line segment is correct.
        var thirdLineDirection = Flip(firstLineDirection);
        ok = LineFollowsDirection(spots.Skip(squareLen * 2 - 2).Take(squareLen).ToList(), thirdLineDirection);
        if (!ok)
        {
            return false;
        }

        // Ensure the direction of the second line segment is correct.
        var secondLineDirection = GetDirection(spots[squareLen - 1], spots[squareLen]);
        ok = LineFollowsDirection(spots.Skip(squareLen - 1).Take(squareLen).ToList(), secondLineDirection);
        if (!ok)
        {
            return false;
        }

        // Ensure the direction of the fourth line segment is correct.
        var fourthLineDirection = Flip(secondLineDirection);
        ok = LineFollowsDirection(spots.Skip(squareLen * 3 - 3).Take(squareLen).ToList(), fourthLineDirection);
        if (!ok)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Check whether 2 SpotPresenters are cardinally adjacent.
    ///
    /// That is, are they directly on top of each other? Or side-by-side?
    /// </summary>
    public static bool IsCardinallyAdjacent(SpotPresenter spot1, SpotPresenter spot2)
    {
        var a = spot1.SpotModel.BoardPosition;
        var b = spot2.SpotModel.BoardPosition;

        if (a.x == b.x && Mathf.Abs(a.y - b.y) == 1)
        {
            return true;
        }

        if (a.y == b.y && Mathf.Abs(a.x - b.x) == 1)
        {
            return true;
        }

        return false;
    }
}
