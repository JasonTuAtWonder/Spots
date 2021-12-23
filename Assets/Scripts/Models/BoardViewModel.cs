using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// BoardViewModel holds the data model for the game board.
///
/// Because this class holds references to SpotPresenter objects,
/// it is named "ViewModel" to indicate that it holds view-level data in addition to model-level data.
/// </summary>
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

    void Awake()
    {
        ConnectedSpots = new List<SpotPresenter>();
        Spots = new List<List<SpotPresenter>>();
    }
}
