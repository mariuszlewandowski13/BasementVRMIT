using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadSceneScript : MonoBehaviour {

  

    private void Awake()
    {
        LoadFromFile();
    }

    private void LoadFromFile()
    {
        string[] filelines = File.ReadAllLines("../user_data");
        ApplicationStaticData.userName = filelines[0] + " " + filelines[1];
         
    }

    private void Start()
    {
       // GetComponent<ConnectAndJoinRandom>().Connect(false);
    }

    private void Update()
    {
    }



}
