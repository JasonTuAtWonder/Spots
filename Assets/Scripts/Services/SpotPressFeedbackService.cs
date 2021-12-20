using UnityEngine;

public class SpotPressFeedbackService : MonoBehaviour
{
    [Header("Dependencies")]
    [NotNull] public GameConfiguration GameConfiguration;
    [NotNull] public SpotPressPresenter SpotPressFeedback;

    public SpotPressPresenter InstantiateFeedback(Vector3 position, Color color)
    {
        var obj = Instantiate(SpotPressFeedback, position, Quaternion.identity);
        obj.SetColor(color);
        return obj;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            var x = Random.Range(-25f, 25f);
            var y = Random.Range(-25f, 25f);
            InstantiateFeedback(new Vector3(x, y, 5f), GameConfiguration.RandomColor());
		}
    }
}
