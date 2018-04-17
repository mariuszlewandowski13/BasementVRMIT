using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLessonScript : MonoBehaviour {

    public GameObject door;

	// Use this for initialization
	void Start () {
        door.GetComponent<DoorOpenScript>().roomName = ApplicationStaticData.className;

    }

}
