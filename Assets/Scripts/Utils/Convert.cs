using UnityEngine;

public static class Convert
{
    /// <summary>
    /// Given a Vector2 in board space, convert the board space coordinates into world space coordinates.
    ///
    /// Example: (1, 2) in board space translates to (10, 20) in world space.
    ///
    /// There is a difference between the two spaces mostly cause LineRenderer likes working with
    /// larger line widths than 1. So this function allows us to customize the conversion between
    /// the two coordinate spaces.
    /// </summary>
    public static Vector2 BoardToWorldPosition(Vector2Int boardPos)
    {
        return boardPos * 10;
    }

    /// <summary>
    /// Given a Vector2 in world space, convert the world space coordinates into "board space" coordinates.
    ///
    /// Example: (10, 20) in world space translates to (1, 2) in board space.
    ///
    /// So we are referring to the spot that is 1 spot to the right of the left row,
    /// and 2 spots up from the bottom row.
    /// </summary>
    public static Vector2Int WorldToBoardPosition(Vector2 worldPos)
    {
        var boardPos = worldPos * 0.1f;
        var x = Mathf.FloorToInt(boardPos.x);
        var y = Mathf.FloorToInt(boardPos.y);
        return new Vector2Int(x, y);
    }
}
