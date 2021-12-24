using UnityEngine;

/// <summary>
/// Helper class to persist animation curves in a ScriptableObject,
/// since Unity keeps blowing away my AnimationCurve data when it's a field on a MonoBehaviour.
/// </summary>
[CreateAssetMenu]
public class AnimationConfiguration : ScriptableObject
{
    public AnimationCurve AnimationCurve;
}
