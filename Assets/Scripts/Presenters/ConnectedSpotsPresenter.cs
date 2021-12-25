using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ConnectedSpotsPresenter visually connects like-colored dots that were connected by the player.
/// </summary>
public class ConnectedSpotsPresenter : MonoBehaviour
{
    [Header("Models")]
    [NotNull] public BoardViewModel BoardModel;

    [Header("Views")]
    [NotNull] public Camera Camera;
    [NotNull] public LineRenderer ConnectedDots;
    [NotNull] public LineRenderer LastDotToMousePointer;

    void Awake()
    {
        ConnectedDots.startWidth = 2f;
        LastDotToMousePointer.startWidth = 2f;
    }

    void Update()
    {
        UpdateLineColor();
        UpdateConnectedDots();
        UpdateLastDotToMousePointer();
    }

    void UpdateLineColor()
    {
        var spots = BoardModel.ConnectedSpots;
        if (spots.Count > 0)
        {
            var spot = spots[0];
            ConnectedDots.startColor = spot.SpotModel.Color;
            ConnectedDots.endColor = spot.SpotModel.Color;
            LastDotToMousePointer.startColor = spot.SpotModel.Color;
            LastDotToMousePointer.endColor = spot.SpotModel.Color;
        }
    }

    /// <summary>
    /// Update the ConnectedDots LineRenderer to reflect the currently-connected dots' positions.
    /// </summary>
    void UpdateConnectedDots()
    {
        var positions = new List<Vector3>();
        var spots = BoardModel.ConnectedSpots;

        foreach (var spot in spots)
            positions.Add(spot.transform.position);

        ConnectedDots.positionCount = positions.Count;
        ConnectedDots.SetPositions(positions.ToArray());
    }

    /// <summary>
    /// Update the LastDotToMousePointer LineRenderer to reflect the current state of the left mouse button.
    /// </summary>
    void UpdateLastDotToMousePointer()
    {
        var positions = new List<Vector3>();
        var spots = BoardModel.ConnectedSpots;

        // If the left mouse button is held,
        if (spots.Count > 0 && Input.GetMouseButton(0))
        {
            // Populate our list of positions, which we will set on the LastDotToMousePointer LineRenderer:

            var last = spots[spots.Count - 1];
            positions.Add(last.transform.position);

            var mouseWorldPos = Camera.ScreenToWorldPoint(Input.mousePosition);
            positions.Add(new Vector2(mouseWorldPos.x, mouseWorldPos.y));
        }

        LastDotToMousePointer.positionCount = positions.Count;
        LastDotToMousePointer.SetPositions(positions.ToArray());
    }
}
