using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// BoardViewModel holds the data model for the game board.
///
/// Because this class holds references to SpotPresenter objects,
/// it is named as a "ViewModel" to indicate that it holds view-level data in addition to model-level data.
/// </summary>
[DefaultExecutionOrder((int)ExecutionOrder.BoardModel)]
public class BoardViewModel : MonoBehaviour
{
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
    }
}
