using UnityEngine;

[RequireComponent(typeof(SpotModel))]
public class SpotPresenter : MonoBehaviour
{
    [Header("Models")]
    SpotModel spotModel;

    [Header("Views")]
    [NotNull] public MeshRenderer SpotView;

    Material spotMaterial;

    void Awake()
    {
        spotModel = GetComponent<SpotModel>();
        spotMaterial = SpotView.material;

        Render();
    }

    /// <summary>
    /// Bind the SpotModel's data to the SpotView.
    /// </summary>
    void Render()
    {
        spotMaterial.color = spotModel.Color;
    }
}
