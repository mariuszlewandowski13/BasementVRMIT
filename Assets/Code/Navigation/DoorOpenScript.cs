#region Usings

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

#endregion

public class DoorOpenScript : MonoBehaviour {

    #region Public Properties

    public string roomName = "";
    public string sceneName = "";
    #endregion

    #region Private Properties

    private bool active = false;
    private bool colorActiveSet = false;
    private Color prevColor;

    

    public bool loadScene = false;

    #endregion

    #region Methods

    void Start()
    {
        GetComponent<ObjectInteractionScript>().ControllerCollision += ControllerCollision;
    }

    private void ControllerCollision(GameObject gameObj, bool isEnter)
    {
        if (isEnter && !active)
        {
            active = true;
            gameObj.GetComponent<ControlObjects>().TriggerDown += OnTriggerDown;
        }
        else if (!isEnter && !GetComponent<ObjectInteractionScript>().GetIsSelected() && active)
        {
            active = false;
            gameObj.GetComponent<ControlObjects>().TriggerDown -= OnTriggerDown;
        }
    }

    private void OnTriggerDown(GameObject controller)
    {
        if (roomName != null && sceneName != null)
        {
            if (PhotonNetwork.inRoom)
            {
               
                PhotonNetwork.Disconnect();
            }

            if (GameObject.Find("Desktop") != null)
            {
                GameObject.Find("Desktop").SetActive(false);
            }

            if (GameObject.Find("FirebaseManager") != null)
            {
                GameObject.Find("FirebaseManager").SetActive(false);
            }

            SetSceneToLoad();
        } 
    }

    void Update()
    {
        if (active && !colorActiveSet)
        {
            if (GetComponent<Renderer>() != null)
            {
                prevColor = GetComponent<Renderer>().material.color;
                GetComponent<Renderer>().material.SetColor("_Color", ApplicationStaticData.doorsHighligthing);
            }
            ChangeChildrenColor(ApplicationStaticData.doorsHighligthing);
            colorActiveSet = true;
        }
        else if (!active && colorActiveSet)
        {
            if (GetComponent<Renderer>() != null)
            {
                GetComponent<Renderer>().material.SetColor("_Color", prevColor);
            }
            RestoreChildrenColor();
            colorActiveSet = false;
        }

        if (loadScene && !PhotonNetwork.inRoom)
        {
           
            loadScene = false;
            SceneManager.LoadScene(sceneName);
        }
    }

    private void ChangeChildrenColor(Color col)
    {
       ChangeColorScript[] children =  transform.GetComponentsInChildren<ChangeColorScript>();
        foreach (ChangeColorScript child in children)
        {
            child.ChangeColor(col);
        }
    }

    private void RestoreChildrenColor()
    {
        ChangeColorScript[] children = transform.GetComponentsInChildren<ChangeColorScript>();
        foreach (ChangeColorScript child in children)
        {
            child.RestoreColor();
        }
    }

    public void SetSceneToLoad()
    {
        loadScene = true;
        ApplicationStaticData.roomToConnectName = roomName;
    }

    public void highlightRed()
    {
        if (GetComponent<Renderer>() != null)
        {
            prevColor = GetComponent<Renderer>().material.color;
            GetComponent<Renderer>().material.SetColor("_Color", ApplicationStaticData.doorsHighligthing);

        }
        ChangeChildrenColor(ApplicationStaticData.doorsHighligthing);
        colorActiveSet = true;
        active = true;
    }

    public void disableHighlight()
    {
        if (GetComponent<Renderer>() != null)
        {
            GetComponent<Renderer>().material.SetColor("_Color", prevColor);
        }
        RestoreChildrenColor();
        colorActiveSet = false;
        active = false;
    }

    private void OnApplicationQuit()
    {
        if (GameObject.Find("Desktop") != null)
        {
            GameObject.Find("Desktop").SetActive(false);
        }

        if (GameObject.Find("FirebaseManager") != null)
        {
            GameObject.Find("FirebaseManager").SetActive(false);
        }
    }

    #endregion
}
