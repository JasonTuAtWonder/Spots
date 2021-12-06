using UnityEngine;
using System.Collections.Generic;

[DefaultExecutionOrder((int)ExecutionOrder.BoardPresenter)]
public class BoardPresenter : MonoBehaviour
{
    [Header("Models")]
    [NotNull] public GameConfiguration GameConfiguration;
    [NotNull] public BoardModel BoardModel;

    [Header("Views")]
    [NotNull] public Camera Camera;

    void Awake()
    {
        InitializeBoard();
    }

    void Update()
    {
        UpdateCurrentTouch();

        if (BoardModel.CurrentMousePosition == null)
        {
            BoardModel.ConnectedSpots.Clear();
            return;
        }

        Ray ray = Camera.ScreenPointToRay(BoardModel.CurrentMousePosition.Value);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (BoardModel.ConnectedSpots.Count == 0)
            {
                // TODO: Does this need optimization?
                BoardModel.ConnectedSpots.Add(hit.collider.GetComponent<SpotModel>());
            }
            else
            {
                // Grab the last connected spot.
                var last = BoardModel.ConnectedSpots[BoardModel.ConnectedSpots.Count - 1];

                // Grab the spot that is being clicked on.
                var current = hit.collider.GetComponent<SpotModel>();

                // If they are the same, early return.
                if (last == current)
                    return;

                // If the colors don't match, early return.
                if (last.Color != current.Color)
                    return;

                // Grab the board position of both last and current spots.
                var lastBoardPos = last.transform.position;
                var currentBoardPos = current.transform.position;

                // If the spots are not cardinally adjacent, early return.
                if (!IsCardinallyAdjacent(lastBoardPos, currentBoardPos))
                {
                    return;
                }

                // Else, add the spot to the list of connected spots.
                BoardModel.ConnectedSpots.Add(current);

                // TODO: also check that the current spot isn't in the chain of spots already
            }
        }
    }

    void InitializeBoard()
    {
        for (var y = 0; y < GameConfiguration.Height; y++)
        {
            var row = new List<SpotModel>();

            for (var x = 0; x < GameConfiguration.Width; x++)
            {
                var spot = Instantiate(GameConfiguration.SpotPrefab);
                spot.transform.position = new Vector3(x, y, 0);
                row.Add(spot);
            }

            BoardModel.Spots.Add(row);
        }
    }

    void UpdateCurrentTouch()
    {
        if (Input.GetMouseButton(0))
            BoardModel.CurrentMousePosition = Input.mousePosition;
        else
            BoardModel.CurrentMousePosition = null;
    }

    static bool IsCardinallyAdjacent(Vector2 a, Vector2 b)
    {
        if (a.x == b.x && Mathf.Abs(a.y - b.y) == 1)
        {
            return true;
        }

        if (a.y == b.y && Mathf.Abs(a.x - b.x) == 1)
        {
            return true;
        }

        return false;
    }
}
