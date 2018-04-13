using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class WaitingForLogin : MonoBehaviour {

    public Text textField;

    private float speed = 0.2f;
	
    private int actualCounter = 0;
    private float lastUpdateTime = 0.0f;

    private int maxPointsCount = 3;

	
	void Update () {
        if (Time.time > lastUpdateTime + speed)
        {
            lastUpdateTime = Time.time;
            textField.text = "Waiting" + new string('.', actualCounter);
            actualCounter++;
            actualCounter = (actualCounter > maxPointsCount ? 0 : actualCounter);
        }
	}
}
