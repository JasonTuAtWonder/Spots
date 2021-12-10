public static class SquareMechanic
{
    /// <summary>
    /// Check whether an integer `num` is a perfect square.
    ///
    /// This implementation uses a brute-force approach, which is fine when `num` is fairly small.
    /// </summary>
    public static bool IsSquare(int num)
    {
        for (int i = 1, square = 1; square <= num; i++, square = i * i)
        {
            if (square == num)
            { 
                return true;
		    }
		}

        return false;
    }
}
