using UnityEngine;

public static class InputHandling
{
#if false
    public static Touch? GetFirstTouch()
    {
        if (Input.touchCount == 0)
            return null;

        return Input.GetTouch(0);
    }

    /// <summary>
    /// Returns a Touch if the user pressed down on the screen.
    /// Otherwise, returns null.
    /// </summary>
    public static Touch? DidTouchDown()
    {
        if (Input.touchCount == 0)
            return null;

        var firstTouch = Input.GetTouch(0);
        if (firstTouch.phase != TouchPhase.Began)
            return null;

        return firstTouch;
    }

    /// <summary>
    /// Returns a Touch if the user pressed up on the screen.
    /// Otherwise, returns null.
    /// </summary>
    public static Touch? DidTouchUp()
    {
        if (Input.touchCount == 0)
            return null;

        var firstTouch = Input.GetTouch(0);
        if (firstTouch.phase == TouchPhase.Canceled || firstTouch.phase == TouchPhase.Ended)
            return firstTouch;

        return null;
    }

    /// <summary>
    /// Returns a Touch if the user is currently pressing the screen.
    /// Otherwise, returns null.
    /// </summary>
    public static Touch? DidTouch()
    {
        if (Input.touchCount == 0)
            return null;

        var firstTouch = Input.GetTouch(0);
        if (firstTouch.phase == TouchPhase.Moved || firstTouch.phase == TouchPhase.Stationary)
            return firstTouch;

        return null;
    }
#endif
}
