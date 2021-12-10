using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEngine.Assertions;
#endif

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
#if UNITY_EDITOR
        Assert.AreEqual(SquareMechanic.IsSquare(0), false);
	    Assert.AreEqual(SquareMechanic.IsSquare(1), true);
	    Assert.AreEqual(SquareMechanic.IsSquare(2), false);
	    Assert.AreEqual(SquareMechanic.IsSquare(3), false);
	    Assert.AreEqual(SquareMechanic.IsSquare(4), true);
	    Assert.AreEqual(SquareMechanic.IsSquare(5), false);
	    Assert.AreEqual(SquareMechanic.IsSquare(6), false);
	    Assert.AreEqual(SquareMechanic.IsSquare(7), false);
	    Assert.AreEqual(SquareMechanic.IsSquare(8), false);
	    Assert.AreEqual(SquareMechanic.IsSquare(9), true);
	    Assert.AreEqual(SquareMechanic.IsSquare(10), false);
	    Assert.AreEqual(SquareMechanic.IsSquare(11), false);
	    Assert.AreEqual(SquareMechanic.IsSquare(12), false);
	    Assert.AreEqual(SquareMechanic.IsSquare(13), false);
	    Assert.AreEqual(SquareMechanic.IsSquare(14), false);
	    Assert.AreEqual(SquareMechanic.IsSquare(15), false);
	    Assert.AreEqual(SquareMechanic.IsSquare(16), true);
        throw new System.Exception("testing stuff out");
        // TODO: jason look for todos in this file
#endif

        InitializeBoard();
    }

    void Update()
    {
        UpdateConnectedSpots();
        HandleDisconnects();
        ReplenishSpots();
        UpdateSpotPositions();
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

			// Generate new spots to fill in the remainder of the row.
            for (var y = newColumn.Count; y < GameConfiguration.Height; y++)
            {
                BoardModel.Spots[y][x] = InstantiateSpotAt(x, y);
		    }
		}
    }

    /// <summary>
    /// Instantiate Spot at board coordinates (x, y).
    /// </summary>
    SpotModel InstantiateSpotAt(int x, int y)
    {
        var spot = Instantiate<SpotModel>(GameConfiguration.SpotPrefab);
        spot.transform.position = BoardToWorldPosition(new Vector2(x, y));
        return spot;
    }

    void InitializeBoard()
    {
        for (var y = 0; y < GameConfiguration.Height; y++)
        {
            var row = new List<SpotModel>();

            for (var x = 0; x < GameConfiguration.Width; x++)
            {
                // var spot = Instantiate(GameConfiguration.SpotPrefab);
                // spot.transform.position = BoardToWorldPosition(new Vector2(x, y));
                var spot = InstantiateSpotAt(x, y);
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

                // Grab the board position of both last and current spots.
                var lastBoardPos = last.transform.position;
                var currentBoardPos = current.transform.position;

                // Compute whether the current spot is the same as any of the spots already in the list.
                var found = BoardModel.ConnectedSpots.Find(spot => spot == current);

                // If the spots are not cardinally adjacent, early return.
                if (!IsCardinallyAdjacent(lastBoardPos, currentBoardPos))
                {
                    return;
                }

                // If the colors don't match, early return.
                if (last.Color != current.Color)
                    return;

                // If the current spot is the same as any of the spots in the list,
                if (found)
                { 
                    // TODO: Then check whether the cycle of connected spots is a square.
                    // IsSquare(...)

                    // TODO: Save the square info for later processing.
                    // ...

                    // And early return.
                    return;
				}

                // Else, add the spot to the list of connected spots.
                BoardModel.ConnectedSpots.Add(current);
            }
        }
    }

    /// <summary>
    /// Check whether a list of spots contains a subsequence of spots in a square arrangement.
    /// </summary>
    bool IsSquare(List<SpotModel> spots)
    {
        // TODO: number of spots is a square number

        // TODO: x directions make sense

        // TODO: y directions make sense

        return false;
    }

    void HandleDisconnects()
    {
        if (!Input.GetMouseButtonUp(0))
            return;

		// Grab a copy of the list of connected dots.
        var connectedSpots = BoardModel.ConnectedSpots;
		var spots = new List<SpotModel>(connectedSpots);

        /*
        // TODO: Check whether there's a subsequence within connectedSpots that
        // satisfies the "Square Mechanic".
        var hasSquare = HasSquare(connectedSpots);
        Debug.Log("has square: " + hasSquare);
        */

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

    /// <summary>
    /// Update each spot's position based on its current board position. 
    /// </summary>
    void UpdateSpotPositions()
    { 
        for (var y = 0; y < GameConfiguration.Height; y++)
        { 
            for (var x = 0; x < GameConfiguration.Width; x++)
            {
                var spot = BoardModel.Spots[y][x];
                if (spot != null)
                { 
					spot.transform.position = BoardToWorldPosition(new Vector2(x, y));
				}
		    }
		}
    }
}
