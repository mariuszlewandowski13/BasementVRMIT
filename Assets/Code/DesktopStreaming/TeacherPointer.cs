using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.Drawing;

public enum MouseEventFlags
{
    LEFTDOWN = 0x00000002,
    LEFTUP = 0x00000004,
    MIDDLEDOWN = 0x00000020,
    MIDDLEUP = 0x00000040,
    MOVE = 0x00000001,
    ABSOLUTE = 0x00008000,
    RIGHTDOWN = 0x00000008,
    RIGHTUP = 0x00000010,
    WM_MOUSEWHEEL = 0x020A,
    WHEEL_DELTA = 120
}


public class TeacherPointer : MonoBehaviour {

    private LineRenderer pointerRenderer;

    private RaycastHit hit;

    private bool active;

    private List<string>  boardTacceptedTags = new List<string> { "board" ,"board2", "keyboardBtn" };

    public OVRInput.Controller controller;


    private Transform lastClickedObject;


    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, Int32 wParam, ref Point lParam);

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

    private void HandleThumbstickScroll()
    {

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
        if ( OVRInput.GetUp(OVRInput.Button.Two, controller))
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
                tr.GetComponent<VideoStreaming>().HandleMouseClick(hit.textureCoord.x, hit.textureCoord.y);
                tr.GetComponent<VideoStreaming>().MouseClickDown();
            } else if (OVRInput.GetUp(OVRInput.Button.One, controller))
            {
                tr.GetComponent<VideoStreaming>().MouseClickUp();
            }
        }
    }





   

    public void OnMinimizeButtonClick()
    {
        ShowWindow(GetActiveWindow(), 2);
    }


    private void ScrollWindow(IntPtr hwnd, Point p, int scrolls = -1)
    {
        SendMessage(hwnd, (int)MouseEventFlags.WM_MOUSEWHEEL, ((int)MouseEventFlags.WHEEL_DELTA * scrolls) << 16, ref p);
    }
}
