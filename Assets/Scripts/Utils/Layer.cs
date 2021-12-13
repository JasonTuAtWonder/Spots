using System.Collections.Generic;
using UnityEngine;

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
        "Spots Floor 0",
        "Spots Floor 1",
        "Spots Floor 2",
        "Spots Floor 3",
        "Spots Floor 4",
        "Spots Floor 5",
        "Spots Floor 6",
        "Spots Floor 7",
        "Spots Floor 8",
        "Spots Floor 9",
    };

    /// <summary>
    /// Given a floor index, get the corresponding layer index.
    ///
    /// This is a convenience method that makes it easier to fetch layer indices.
    /// </summary>
    public static int Floor(int floorIndex)
    {
        return LayerMask.NameToLayer(FloorNames[floorIndex]);
    }
}
