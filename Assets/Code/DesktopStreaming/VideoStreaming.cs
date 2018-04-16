using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;
using System.Runtime.InteropServices;

public class VideoStreaming : MonoBehaviour {

    public RenderTexture m_Display;
    public firebaseManager fire;

    public byte[] textureBytes;
    public bool frameReady;

    private int resolutionWidth;
    private int resolutionHeight;

    
    private string lastFrameURL;

    private Texture2D tex;

    public bool streaming;

    private void Awake()
    {
        if (!ApplicationStaticData.IsSuperUser())
        {
            GetComponent<uDesktopDuplication.Texture>().enabled = false;
            tex = new Texture2D(2, 2);
            GetComponent<Renderer>().material.mainTexture = tex;
        }
    }

    private void Start()
    {
        if (ApplicationStaticData.IsSuperUser())
        {
            InitResolution();
        }
    }

    private void InitResolution()
    {
        resolutionWidth = Screen.currentResolution.width-1;
        resolutionHeight = Screen.currentResolution.height -1;
    }


    void Update () {
        if (ApplicationStaticData.IsSuperUser())
        {
            SendFrame();
        }
        else if (frameReady)
        {
            LoadFrame();
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


    IEnumerator SendNewFrame()
    {
        yield return new WaitForEndOfFrame();
        RenderTexture.active = m_Display;
        fire.upload(toTexture2D(m_Display).EncodeToJPG());
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
            fire.download();
        }
        
    }


    private void LoadBytesToTexture()
    {
        Debug.Log("loading texture");

        Debug.Log(Time.time);

        tex.LoadImage(textureBytes);
        tex.Apply();
        frameReady = false;
    }

}
