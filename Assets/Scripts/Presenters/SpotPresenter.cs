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
        Render();
    }

    /// <summary>
    /// Bind the SpotModel's data to the SpotView.
    /// </summary>
    void Render()
    {
        spotMaterial.color = SpotModel.Color;
    }
}
