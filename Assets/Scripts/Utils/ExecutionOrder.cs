/// <summary>
/// ExecutionOrder defines the execution order of scripts.
///
/// You can read the enum from top to bottom, and rest assured the callbacks
/// within each of the corresponding classes will execute in that order.
///
/// However, to ensure the execution order kicks in, apply a [DefaultExecutionOrder()] attribute
/// to the top of the respective MonoBehaviour.
///
/// For example, type out [DefaultExecutionOrder(ExecutionOrder.BoardModel)] above the BoardModel class.
/// </summary>
public enum ExecutionOrder
{
    BoardPresenter,
    ProgressBarFeedback, // Depends on the value of BoardModel's .IsClosedRectangle, which is updated by BoardPresenter.
}
