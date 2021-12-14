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
}
