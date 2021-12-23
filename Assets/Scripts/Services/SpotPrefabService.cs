using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// SpotPrefabService instantiates Spots for use in-game.
/// </summary>
public class SpotPrefabService : MonoBehaviour
{
    [Header("Prefabs")]
    [NotNull] public SpotModel SpotModel;
    [NotNull] public SpotPresenter SpotPresenter;

    void Awake()
    {
        Assert.AreEqual(SpotModel.gameObject, SpotPresenter.gameObject, "SpotModel and SpotPresenter should refer to the same prefab.");
    }

    /// <summary>
    /// Instantiate spot at board coordinates (x, y).
    /// </summary>
    public SpotPresenter MakeSpot(int x, int y)
    { 
        var spot = Instantiate(SpotPresenter);
        spot.SpotModel.BoardPosition = new Vector2Int(x, y);
        spot.SetWorldPosition();
        return spot;
    }
}
