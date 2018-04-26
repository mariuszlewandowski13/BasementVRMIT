using UnityEngine;
using System.Runtime.InteropServices;


public class VideoStreaming : MonoBehaviour {

    public bool owner;

    [DllImport("user32")]
    public static extern int SetCursorPos(int x, int y);

    [DllImport("user32.dll")]
    static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);


    private void OnWillRenderObject()
    {
        if (owner)
        {
            GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(-1, 1));
        }
        
    }


    void CalculateMousePosition(out int x, out int y, float xIn, float yIn)
    {
        x = (int)((1 - xIn) * Screen.currentResolution.width);
        y = (int)(yIn * Screen.currentResolution.height);
    }


    public void HandleMouseClick(float xTex, float yTex)
    {
        if (owner)
        {
            int x;
            int y;
            CalculateMousePosition(out x, out y, xTex, yTex);
            SetCursor(x, y);
        }
        else {
            if (PhotonNetwork.inRoom)
            {
                GetComponent<PhotonView>().RPC("HandleMouseClick", PhotonTargets.Others, xTex, yTex);
            }
        }
        
    }


    [PunRPC]
    private void SetCursor(int x, int y)
    {
        SetCursorPos(x, y);
    }

    private void ClickDown()
    {
        mouse_event((uint)MouseEventFlags.LEFTDOWN, 0, 0, 0, 0);
    }

    [PunRPC]
    public void MouseClickDown()
    {
        if (owner)
        {
            ClickDown();
        }
        else
        {
            if (PhotonNetwork.inRoom)
            {
                GetComponent<PhotonView>().RPC("ClickDown", PhotonTargets.Others);
            }
        }
    }

    [PunRPC]
    private void ClickUp()
    {
        mouse_event((uint)MouseEventFlags.LEFTUP, 0, 0, 0, 0);
    }

    public void MouseClickUp()
    {
        if (owner)
        {
            ClickUp();
        }
        else
        {
            if (PhotonNetwork.inRoom)
            {
                GetComponent<PhotonView>().RPC("ClickUp", PhotonTargets.Others);
            }
        }
    }

}
