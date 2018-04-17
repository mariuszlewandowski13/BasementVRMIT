using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerFry : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        GetComponent<AudioSource>().Play();
        other.gameObject.SendMessage("OnFry", null, SendMessageOptions.DontRequireReceiver);
    }
}
