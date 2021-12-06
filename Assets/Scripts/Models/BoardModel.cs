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

    public Vector2? CurrentMousePosition
    {
        get;
        set;
    }

    void Awake()
    {
        Spots = new List<List<SpotModel>>();
        ConnectedSpots = new List<SpotModel>();
    }

    void Update()
    {
        Debug.Log("connected spots: " + ConnectedSpots.Count);
        foreach (var spot in ConnectedSpots)
        {
            Debug.Log(spot.transform.position);
        }
    }
}
