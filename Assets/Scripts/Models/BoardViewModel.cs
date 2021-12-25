using System.Collections.Generic;
using UnityEngine;

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
    /// Whether a square is being connected by the player.
    /// </summary>
    public bool IsClosedSquare
    {
        get;
        set;
    }

    /// <summary>
    /// Whether a square was being connected by the player the previous frame.
    /// </summary>
    public bool WasClosedSquare
    {
        get;
        private set;
    }

    /// <summary>
    /// Whether a square was newly detected this frame.
    /// </summary>
    public bool DetectedClosedSquare
    {
        get
        {
            return IsClosedSquare && !WasClosedSquare;
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
        WasClosedSquare = IsClosedSquare;
    }
}
