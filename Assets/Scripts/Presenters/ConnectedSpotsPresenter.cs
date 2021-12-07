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
    [NotNull] public LineRenderer LineRenderer;
    [NotNull] public Camera Camera;

    void Update()
    {
        var spots = BoardModel.ConnectedSpots;

        // Map spots -> transforms.
        var positions = new List<Vector3>();
        foreach (var spot in spots)
        {
            // local space
            positions.Add(spot.transform.position);
        }

        LineRenderer.startWidth = .1f;
        LineRenderer.endWidth = .1f;

        // Add the mouse position last.
        if (positions.Count > 0)
        {
            var mouseWorldPos = Camera.ScreenToWorldPoint(Input.mousePosition);
            positions.Add(new Vector2(mouseWorldPos.x, mouseWorldPos.y));
        }

        // Update the line renderer with the board model's connected spots.
        LineRenderer.positionCount = positions.Count;
        if (positions.Count > 0)
        {
            LineRenderer.SetPositions(positions.ToArray());
        }

        // TODO: line renderer takes the color of the first spot.

        // TODO: last point of the line renderer is also the mouse position
    }
}
