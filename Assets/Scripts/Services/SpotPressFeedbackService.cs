using UnityEngine;

/// <summary>
/// SpotPressFeedbackService instantiates visual feedback for spot presses.
/// </summary>
public class SpotPressFeedbackService : MonoBehaviour
{
    [Header("Dependencies")]
    [NotNull] public GameConfiguration GameConfiguration;
    [NotNull] public SpotPressFeedback SpotPressFeedback;

    public SpotPressFeedback InstantiateFeedback(Vector3 position, Color color)
    {
        var obj = Instantiate(SpotPressFeedback, position, Quaternion.identity);
        obj.SetColor(color);
        return obj;
    }

    public void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.A))
        {
            var x = Random.Range(-25f, 25f);
            var y = Random.Range(-25f, 25f);
            InstantiateFeedback(new Vector3(x, y, 5f), GameConfiguration.RandomColor());
		}
#endif
    }
}
