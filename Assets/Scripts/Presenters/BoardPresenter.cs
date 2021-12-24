using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System;

/// <summary>
/// BoardPresenter presents the board to the player.
///
/// It is where most of the game logic runs:
///
/// * handling the player's input,
/// * updating the underlying data model, and
/// * providing audiovisual feedback to the player about data changes.
/// </summary>
[DefaultExecutionOrder((int)ExecutionOrder.BoardPresenter)]
public class BoardPresenter : MonoBehaviour
{
    [Header("Models")]
    [NotNull] public GameConfiguration GameConfiguration;
    [NotNull] public BoardViewModel BoardModel;

    [Header("Views")]
    [NotNull] public Camera Camera;

    [Header("Services")]
    [NotNull] public AudioService AudioService;
    [NotNull] public SpotPressFeedbackService SpotPressFeedbackService;
    [NotNull] public SpotPrefabService SpotPrefabService;

    /// <summary>
    /// Note: OnEnable() is used instead of Awake() so the callback is called after domain reload.
    /// </summary>
    void OnEnable()
    {
        InitializeBoard();
        StartCoroutine(DropSpotsFromAbove());
    }

    void Update()
    {
        BoardModel.IsClosedRectangle = OnSpotMove(UpdateConnectedSpots);

        if (BoardModel.IsClosedRectangleDetected)
        { 
            // Play some audio feedback.
			AudioService.PlayOneShot(AudioService.Chime);

            // For all the circles of the same color, play some visual feedback to indicate they will be cleared.
            var color = BoardModel.ConnectedSpots[0].SpotModel.Color;
            HighlightAllSpotsWith(color);
		}

        HandleDisconnects(BoardModel.IsClosedRectangle);

        if (Input.GetMouseButtonUp(0))
        { 
			ReplenishSpots();
		}
    }

    /// <summary>
    /// Highlight all spots with a given `color`.
    /// </summary>
    void HighlightAllSpotsWith(Color color)
    { 
        for (var y = 0; y < GameConfiguration.Height; y++)
        { 
            for (var x = 0; x < GameConfiguration.Width; x++)
            {
                var spot = BoardModel.Spots[y][x];
                var spotColor = spot.SpotModel.Color;

                if (spotColor.Equals(color))
                {
                    var pos = spot.transform.position;
                    SpotPressFeedbackService.MakeFeedback(pos, spotColor);
				}
		    }
		}
    }

    /// <summary>
    /// Kicks off animation for dropping spots from the top of the screen.
    /// </summary>
    IEnumerator DropSpotsFromAbove()
    { 
        for (var y = 0; y < GameConfiguration.Height; y++)
        { 
            for (var x = 0; x < GameConfiguration.Width; x++)
            {
                var spotPresenter = BoardModel.Spots[y][x];
                spotPresenter.FallAndBounce(.5f);
		    }

		    yield return new WaitForSeconds(.05f);
		}
    }

    /// <summary>
    /// Update the board so that empty spots are filled by the spots above.
    /// </summary>
    void ReplenishSpots()
    {
        var numNewSpots = 0;

        // For each column,
        for (var x = 0; x < GameConfiguration.Width; x++)
        { 
			// Create a new column.
			List<SpotPresenter> newColumn = new List<SpotPresenter>();

            // Fill the new column with the spots that aren't null.
            for (var y = 0; y < GameConfiguration.Height; y++)
            {
                var row = BoardModel.Spots[y];
                if (row == null)
                    continue;

                var spot = row[x];
                if (spot == null)
                    continue;

			    newColumn.Add(spot);
		    }

            // Update the grid with the new column.
            // At this point, some (or all) spots from the top of the column may be null.
            for (var y = 0; y < newColumn.Count; y++)
            { 
                BoardModel.Spots[y][x] = newColumn[y];
                newColumn[y].SpotModel.BoardPosition = new Vector2Int(x, y);
		    }

			// Generate new spots to fill in the remainder of the column.
            for (var y = newColumn.Count; y < GameConfiguration.Height; y++)
            {
                var row = BoardModel.Spots[y];
                row[x] = SpotPrefabService.MakeSpot(x, y);
                numNewSpots++;
		    }
		}

        if (numNewSpots > 0)
        {
            StartCoroutine(DropSpotsFromAbove());
		}
    }

    /// <summary>
    /// Initializes the BoardViewModel's game board.
    /// </summary>
    void InitializeBoard()
    {
        for (var y = 0; y < GameConfiguration.Height; y++)
        {
            var row = new List<SpotPresenter>();

            for (var x = 0; x < GameConfiguration.Width; x++)
            {
                var spot = SpotPrefabService.MakeSpot(x, y);
                row.Add(spot);
            }

            BoardModel.Spots.Add(row);
        }
    }

