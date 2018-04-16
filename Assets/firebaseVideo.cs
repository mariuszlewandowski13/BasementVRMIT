using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class firebaseVideo : MonoBehaviour {

    private Texture2D tex;
    public firebaseManager fire;

   

    public GameObject cube;
    // Grab the camera's view when this variable is true.
    bool grab;
    public RenderTexture m_Display;

    private void Update()
    {
        //Press space to start the screen grab
        if (Input.GetKeyDown(KeyCode.Space))
            grab = true;
    }
    
    IEnumerator SendFrame()
    {
        yield return new WaitForSeconds(1);
        RenderTexture.active = m_Display;
        fire.upload(toTexture2D(m_Display).EncodeToJPG());
    }

    Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(1920, 1080, TextureFormat.RGB24, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }

    // Use this for initialization
    void Start () {
        StartCoroutine(SendFrame());
	}
	
}
