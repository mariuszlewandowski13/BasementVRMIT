using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;

public class VideoStreaming : MonoBehaviour {


    private bool streaming;

    private Renderer movieRenderer;

    private byte [] textureBytes;

    private int fileCounter = 0;

    private int resolutionWidth;
    private int resolutionHeight;

    private bool frameReady;
    private bool frameReadyToSend;
    private string lastFrameURL;

    private void Awake()
    {
        if (!ApplicationStaticData.IsSuperUser())
        {
            GetComponent<Video>().enabled = false;
        }
    }

    private void Start()
    {
        movieRenderer = GetComponent<Renderer>();
        //InitFirebase();

        InitResolution();
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
        }
    }

    void SendNewTexture()
    {
        GetNewFrame();
        if (textureBytes != null && textureBytes.Length > 0)
        {
            frameReadyToSend = true;
        }
        else
        {
            streaming = false;
        }
    }

    void ProcessTextureBytes()
    {
        frameReadyToSend = false;
        SendTexture(textureBytes);
        streaming = false;

    }

    private void LoadFrame()
    {
        LoadBytesToTexture();
    }



    private void GetNewFrame()
    {
        var bmpScreenshot = new Bitmap(resolutionWidth,
                                       resolutionHeight,
                                       PixelFormat.Format32bppArgb);
        var gfxScreenshot = System.Drawing.Graphics.FromImage(bmpScreenshot);
        gfxScreenshot.CopyFromScreen(0,
                                    0,
                                    0,
                                    0,
                                    new Size(resolutionWidth, resolutionHeight),
                                    CopyPixelOperation.SourceCopy);

        // Save the screenshot to the specified path that the user has chosen.
        bmpScreenshot.Save("Screenshot.jpg", ImageFormat.Jpeg);
        bmpScreenshot.Dispose();

        textureBytes = File.ReadAllBytes("Screenshot.jpg");
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
        
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(textureBytes);
        
        movieRenderer.material.mainTexture = tex;
        movieRenderer.material.SetTextureScale("_MainTex", new Vector2(-1, -1));
        frameReady = false;
    }

    [PunRPC]
    public void LoadNewTexture(byte [] tex)
    {
        if (!frameReady)
        {
            textureBytes = tex;
            frameReady = true;
        }
        
    }

    public void SendTexture(byte[] tex)
    {
        if (PhotonNetwork.inRoom)
        {
            GetComponent<PhotonView>().RPC("LoadNewTexture", PhotonTargets.Others, tex);
        }

    }


}
