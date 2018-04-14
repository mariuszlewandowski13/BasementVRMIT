using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class TeacherPointer : MonoBehaviour {

    private LineRenderer pointerRenderer;

    private RaycastHit hit;

    private bool active;

    private string acceptedTag = "board";

    public OVRInput.Controller controller;

    [DllImport("user32")]
    public static extern int SetCursorPos(int x, int y);

    [DllImport("user32.dll")]
    static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);

    public enum MouseEventFlags
    {
        LEFTDOWN = 0x00000002,
        LEFTUP = 0x00000004,
        MIDDLEDOWN = 0x00000020,
        MIDDLEUP = 0x00000040,
        MOVE = 0x00000001,
        ABSOLUTE = 0x00008000,
        RIGHTDOWN = 0x00000008,
        RIGHTUP = 0x00000010
    }

    private void Start()
    {
        pointerRenderer = GetComponent<LineRenderer>();
    }

    void Update () {
        Ray ray =  new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out hit);

        if (hit.transform != null && hit.transform.tag == acceptedTag)
        {
            PointerOn();
            HandlePointerClick();
        }
        else if (active)
        {
            PointerOff();
        }
        HandleMinimizing();
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

    void HandlePointerClick()
    {
 if (OVRInput.GetDown(OVRInput.Button.One, controller) || OVRInput.GetUp(OVRInput.Button.One, controller))
        {
            int x = 0;
            int y = 0;
            CalculateMousePosition(out x, out y);
            SetCursorPos(x, y);

            if (OVRInput.GetDown(OVRInput.Button.One, controller))
            {
                MouseClickDown();
            } else if (OVRInput.GetUp(OVRInput.Button.One, controller))
            {
                MouseClickUp();
            }
        }
    }

    void CalculateMousePosition(out int x, out int y)
    {
      //  Debug.Log(Screen.currentResolution);
        x = (int)(hit.textureCoord.x * Screen.currentResolution.width);
        y = (int)(hit.textureCoord.y * Screen.currentResolution.height);
    }

    void MouseClickDown()
    {
        mouse_event((uint)MouseEventFlags.LEFTDOWN, 0, 0, 0, 0);
    }

    void MouseClickUp()
    {
        mouse_event((uint)MouseEventFlags.LEFTUP, 0, 0, 0, 0);
        
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
