using System.Collections.Generic;

public static class Layer
{
    /// <summary>
    /// List of GameObject layer names that you can use for setting a GameObject's layer.
    ///
    /// For example,
    ///
    ///     gameObject.layer = LayerMask.NameToLayer(Layer.FloorNames[1]);
    /// </summary>
    public static List<string> FloorNames = new List<string>
    {
        "INVALID",
        "Spots Floor 1",
        "Spots Floor 2",
        "Spots Floor 3",
        "Spots Floor 4",
        "Spots Floor 5",
        "Spots Floor 6",
        "Spots Floor 7",
        "Spots Floor 8",
        "Spots Floor 9",
        "Spots Floor 10",
    };
}
