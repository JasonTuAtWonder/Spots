using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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
        var isSquare = UpdateConnectedSpots();
        HandleDisconnects(isSquare);
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
        spot.transform.position = Convert.BoardToWorldPosition(new Vector2(x, y));
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


    /// <summary>
    /// Add to the BoardModel's ConnectedSpots given the current frame's state.
    /// </summary>
    /// <returns>Whether a square was detected.</returns>
    bool UpdateConnectedSpots()
    {
        if (!Input.GetMouseButton(0))
            return false;

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
                // var found = BoardModel.ConnectedSpots.Find(spot => spot == current);
                var foundIndex = BoardModel.ConnectedSpots.FindIndex(spot => spot == current);

                // If the spots are not cardinally adjacent, early return.
                if (!IsCardinallyAdjacent(lastBoardPos, currentBoardPos))
                {
                    return false;
                }

                // If the colors don't match, early return.
                if (last.Color != current.Color)
                    return false;

                // If the current spot is the same as any of the spots in the list,
                if (foundIndex == 0)
                { 
                    var isSquare = IsSquare(BoardModel.ConnectedSpots);
                    if (isSquare)
                    {
                        return true;
				    }
				}
                else if (foundIndex > -1)
                {
                    // Then early return.
                    return false;
				}

                // Else, add the spot to the list of connected spots.
                BoardModel.ConnectedSpots.Add(current);
            }
        }

        return false;
    }

    /// <summary>
    /// Check whether a line of spots follows a certain direction,
    /// with each spot 1 board unit after the other.
    ///
    /// For example, a line (with numbers indicating order) such as:
    ///
    ///     1 2 3
    ///
    /// Would satisfy the Direction.RIGHT criteria.
    ///
    /// Whereas a line such as:
    ///
    ///     3
    ///     2
    ///     1
    ///
    /// Would satisfy the Direction.UP criteria.
    /// </summary>
    private static bool LineFollowsDirection(List<SpotModel> lineOfSpots, Direction direction)
    {
        if (direction == Direction.INVALID)
            return false;

	    for (var i = 0; i < lineOfSpots.Count - 1; i++)
        {
            var current = lineOfSpots[i].BoardPosition;
            var next = lineOfSpots[i + 1].BoardPosition;

            if (direction == Direction.UP)
            { 
                if (next.y != current.y + 1)
                    return false;
		    }
            else if (direction == Direction.DOWN)
            {
                if (next.y != current.y - 1)
                    return false;
		    }
            else if (direction == Direction.LEFT)
            {
                if (next.x != current.x - 1)
                    return false;
		    }
            else if (direction == Direction.RIGHT)
            {
                if (next.x != current.x + 1)
                    return false;
		    }
            else
            {
                throw new System.Exception($"Unsupported direction: {direction}");
		    }
		}

        return true;
    }

    /// <summary>
    /// Get the direction of spot2.BoardPosition - spot1.BoardPosition.
    ///
    /// If the 2 spots are not adjacent, then this method will return Direction.INVALID.
    /// </summary>
    private static Direction GetDirection(SpotModel spot1, SpotModel spot2)
    {
        var boardPos1 = spot1.BoardPosition;
        var boardPos2 = spot2.BoardPosition;

        var xDiff = Mathf.Abs(boardPos1.x - boardPos2.x);
        var yDiff = Mathf.Abs(boardPos1.y - boardPos2.y);

        if (xDiff + yDiff != 1)
            return Direction.INVALID;

        if (SquareMechanic.Sign(boardPos2.x - boardPos1.x) == Sign.ZERO)
        {
            // Then this is a vertical direction.
            var ySign = SquareMechanic.Sign(boardPos2.y - boardPos1.y);
            if (ySign == Sign.POSITIVE)
                return Direction.UP;
            else if (ySign == Sign.NEGATIVE)
                return Direction.DOWN;
		}
        else
        {
            // Then this is a horizontal direction.
            var xSign = SquareMechanic.Sign(boardPos2.x - boardPos1.x);
            if (xSign == Sign.POSITIVE)
                return Direction.RIGHT;
            else if (xSign == Sign.NEGATIVE)
                return Direction.LEFT;
		}

        return Direction.INVALID;
    }

    private static Direction Flip(Direction dir)
    {
        if (dir == Direction.UP)
        {
            return Direction.DOWN;
		}
        else if (dir == Direction.DOWN)
        {
            return Direction.UP;
		}
        else if (dir == Direction.LEFT)
        {
            return Direction.RIGHT;
		}
        else if (dir == Direction.RIGHT)
        {
            return Direction.LEFT;
		}
        else if (dir == Direction.INVALID)
        {
            return Direction.INVALID;
		}
        else
        {
            throw new System.Exception($"Unsupported direction: {dir}");
		}
    }

    /// <summary>
    /// Check whether a list of spots contains a subsequence of spots in a square arrangement.
    /// </summary>
    public static bool IsSquare(List<SpotModel> spots)
    {
        if (!SquareMechanic.IsSquare(spots.Count))
        {
            return false;
		}

        // Compute the length of a square side.
        var squareLen = spots.Count / 4 + 1;

        // Ensure the direction of the first line segment is correct.
        var firstLineDirection = GetDirection(spots[0], spots[1]);
        var ok = LineFollowsDirection(spots.Take(squareLen).ToList(), firstLineDirection);
        if (!ok)
        {
            return false;
		}

        // Ensure the direction of the third line segment is correct.
        var thirdLineDirection = Flip(firstLineDirection);
        ok = LineFollowsDirection(spots.Skip(squareLen * 2 - 2).Take(squareLen).ToList(), thirdLineDirection);
        if (!ok)
        {
            return false;
		}

        // Ensure the direction of the second line segment is correct.
        var secondLineDirection = GetDirection(spots[squareLen - 1], spots[squareLen]);
        ok = LineFollowsDirection(spots.Skip(squareLen - 1).Take(squareLen).ToList(), secondLineDirection);
        if (!ok)
        {
            return false;
		}

        // Ensure the direction of the fourth line segment is correct.
        var fourthLineDirection = Flip(secondLineDirection);
        ok = LineFollowsDirection(spots.Skip(squareLen * 3 - 3).Take(squareLen).ToList(), fourthLineDirection);
        if (!ok)
        {
            return false;
		}

        return true;
    }

    void HandleDisconnects(bool isSquare)
    {
        if (!Input.GetMouseButtonUp(0) && !isSquare)
            return;

		// Grab a copy of the list of connected dots.
        var connectedSpots = BoardModel.ConnectedSpots;
		var spotsToDestroy = new List<SpotModel>(connectedSpots);

		// Then clear the connected dots from the board.
		connectedSpots.Clear();

        if (isSquare)
        {
            // Then also add all spots of the same color as the connected square dots:

            // Find the color of the connected square dots.
            var colorMatch = spotsToDestroy[0].Color;

            // And add all dots of the same color to our spotsToDestroy.
            for (var y = 0; y < GameConfiguration.Height; y++)
            { 
                for (var x = 0; x < GameConfiguration.Width; x++)
                {
                    var spot = BoardModel.Spots[y][x];
                    if (spot.Color == colorMatch)
                    {
                        spotsToDestroy.Add(spot);
				    }
				}
		    }

            // Then, because there might be dupes in spotsToDestroy, we de-dupe.
            spotsToDestroy = spotsToDestroy.Distinct().ToList();
		}

        // Perform a "disconnect" only if there are 2 or more connected spots.
        if (spotsToDestroy.Count >= 2)
        { 
			foreach (var spot in spotsToDestroy)
			{
			    // Find the spot's position on the board.
			    var boardPos = Convert.WorldToBoardPosition(spot.transform.position);
                var x = boardPos.x;
                var y = boardPos.y;

			    // Clear that spot from the grid (leaving a `null` in its place).
			    var toDestroy = BoardModel.Spots[y][x].gameObject;
			    Destroy(toDestroy);
			    BoardModel.Spots[y][x] = null;
			}
		}
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
					spot.transform.position = Convert.BoardToWorldPosition(new Vector2(x, y));
				}
		    }
		}
    }
}
