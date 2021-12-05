using UnityEngine;
using System.Collections.Generic;

public class BoardModel : MonoBehaviour
{
    public GameConfiguration GameConfiguration;

    List<List<SpotModel>> spots;

    void Awake()
    {
        InitializeBoard();
    }

    /// <summary>
    /// Instantiate spot prefabs for the board.
    /// </summary>
    void InitializeBoard()
    {
        spots = new List<List<SpotModel>>();

        for (var i = 0; i < GameConfiguration.Width; i++)
        {
            var row = new List<SpotModel>();

            for (var j = 0; j < GameConfiguration.Height; j++)
            {
                var spot = Instantiate(GameConfiguration.SpotPrefab);
                row.Add(spot);
            }

            spots.Add(row);
        }
    }
}
