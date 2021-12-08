using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ConnectedSpotsPresenter visually connects like-colored dots that were connected by the player.
/// </summary>
public class ConnectedSpotsPresenter : MonoBehaviour
{
    [Header("Models")]
    [NotNull] public BoardModel BoardModel;

    [Header("Views")]
    [NotNull] public Camera Camera;
    [NotNull] public LineRenderer ConnectingLines;
    [NotNull] public LineRenderer MousePointerLine;

    void Awake()
    {
        ConnectingLines.startWidth = 2f;
        MousePointerLine.startWidth = 2f;
    }

    void Update()
    {
        UpdateConnectingLines();
        UpdateMousePointerLine();

        // TODO: line renderer takes the color of the first spot.
    }

    void UpdateConnectingLines()
    { 
        var positions = new List<Vector3>();
        var spots = BoardModel.ConnectedSpots;

        foreach (var spot in spots)
            positions.Add(spot.transform.position);

        ConnectingLines.positionCount = positions.Count;
	    ConnectingLines.SetPositions(positions.ToArray());
    }

    void UpdateMousePointerLine()
    {
        var positions = new List<Vector3>();
        var spots = BoardModel.ConnectedSpots;

        if (spots.Count > 0)
        {
            var last = spots[spots.Count - 1];
            positions.Add(last.transform.position);

            var mouseWorldPos = Camera.ScreenToWorldPoint(Input.mousePosition);
            positions.Add(new Vector2(mouseWorldPos.x, mouseWorldPos.y));
        }

        MousePointerLine.positionCount = positions.Count;
	    MousePointerLine.SetPositions(positions.ToArray());
    }
}
