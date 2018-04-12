#region Usings

using UnityEngine;

#endregion

public class ControllerTouchpadScript : MonoBehaviour
{
    #region Public Events & Delegates

    public delegate void InteractionTouchpad(Vector2 axis, bool first);
    public static event InteractionTouchpad TouchpadSwipe;
    public event InteractionTouchpad TouchpadSwipeOnControllerAction;

    public delegate void TouchpadButtonInteraction();
    public event TouchpadButtonInteraction GripDown;

    public OVRInput.Controller controller;

    #endregion


    #region Methods


    void Update()
    {
        Vector2 axis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, controller);
        bool touch = (axis != new Vector2());
        //Debug.Log("Touch " + touch.ToString());
        bool touchpadDown = OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, controller);

        if (TouchpadSwipe != null && touch)
        {
            TouchpadSwipe(axis, touch);
        }

        if (TouchpadSwipeOnControllerAction != null && touch)
        {
            TouchpadSwipeOnControllerAction(axis, touch);
        }

        if (touchpadDown && GripDown != null)
        {
            GripDown();
        }
    }

    #endregion
}