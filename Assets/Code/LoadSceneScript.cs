using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class LoadSceneScript : MonoBehaviour {

    public GameObject[] teacherPointers;

    private void Awake()
    {
        Application.runInBackground = true;
      
    }

    void Start () {

        if (ApplicationStaticData.userType == UserType.student)
        {
            DisablePointers();
        }

        GetComponent<ConnectAndJoinRandom>().Connect(true);
    }

    private void DisablePointers()
    {
        foreach (GameObject pointer in teacherPointers)
        {
            pointer.SetActive(false);
        }
    }



}
