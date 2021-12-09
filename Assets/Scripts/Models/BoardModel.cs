using UnityEngine;
using System.Collections.Generic;

[DefaultExecutionOrder((int)ExecutionOrder.BoardModel)]
public class BoardModel : MonoBehaviour
{
    // +x points to the right
    // +y points up
    public List<List<SpotModel>> Spots
    {
        get;
        private set;
    }

    public List<SpotModel> ConnectedSpots
    {
        get;
        private set;
    }

    void Awake()
    {
        Spots = new List<List<SpotModel>>();
        ConnectedSpots = new List<SpotModel>();
    }
}
