using UnityEngine;

public static class ColorExtensions
{
    public static bool Equals(this Color color1, Color color2)
    {
        var r = Mathf.Approximately(color1.r, color2.r);
        var g = Mathf.Approximately(color1.g, color2.g);
        var b = Mathf.Approximately(color1.b, color2.b);
        var a = Mathf.Approximately(color1.a, color2.a);
        return r && g && b && a;
    }
}
