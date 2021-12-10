using UnityEngine;

[DefaultExecutionOrder((int)ExecutionOrder.SpotPresenter)]
public class SpotPresenter : MonoBehaviour
{
    [Header("Models")]
    [NotNull] public SpotModel SpotModel;

    [Header("Views")]
    [NotNull] public MeshRenderer SpotView;

    Material spotMaterial;

    void Awake()
    {
        spotMaterial = SpotView.material;
        UpdateColor();
    }

    /// <summary>
    /// Bind the SpotModel's data to the SpotView.
    /// </summary>
    void UpdateColor()
    {
        spotMaterial.color = SpotModel.Color;
    }
}
