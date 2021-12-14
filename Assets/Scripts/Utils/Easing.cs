public static class Easing
{
    public static float EaseOutBounce(float x)
    {
		var n1 = 7.5625f;
	    var d1 = 2.75f;

		if (x < 1f / d1)
		{
		    return n1 * x * x;
		}
		else if (x < 2f / d1)
		{
		    return n1 * (x -= 1.5f / d1) * x + 0.75f;
		}
		else if (x < 2.5 / d1)
		{
		    return n1 * (x -= 2.25f / d1) * x + 0.9375f;
		}
		else
		{
		    return n1 * (x -= 2.625f / d1) * x + 0.984375f;
		}
	}
}
