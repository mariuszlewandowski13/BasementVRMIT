#region Usings

using UnityEngine;
using System.Collections;

#endregion

public class PositionBarScript : MonoBehaviour, IClickable {

    #region Private Properties

    private bool active = false;

    private GameObject toolbar;

    private float minYPosition = 0.2f;

    private float controllerYPreviousPosition;

    private Vector3 controllerPrevPosition;

    private bool moving;
    private GameObject ActualMovingController;

    #endregion




    #region Methods

    void Start()
    {
        toolbar = transform.parent.parent.gameObject;
    }


    private void Update()
    {
        OnControllerMove(ActualMovingController);
    }


    private void OnControllerMove(GameObject controller)
    {
        if (moving)
        {
            float controllerYPosition = controller.transform.position.y;


            float shiftY = controllerYPreviousPosition - controllerYPosition;


            float toolbarYPosition = toolbar.GetComponent<ToolbarMovementScript>().positionYOffset;

            if (toolbarYPosition - shiftY >= minYPosition) toolbar.GetComponent<ToolbarMovementScript>().positionYOffset -= shiftY;

            controllerYPreviousPosition = controllerYPosition;


            //rotation
            Vector3 controllerPos = controller.transform.position;

            Vector3 from = controllerPrevPosition - toolbar.transform.position;
            Vector3 to = controllerPos - toolbar.transform.position;



            from.y = 0.0f;
            to.y = 0.0f;



            float angle = Vector3.Angle(from, to);

            Vector3 cross = Vector3.Cross(from, to);
            if (cross.y < 0) angle = -angle;



            Vector3 toolbarRot = toolbar.transform.rotation.eulerAngles;
            toolbarRot.y += angle;
            toolbar.transform.rotation = Quaternion.Euler(toolbarRot);


            controllerPrevPosition = controllerPos;
        }
    }

    public void ClickDown(GameObject controller)
    {
        if (!moving)
        {
            controllerYPreviousPosition = controller.transform.position.y;
            controllerPrevPosition = controller.transform.position;

            ActualMovingController = controller;
            moving = true;
        }

    }

    public void ClickUp(GameObject controller)
    {
        if (moving)
        {
            moving = false;
            ActualMovingController = null;
        }
    }


    #endregion
}
