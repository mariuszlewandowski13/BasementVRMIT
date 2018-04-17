using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load11Scene : MonoBehaviour {

    public GameObject keyboard;

    public VideoStreaming videoStreamingStudent;
    public VideoStreaming videoStreamingTeacher;

    // Use this for initialization
    void Start () {
        videoStreamingTeacher.owner = ApplicationStaticData.IsSuperUser();
        videoStreamingStudent.owner = !ApplicationStaticData.IsSuperUser();

        ApplicationStaticData.roomToConnectName = "session11";
        keyboard.SetActive(false);
        GetComponent<ConnectAndJoinRandom>().Connect(true);
    }

}
