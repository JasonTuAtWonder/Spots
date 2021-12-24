using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// BoardViewModel holds the data model for the game board.
///
/// Because this class holds references to SpotPresenter objects,
/// it is named "ViewModel" to indicate that it holds view-level data in addition to model-level data.
/// </summary>
[DefaultExecutionOrder((int)ExecutionOrder.BoardViewModel)]
public class BoardViewModel : MonoBehaviour
{
    /// <summary>
    /// The game board's 2D grid of spots.
    ///
    /// +x points to the right, and +y points up.
    /// </summary>
    public List<List<SpotPresenter>> Spots
    {
        get;
        private set;
    }

    /// <summary>
    /// The player's currently-connected list of spots.
    /// </summary>
    public List<SpotPresenter> ConnectedSpots
    {
        get;
        private set;
    }

    /// <summary>
    /// Whether a square (or rectangle) is being connected by the player.
    /// </summary>
    public bool IsClosedRectangle
    {
        get;
        set;
    }

    /// <summary>
    /// Whether a square (or rectangle) was being connected by the player the previous frame.
    /// </summary>
    public bool WasClosedRectangle
    {
        get;
        private set;
    }

    /// <summary>
    /// Whether a square (or rectangle) was newly detected this frame.
    /// </summary>
    public bool IsClosedRectangleDetected
    { 
        get
        {
            return IsClosedRectangle && !WasClosedRectangle;
		}
    }

    /// <summary>
    /// Note: OnEnable() is used instead of Awake() so the callback is called after domain reload.
    /// </summary>
    void OnEnable()
    {
        ConnectedSpots = new List<SpotPresenter>();
        Spots = new List<List<SpotPresenter>>();
    }

    void LateUpdate()
    {
        WasClosedRectangle = IsClosedRectangle;
    }
}
