﻿#region Usings

using UnityEngine;
using System.Collections;

#endregion

public class BackButtonScript : MonoBehaviour {

    #region Private Properties

    private bool active = false;

    private GameObject ownerToolbar;

    public delegate void BackButtonClicked();
    public event BackButtonClicked ButtonDown;

    public bool CanGoBack = true;

    #endregion

    #region Methods
    void Start()
    {
        GetComponent<ObjectInteractionScript>().ControllerCollision += ControllerCollision;
        ownerToolbar = transform.parent.parent.gameObject;
    }

    private void ControllerCollision(GameObject gameObj, bool isEnter)
    {
        if (isEnter && !active)
        {
            gameObj.GetComponent<ControlObjects>().TriggerDown += OnTriggerDown;
            active = true;
        }
        else if (!isEnter && active)
        {
            gameObj.GetComponent<ControlObjects>().TriggerDown -= OnTriggerDown;
            active = false;
        }
    }

    private void OnTriggerDown(GameObject controller)
    {

        ownerToolbar.GetComponent<ToolbarManagerScript>().MainBar();
        if (ButtonDown != null)
        {
            ButtonDown();
        }

    }
    void Update()
    {
        //if (active && GetComponent<HighlightingSystem.Highlighter>() != null)
        //{
        //    GetComponent<HighlightingSystem.Highlighter>().SeeThroughOff();
        //    GetComponent<HighlightingSystem.Highlighter>().On();
        //}
    }


    #endregion
}