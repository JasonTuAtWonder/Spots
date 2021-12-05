using UnityEngine;

[DefaultExecutionOrder((int)ExecutionOrder.BoardPresenter)]
public class BoardPresenter : MonoBehaviour
{
    [Header("Models")]
    [NotNull] public BoardModel BoardModel;

    void Awake()
    {
        PerformLayout();
    }

    void PerformLayout()
    {
        var spots = BoardModel.Spots;

        for (var y = 0; y < spots.Count; y++)
        {
            var row = spots[y];

            for (var x = 0; x < row.Count; x++)
            {
                var spot = row[x];
                spot.transform.position = new Vector3(x, y, 0);
            }
        }
    }
}
