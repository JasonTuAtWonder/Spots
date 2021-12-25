using UnityEngine;

/// <summary>
/// CallAttentionToSpotService instantiates visual feedback for calling attention to individual spots.
/// </summary>
public class CallAttentionToSpotService : MonoBehaviour
{
    [Header("Dependencies")]
    [NotNull] public GameConfiguration GameConfiguration;
    [NotNull] public CallAttentionToSpotPresenter CallAttentionToSpotPrefab;

    public CallAttentionToSpotPresenter MakeFeedback(Vector3 position, Color color)
    {
        var obj = Instantiate(CallAttentionToSpotPrefab, position, Quaternion.identity);
        obj.SetColor(color);
        return obj;
    }

    public void Update()
    {
#if UNITY_EDITOR
        // For testing when you are running the game in-editor.
        if (Input.GetKeyDown(KeyCode.A))
        {
            var x = Random.Range(-25f, 25f);
            var y = Random.Range(-25f, 25f);
            MakeFeedback(new Vector3(x, y, 5f), GameConfiguration.RandomColor());
        }
#endif
    }
}
