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
        UpdateConnectedSpots();
        HandleDisconnects();
        // ReplenishSpots();
    }

    void ReplenishSpots()
    {
        // Update the board so that empty spots are filled by the spots above:

        // For each column,
        for (var x = 0; x < GameConfiguration.Width; x++)
        { 
			// Create a new column containing the non-null nodes.
			List<SpotModel> newColumn = new List<SpotModel>();

            // Populate the new column.
            for (var y = 0; y < GameConfiguration.Height; y++)
            {
                var spot = BoardModel.Spots[y][x];
                if (spot != null)
                    newColumn.Add(spot);
		    }

            // Finally, update the grid. (Cache locality isn't great. :P)
            for (var y = 0; y < newColumn.Count; y++)
                BoardModel.Spots[y][x] = newColumn[y];
            for (var y = newColumn.Count; y < GameConfiguration.Height; y++)
                BoardModel.Spots[y][x] = null;

            throw new System.Exception("jason look here");

            // TODO: Update each spot's position. This should be done in SpotPresenter.
            // ...

			// TODO: Generate new spots to fill in the remainder of the row.
			// ...
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
                spot.transform.position = BoardToWorldPosition(new Vector2(x, y));
                row.Add(spot);
            }

            BoardModel.Spots.Add(row);
        }
    }

    static bool IsCardinallyAdjacent(Vector2 a, Vector2 b)
    {
        if (a.x == b.x && Mathf.Abs(a.y - b.y) == 10f)
        {
            return true;
        }

        if (a.y == b.y && Mathf.Abs(a.x - b.x) == 10f)
        {
            return true;
        }

        return false;
    }

    void UpdateConnectedSpots()
    {
        if (!Input.GetMouseButton(0))
            return;

        var mouseWorldPos = Input.mousePosition;
        Ray ray = Camera.ScreenPointToRay(mouseWorldPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (BoardModel.ConnectedSpots.Count == 0)
            {
                BoardModel.ConnectedSpots.Add(hit.collider.GetComponent<SpotModel>());
            }
            else
            {
                // Grab the last connected spot.
                var last = BoardModel.ConnectedSpots[BoardModel.ConnectedSpots.Count - 1];

                // Grab the spot that is being clicked on.
                var current = hit.collider.GetComponent<SpotModel>();

                // If the current spot is the same as any of the spots in the list, early return.
                var found = BoardModel.ConnectedSpots.Find(spot => spot == current);
                if (found)
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
            }
        }
    }

    void HandleDisconnects()
    {
        if (!Input.GetMouseButtonUp(0))
            return;

		// Grab a copy of the list of connected dots.
        var connectedSpots = BoardModel.ConnectedSpots;
		var spots = new List<SpotModel>(connectedSpots);

		// Then clear the connected dots from the board.
		connectedSpots.Clear();

        // Perform a "disconnect" only if there are 2 or more connected spots.
        if (spots.Count >= 2)
        { 
			foreach (var spot in spots)
			{
			    // Find the spot's position on the board.
			    var boardPos = WorldToBoardPosition(spot.transform.position);
			    var x = boardPos.Item1;
			    var y = boardPos.Item2;

			    // Clear that spot from the grid (leaving a `null` in its place).
			    var toDestroy = BoardModel.Spots[y][x].gameObject;
			    Destroy(toDestroy);
			    BoardModel.Spots[y][x] = null;
			}
		}
    }

    /// <summary>
    /// Given a Vector2 in board space, convert the board space coordinates into world space coordinates.
    ///
    /// Example: (1, 2) in board space translates to (10, 20) in world space.
    ///
    /// There is a difference between the two spaces mostly cause LineRenderer likes working with
    /// larger line widths than 1. So this function allows us to customize the conversion between
    /// the two coordinate spaces.
    /// </summary>
    public static Vector2 BoardToWorldPosition(Vector2 boardPos)
    {
        return boardPos * 10f;
    }

    /// <summary>
    /// Given a Vector2 in world space, convert the world space coordinates into "board space" coordinates.
    ///
    /// Example: (10, 20) in world space translates to (1, 2) in board space.
    ///
    /// So we are referring to the spot that is 1 spot to the right of the left row,
    /// and 2 spots up from the bottom row.
    /// </summary>
    public static (int, int) WorldToBoardPosition(Vector2 worldPos)
    {
        var boardPos = worldPos * 0.1f;
        var x = Mathf.FloorToInt(boardPos.x);
        var y = Mathf.FloorToInt(boardPos.y);
        return (x, y);
    }
}
