using UnityEngine;
using System.Collections;

public class SpotPressPresenter : MonoBehaviour
{
    [Header("Views")]
    [NotNull] public MeshRenderer Renderer;
    [NotNull] public GameObject Scaler;

    Material mat;

    void Awake()
    {
        mat = Renderer.material;
    }

    void OnDestroy()
    {
        Destroy(mat);
    }

    void Start()
    {
        // Kick off coroutine to animate the scale.
        StartCoroutine(AnimateScaleAndTransparency(.5f));
    }

    IEnumerator AnimateScaleAndTransparency(float duration)
    {
        var from = Vector3.zero;
        var to = Vector3.one * 2f;
        var journey = 0f;

        while (journey <= duration)
        {
            // Update elapsed time.
            journey += Time.deltaTime;

            // Update fields using elapsed time.
            float t = Mathf.Clamp01(journey / duration);
            if (this != null)
            { 
                // Update scale.
			    transform.localScale = Vector3.Lerp(from, to, t);

                // Update transparency.
                SetTransparency(1f - t);
		    }

            // Complete one step of coroutine execution.
            yield return null;
        }

        // Once animation is complete, destroy this game object.
        Destroy(gameObject);
    }

    public void SetColor(Color color)
    {
        mat.SetColor("_DesiredColor", color);
    }

    public void SetTransparency(float alpha)
    {
        mat.SetFloat("_Transparency", alpha);
    }
}
