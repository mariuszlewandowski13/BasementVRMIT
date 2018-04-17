using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerRing : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        other.gameObject.SendMessage("OnRing", null, SendMessageOptions.DontRequireReceiver);
    }
}
