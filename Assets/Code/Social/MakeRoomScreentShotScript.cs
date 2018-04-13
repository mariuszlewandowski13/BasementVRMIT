using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class MakeRoomScreentShotScript : MonoBehaviour {

    private static string screenShotname= "basement.png";
    private static string id = "";
    private bool capturingStarted;
    private float screenShotTime = 30.0f;
    private float startTime;

    private Camera camera2;
    private Texture2D tex;
 
    private bool textureReady;


    public void MakeScreenshot()
    {
        capturingStarted = true;
        tex = CaptureImage();
        textureReady = true;
    }

    private void SaveTexture()
    {
        id = ApplicationStaticData.roomToConnectName.Split(new string[] { "_" }, StringSplitOptions.None)[0];
        byte[] bytes = tex.EncodeToPNG();
        File.WriteAllBytes(ApplicationStaticData.screenshotsPath + screenShotname, bytes); 
    }

    void Start()
    {
        if (GameObject.Find("ScreenShotCamera") != null)
        {
            camera2 = GameObject.Find("ScreenShotCamera").GetComponent<Camera>();
            if (camera2 != null)
            {
                camera2.enabled = false;
                startTime = Time.time;
            }
                
        }
        
    }


    void Update()
    {
        if (camera2 != null)
        {
            if (capturingStarted && File.Exists(ApplicationStaticData.screenshotsPath + screenShotname))
            {
                SaveScreenShotToTheServer();
                capturingStarted = false;
            }

            if (textureReady)
            {
                SaveTexture();
                textureReady = false;
            }

            if (Time.time - startTime >= screenShotTime)
            {
                Debug.Log("Screenshot time!!!");
                StartCoroutine(StartTakeScreenShot());
                startTime = Time.time;
            }
        }
        

    }


    private void SaveScreenShotToTheServer()
    {
        byte[] file = File.ReadAllBytes(ApplicationStaticData.screenshotsPath + screenShotname);
        WWWForm form = new WWWForm();
        form.AddField("userID", id);
        form.AddBinaryData("file", file);
        WWW w = new WWW(ApplicationStaticData.scriptsServerAdress + "SaveScreenshot.php", form);
        StartCoroutine(request(w));
    }

    IEnumerator request(WWW w)
    {
        yield return w;
        string message;
        if (w.error == null)
        {
            message = w.text;
        }
        else {
            message = "ERROR: " + w.error + "\n";
        }
       // Debug.Log(message);
        File.Delete(ApplicationStaticData.screenshotsPath + screenShotname);
    }

    public Texture2D CaptureImage()
    {
        RenderTexture.active = camera2.targetTexture;
        camera2.Render();
        Texture2D captured = new Texture2D(camera2.targetTexture.width, camera2.targetTexture.height, TextureFormat.ARGB32, false);
        
        captured.ReadPixels(new Rect(0, 0, camera2.targetTexture.width, camera2.targetTexture.height), 0, 0);
        captured.Apply();
        RenderTexture.active = null;
        return captured;
    }

    IEnumerator StartTakeScreenShot()
    {
        yield return new WaitForEndOfFrame();
        MakeScreenshot();

    }

}
