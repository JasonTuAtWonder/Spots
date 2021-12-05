using UnityEngine;
using System.Collections.Generic;

[DefaultExecutionOrder((int)ExecutionOrder.BoardModel)]
public class BoardModel : MonoBehaviour
{
    public GameConfiguration GameConfiguration;

    public List<List<SpotModel>> Spots
    {
        get;
        private set;
    }

    void Awake()
    {
        InitializeBoard();
    }

    /// <summary>
    /// Instantiate spot prefabs for the board.
    /// </summary>
    void InitializeBoard()
    {
        Spots = new List<List<SpotModel>>();

        for (var i = 0; i < GameConfiguration.Width; i++)
        {
            var row = new List<SpotModel>();

            for (var j = 0; j < GameConfiguration.Height; j++)
            {
                var spot = Instantiate(GameConfiguration.SpotPrefab);
                row.Add(spot);
            }

            Spots.Add(row);
        }
    }
}
