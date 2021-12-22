using UnityEngine;

/// <summary>
/// ExecutionOrder defines the execution order of the Awake() callbacks within this project.
///
/// To ensure this execution order kicks in, apply a [DefaultExecutionOrder()] attribute
/// to the top of the respective MonoBehaviour.
///
/// For example, [DefaultExecutionOrder(ExecutionOrder.BoardModel)] for the BoardModel MonoBehaviour.
/// </summary>
public enum ExecutionOrder
{
    SpotModel,
    SpotPresenter,
    BoardModel,
    BoardPresenter,
    LineBarProgressPresenter, // Depends on the value of BoardPresenter's "IsSquare".
}
