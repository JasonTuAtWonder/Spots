using UnityEngine;

/// <summary>
/// Easing contains easing functions copied from easings.net.
/// </summary>
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

	public static float EaseInOutElastic(float x)
	{ 
		var c5 = (2f * Mathf.PI) / 4.5f;
		return x == 0f
		  ? 0f
		  : x == 1f
		  ? 1f
		  : x < 0.5f
		  ? -(Mathf.Pow(2f, 20f * x - 10f) * Mathf.Sin((20f * x - 11.125f) * c5)) / 2f
		  : (Mathf.Pow(2f, -20f * x + 10f) * Mathf.Sin((20f * x - 11.125f) * c5)) / 2f + 1f;
    }

	public static float EaseOutElastic(float x)
	{ 
		var c4 = (2f * Mathf.PI) / 3f;
		return x == 0f
		  ? 0f
		  : x == 1f
		  ? 1f
		  : Mathf.Pow(2f, -10f * x) * Mathf.Sin((x* 10f - 0.75f) * c4) + 1f;
    }
}
