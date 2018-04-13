#region Usings

using UnityEngine;

#endregion

public class LineDrawingScript : MonoBehaviour {

    #region Private Properties

    private LineDrawingObject marker;
    private GameObject controller;
    private int deactivatedController;

    private bool toActivate = false;
    private bool toDestroy = false;

    public OVRInput.Controller deviceController;

    #endregion

    #region Methods

    public void SetMarker(LineDrawingObject newLineDrawer, GameObject controller)
    {
        marker = newLineDrawer;
        this.controller = controller;
        TurnOn();
    }

    void Update()
    {
        if (toActivate)
        {
            toActivate = false;
        }
        if (toDestroy)
        {
            Destroy(gameObject);
        }
    }

    public void TurnOn()
    {

            controller.transform.parent.GetComponent<ControllerTouchpadScript>().GripDown += BackButtonClicked;

            
        if (GameObject.Find("Toolbar") != null)
        {
            GameObject.Find("Toolbar").transform.Find("buttons").Find("BackButton").GetComponent<BackButtonScript>().ButtonDown += BackButtonClicked;
        }
        

        GetComponent<LinesGL>().SetDrawLinesOn(transform.parent.Find("ControlObject").gameObject, marker);
        GetComponent<LinesGL>().SetLineColor(marker.color);
    }

    public void TurnOff()
    {
            controller.transform.parent.GetComponent<ControllerTouchpadScript>().GripDown -= BackButtonClicked;

        toActivate = true;
        GetComponent<LinesGL>().SetDrawLinesOff(transform.parent.Find("ControlObject").gameObject);

        if (GameObject.Find("Toolbar") != null)
        {
            GameObject.Find("Toolbar").transform.Find("buttons").Find("BackButton").GetComponent<BackButtonScript>().ButtonDown -= BackButtonClicked;
        }
            
    }

    public void BackButtonClicked()
    {
        if (!GetComponent<LinesGL>().drawing)
        {
            TurnOff();
            toDestroy = true;
        }
    }




    #endregion
}
