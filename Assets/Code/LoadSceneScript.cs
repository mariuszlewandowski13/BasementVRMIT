using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class LoadSceneScript : MonoBehaviour {

    public GameObject[] teacherPointers;

    public GameObject keyboard;

    public VideoStreaming videoStreaming;

    private void Awake()
    {
        Application.runInBackground = true;
        ApplicationStaticData.userName = "";
        ApplicationStaticData.userRoom = ApplicationStaticData.userName + "_ROOM";
        ApplicationStaticData.roomToConnectName = ApplicationStaticData.className;
        videoStreaming.owner = ApplicationStaticData.IsSuperUser();
    }

    void Start () {

        if (ApplicationStaticData.userType == UserType.student)
        {
            DisablePointers();
        }

        keyboard.SetActive(false);

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