    /// <summary>
    /// Handle the event when the player moves over a spot.
    /// </summary>
    /// <returns>Whether the player formed a closed rectangle of spots.</returns>
    bool UpdateConnectedSpots(SpotPresenter newSpot)
    {
        // If there are no other connected spots,
        if (BoardModel.ConnectedSpots.Count == 0)
        {
            // Connect the spot as the first spot, and early return.
            ConnectSpot(newSpot);
            return false;
        }

        // Grab the last connected spot.
        var lastIndex = BoardModel.ConnectedSpots.Count - 1;
	    var lastSpot = BoardModel.ConnectedSpots[lastIndex];

	    // If the colors don't match, early return.
	    if (!lastSpot.SpotModel.Color.Equals(newSpot.SpotModel.Color))
			return false;

	    // If the spots are not cardinally adjacent, early return.
	    if (!SquareMechanic.IsCardinallyAdjacent(lastSpot, newSpot))
			return false;

        // If the spot isn't already connected, connect the spot and early return.
	    var foundIndex = BoardModel.ConnectedSpots.FindIndex(spot => spot == newSpot);
        if (foundIndex == -1)
        {
            ConnectSpot(newSpot);
            return false;
		}

        // If the spot is the second-to-last-connected spot,
        if (foundIndex > -1 && foundIndex == lastIndex - 1)
        { 
            // Remove the last connected spot and early return.
		    RemoveLastConnectedSpot();
            return false;
		}

        // If a loop was detected,
        var isLoop = foundIndex == 0 && BoardModel.ConnectedSpots.Count >= 4;
        if (isLoop)
        { 
            // Then compute whether the loop is square. (Do this before updating the ConnectedSpots list.)
			var isSquare = SquareMechanic.IsSquare(BoardModel.ConnectedSpots);

            // Connect the spot to close the loop.
            ConnectSpot(newSpot);

            // And return whether the loop is square.
            return isSquare;
		}

        // Otherwise, there's a case that we haven't handled.
        // Throw an exception when in development, but otherwise return false.
#if UNITY_EDITOR
        // throw new System.Exception("Unhandled case."); // Keep this around if you'd like to debug!
        return false;
#else
		return false;
#endif
    }

    /// <summary>
    /// Run a specified event handler when the player mouses over a spot.
    /// </summary>
    /// <returns>Whether a closed rectangle was detected.</returns>
    bool OnSpotMove(Func<SpotPresenter, bool> handleSpotMove)
    {
        // If the player is not holding their left mouse button, early return.
        if (!Input.GetMouseButton(0))
            return false;

        // Otherwise, cast a ray through the scene.
        var mouseWorldPos = Input.mousePosition;
        Ray ray = Camera.ScreenPointToRay(mouseWorldPos);
        var hits = Physics.RaycastAll(ray);

        foreach (var hitInfo in hits)
        {
            // If the hit object is not a spot, skip this hit.
            var isSpot = hitInfo.collider.gameObject.CompareTag("Spot");
            if (!isSpot)
                continue;

            // Otherwise, the hit object is a spot. Handle the spot press event.
            var spotPresenter = hitInfo.collider.GetComponent<SpotPresenter>();
            var isClosedRectangle = handleSpotMove(spotPresenter);
            return isClosedRectangle;
		}

        // If no spots were pressed, then return false: a closed rectangle was not detected.
        return false;
    }

    /// <summary>
    /// Provide some feedback to the player when they connect a new spot.
    /// </summary>
    void PlayConnectSpotFeedback(SpotPresenter spotPresenter)
    { 
        // Play audio feedback.
        var audioClip = AudioService.Notes[BoardModel.ConnectedSpots.Count - 1];
        if (audioClip != null)
            AudioService.PlayOneShot(audioClip);
        else
            // Don't have sound, ah well – no-op.

	    // Play visual feedback.
        SpotPressFeedbackService.MakeFeedback(spotPresenter.transform.position, spotPresenter.SpotModel.Color);
    }

    /// <summary>
    /// Connect a new spot.
    /// </summary>
    void ConnectSpot(SpotPresenter spotPresenter)
    {
        // Update connected spots.
        BoardModel.ConnectedSpots.Add(spotPresenter);

        // And play some audiovisual feedback.
        PlayConnectSpotFeedback(spotPresenter);
    }

    /// <summary>
    /// Disconnect the last connected spot.
    ///
    /// This occurs when the player mouses over the connected spot *prior* to the last connected spot.
    /// </summary>
    void RemoveLastConnectedSpot()
    {
        var lastIndex = BoardModel.ConnectedSpots.Count - 1;
        var secondToLast = BoardModel.ConnectedSpots[lastIndex - 1];

        // Remove the last connected spot.
        BoardModel.ConnectedSpots.RemoveAt(lastIndex);

        // And play some audiovisual feedback.
        PlayConnectSpotFeedback(secondToLast);
    }

    /// <summary>
    /// Handle "disconnect" actions, or when the player lets go of the spots they connected.
    /// </summary>
    void HandleDisconnects(bool isSquareLoop)
    {
        // If player did not release the mouse button this frame, early return.
        if (!Input.GetMouseButtonUp(0))
            return;

		// Grab a copy of the list of connected dots.
        var connectedSpots = BoardModel.ConnectedSpots;
		var spotsToDestroy = new List<SpotPresenter>(connectedSpots);

		// Clear the connected dots from the board.
		connectedSpots.Clear();

        if (isSquareLoop)
        {
            // Then mark all spots of the same color (as the connected square) for destruction:

            // Find the color of the connected square dots.
            var squareColor = spotsToDestroy[0].SpotModel.Color;

            // Add all dots of the same color to our spotsToDestroy.
            for (var y = 0; y < GameConfiguration.Height; y++)
            { 
                for (var x = 0; x < GameConfiguration.Width; x++)
                {
                    var spot = BoardModel.Spots[y][x];
                    if (spot.SpotModel.Color.Equals(squareColor))
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
                var boardPos = spot.SpotModel.BoardPosition;
                var x = boardPos.x;
                var y = boardPos.y;

                // Clear that spot from the grid (leaving a `null` in its place).
                var spotPresenter = BoardModel.Spots[y][x];
			    spotPresenter.Disappear(.2f);
			    BoardModel.Spots[y][x] = null;
			}
		}
    }
}
