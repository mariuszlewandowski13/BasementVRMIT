using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class TeacherPointer : MonoBehaviour {

    private LineRenderer pointerRenderer;

    private RaycastHit hit;

    private bool active;

    private List<string>  boardTacceptedTags = new List<string> { "board" ,"board2", "keyboardBtn" };

    public OVRInput.Controller controller;


    private Transform lastClickedObject;


    private void Start()
    {
        pointerRenderer = GetComponent<LineRenderer>();
    }

    void Update () {
        Ray ray =  new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out hit);

        if (hit.transform != null)
        {
            if ((ApplicationStaticData.IsSuperUser() && hit.transform.tag == boardTacceptedTags[0]) || hit.transform.tag == boardTacceptedTags[1])
            {
                HandleBoardPointerClick(hit.transform);
                PointerOn();
            }
            else if((hit.transform.tag == boardTacceptedTags[2]) && hit.transform.GetComponent<IClickable>() != null ){
                PointerOn();
                HandleClickDown(hit.transform);
            } 
        }
        else if (active)
        {
            PointerOff();
        }

        HandleMinimizing();

        HandleClickUp();

        HandleKeyboardOnOff();



    }

    private void HandleKeyboardOnOff()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick, controller) && (ApplicationStaticData.IsSuperUser() || ApplicationStaticData.roomToConnectName == "session11"))
        {
            KeyboardScript.instance.gameObject.SetActive(!KeyboardScript.instance.gameObject.activeSelf);
        }
    }

    private void HandleClickUp()
    {
         if (lastClickedObject != null && OVRInput.GetUp(OVRInput.Button.One, controller))
        {
            lastClickedObject.GetComponent<IClickable>().ClickUp(gameObject);
            lastClickedObject = null;
        }
    }

    private void HandleClickDown(Transform tr)
    {
        if (OVRInput.GetDown(OVRInput.Button.One, controller))
        {
            tr.GetComponent<IClickable>().ClickDown(gameObject);
            lastClickedObject = tr;
        }
    }

    private void HandleMinimizing()
    {
        if (ApplicationStaticData.IsSuperUser() &&  OVRInput.GetUp(OVRInput.Button.Two, controller))
        {
            OnMinimizeButtonClick();
        }
    }

    void PointerOn()
    {
        Vector3[] points = new Vector3[] { transform.position, hit.point };
        pointerRenderer.positionCount = 2;
        pointerRenderer.SetPositions(points);
        active = true;
    }

    void PointerOff()
    {
        pointerRenderer.positionCount = 0;
        active = false;
    }

    void HandleBoardPointerClick(Transform tr)
    {
     if (OVRInput.GetDown(OVRInput.Button.One, controller) || OVRInput.GetUp(OVRInput.Button.One, controller))
        {

            if (OVRInput.GetDown(OVRInput.Button.One, controller))
            {
                int x = 0;
                int y = 0;
                CalculateMousePosition(out x, out y);
                tr.GetComponent<VideoStreaming>().HandleMouseClick(x, y);

                tr.GetComponent<VideoStreaming>().MouseClickDown();
            } else if (OVRInput.GetUp(OVRInput.Button.One, controller))
            {
                tr.GetComponent<VideoStreaming>().MouseClickUp();
            }
        }
    }

    void CalculateMousePosition(out int x, out int y)
    {
      //  Debug.Log(Screen.currentResolution);
        x = (int)((1-hit.textureCoord.x) * Screen.currentResolution.width);
        y = (int)(hit.textureCoord.y * Screen.currentResolution.height);
    }



    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    public void OnMinimizeButtonClick()
    {
        ShowWindow(GetActiveWindow(), 2);
    }
}
