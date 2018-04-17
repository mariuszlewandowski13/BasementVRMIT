using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class LoadSceneScript : MonoBehaviour
{

    public GameObject lessonTeleport;

    public GameObject[] teacherPointers;

    public GameObject keyboard;

    public VideoStreaming videoStreaming;

    public GameObject player;

    private void Awake()
    {
        Application.runInBackground = true;
        ApplicationStaticData.roomToConnectName = ApplicationStaticData.className;
        videoStreaming.owner = ApplicationStaticData.IsSuperUser();
    }

    void Start()
    {

        if (ApplicationStaticData.userType == UserType.student)
        {
            DisablePointers();
        }

        keyboard.SetActive(false);

        GetComponent<ConnectAndJoinRandom>().Connect(true);

        lessonTeleport.GetComponent<DoorOpenScript>().roomName = ApplicationStaticData.userName + "_ROOM";
        SetPlayerPosition();
    }

  

    private void SetPlayerPosition()
    {
        if (!ApplicationStaticData.IsSuperUser())
        {
            player.transform.position = RandomNewPosition();
        }
    }

    private Vector3 RandomNewPosition()
    {
        float x = UnityEngine.Random.Range(-4.5f, 4.5f);
        float z = UnityEngine.Random.Range(2.0f, 5.0f);
        return new Vector3(x, 0.0f, z);
    }

    private void DisablePointers()
    {
        foreach (GameObject pointer in teacherPointers)
        {
            pointer.SetActive(false);
        }
    }

    private void Update()
    {
        if (ApplicationStaticData.IsSuperUser() &&  OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.LTouch) && OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            GetComponent<PhotonView>().RPC("ShowLesson", PhotonTargets.Others);
        }
    }

    [PunRPC]
    private void ShowLesson()
    {
        lessonTeleport.SetActive(true);
    }



}
