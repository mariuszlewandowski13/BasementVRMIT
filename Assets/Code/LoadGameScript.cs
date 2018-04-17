using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadGameScript : MonoBehaviour {

    public GameObject door;


    private void LoadFromFile()
    {
        string[] filelines = File.ReadAllLines("../user_data");
        ApplicationStaticData.userName = filelines[0] + " " + filelines[1];

    }

    private void Start()
    {
        LoadFromFile();
        door.GetComponent<DoorOpenScript>().roomName = ApplicationStaticData.className;

    }

}
