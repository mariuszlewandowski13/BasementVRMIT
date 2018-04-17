using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;
using System.Runtime.InteropServices;


public class VideoStreaming : MonoBehaviour {

    public bool owner;

    public string filename = "0.jpg";

    public RenderTexture m_Display;
    public firebaseManager fire;

    public byte[] textureBytes;
    public bool frameReady;

    private int resolutionWidth;
    private int resolutionHeight;

    
    private string lastFrameURL;

    private Texture2D tex;

    public bool streaming;

    [DllImport("user32")]
    public static extern int SetCursorPos(int x, int y);

    [DllImport("user32.dll")]
    static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);

   
    private void Start()
    {
        if (owner)
        {
            InitResolution();
        }
        else {
            GetComponent<uDesktopDuplication.Texture>().enabled = false;
            tex = new Texture2D(2, 2);
            GetComponent<Renderer>().material.mainTexture = tex;
        }
    }

    private void InitResolution()
    {
        resolutionWidth = Screen.currentResolution.width/2;
        resolutionHeight = Screen.currentResolution.height/2;
        m_Display.width = resolutionWidth;
        m_Display.height = resolutionHeight;

    }


    void Update () {
        if (PhotonNetwork.inRoom)
        {
            if (owner)
            {
                SendFrame();
            }
            else
            {
                LoadFrame();
            }
        }
       
       
	}

    void SendFrame()
    {
        if (!streaming)
        {
            streaming = true;
            StartCoroutine(SendNewFrame());
        }
    }

    private void OnWillRenderObject()
    {
        if (owner)
        {
            GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(-1, 1));
        }
        else {
            GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(-1, -1));
        }
        
    }


    IEnumerator SendNewFrame()
    {

        yield return new WaitForEndOfFrame();
        RenderTexture.active = m_Display;
        if (!firebaseManager.uploading)
        {
            fire.upload(toTexture2D(m_Display).EncodeToJPG(), filename);
        }
        else {
            streaming = false;
        }
        
    }

    Texture2D toTexture2D(RenderTexture rTex)
    {
        RenderTexture.active = rTex;
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }

    private void LoadFrame()
    {
       
        if (frameReady)
        {
            LoadBytesToTexture();
        }
        else if (!streaming)
        {
            streaming = true;
            fire.download(filename);
        }
        
    }


    private void LoadBytesToTexture()
    {
        Debug.Log("loading texture");

        Debug.Log(Time.time);

        tex.LoadImage(textureBytes);
        tex.Apply();
        frameReady = false;
        streaming = false;
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
