using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;
using System.Runtime.InteropServices;

public class VideoStreaming : MonoBehaviour {

    private uDesktopDuplication.Texture uddTexture;

    int x = 100;
    int y = 100;
    const int width = 64;
    const int height = 32;

    Color32[] colors = new Color32[width * height];

    byte[] textureBytes;

    private bool streaming;

    private Renderer movieRenderer;

    private int fileCounter = 0;

    private int resolutionWidth;
    private int resolutionHeight;

    private bool frameReady;
    private bool frameReadyToSend;
    private string lastFrameURL;

    private IntPtr texPointer;
    private Texture2D tex;

    private float lastSendingTime = 0.0f;

    private void Awake()
    {
        if (!ApplicationStaticData.IsSuperUser())
        {
            GetComponent<uDesktopDuplication.Texture>().enabled = false;
        }
        
    }

    private void Start()
    {
        GetComponent<Renderer>().material.mainTexture = tex;
        uddTexture = GetComponent<uDesktopDuplication.Texture>();
        movieRenderer = GetComponent<Renderer>();
        //InitFirebase();

        InitResolution();
        tex = new Texture2D(resolutionWidth, resolutionWidth);
        
    }

    private void InitResolution()
    {
        resolutionWidth = Screen.currentResolution.width;
        resolutionHeight = Screen.currentResolution.height;
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
        if (frameReadyToSend)
        {
            ProcessTextureBytes();
        }
        else if (!streaming)
        {
            streaming = true;
            Thread th = new Thread(SendNewTexture);
            th.Start();
            //SendNewTexture();
        }
    }

    void SendNewTexture()
    {
        GetNewFrame();
        if (!frameReadyToSend)
        {
            streaming = false;
        }
    }

    void ProcessTextureBytes()
    {
        
        SendTexture(colors);
        frameReadyToSend = false;
        streaming = false;

    }

    private void LoadFrame()
    {
        LoadBytesToTexture();
    }



    private void GetNewFrame()
    {
            uDesktopDuplication.Manager.primary.useGetPixels = true;

            var monitor = uddTexture.monitor;
            if (!monitor.hasBeenUpdated) return;

            if (monitor.GetPixels(colors, x, y, width, height))
            {
            frameReadyToSend = true;
            }
    }


    //void GetFrame()
    //{
    //    if (frameReady)
    //    {
    //        LoadBytesToTexture();
    //    }
    //    else if (!streaming)
    //    {
    //        streaming = true;
    //        Thread th = new Thread(DownloadNewFrame);
    //        th.Start();
    //        DownloadNewFrame();
    //    }
    //}

    private void LoadBytesToTexture()
    {
        Debug.Log("loading texture");

        Debug.Log(Time.time);


        tex.SetPixels32(colors);
        tex.Apply();
        frameReady = false;
    }

    [PunRPC]
    public void LoadNewTexture(Color32 [] tex)
    {
        if (!frameReady)
        {
            colors = tex;
            frameReady = true;
        }
        
    }

    public void SendTexture(Color32[] tex)
    {

        if (lastSendingTime + 0.5f < Time.time)
        {
            lastSendingTime = Time.time;
            if (PhotonNetwork.inRoom)
            {
                
                Debug.Log(lastSendingTime);
                GetComponent<PhotonView>().RPC("LoadNewTexture", PhotonTargets.Others, tex);
            }
        }
    }

}
