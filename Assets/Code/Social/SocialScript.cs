using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialScript : MonoBehaviour {



    public static void CheckAndSetPrivatePublicFunctions()
    {
        if (ApplicationStaticData.CanInteract())
        {
            GameObject.Find("Player").transform.Find("controller_left").Find("ControlObject").GetComponent<ControlObjects>().enabled = true;
            GameObject.Find("Player").transform.Find("controller_right").Find("ControlObject").GetComponent<ControlObjects>().enabled = true;
            if (GameObject.Find("Player").transform.Find("Toolbar")) GameObject.Find("Player").transform.Find("Toolbar").gameObject.SetActive(true);
        }
        else
        {
            GameObject.Find("Player").transform.Find("controller_left").Find("ControlObject").GetComponent<ControlObjects>().enabled = false;
            GameObject.Find("Player").transform.Find("controller_right").Find("ControlObject").GetComponent<ControlObjects>().enabled = false;
            if (GameObject.Find("Player").transform.Find("Toolbar")) GameObject.Find("Player").transform.Find("Toolbar").gameObject.SetActive(false);
        }
       
    }
}
