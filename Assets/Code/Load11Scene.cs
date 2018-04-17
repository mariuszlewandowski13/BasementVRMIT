using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load11Scene : MonoBehaviour {

    public GameObject keyboard;
    public VideoStreaming videoStreamingTeacher;
    
    void Awake () {
        Application.runInBackground = true;
        videoStreamingTeacher.owner = !ApplicationStaticData.IsSuperUser();
    }

    private void Start()
    {
        ApplicationStaticData.roomToConnectName = "session11";
        keyboard.SetActive(false);
        GetComponent<ConnectAndJoinRandom>().Connect(true);
    }

}
