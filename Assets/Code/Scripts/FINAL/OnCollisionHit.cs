using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionHit : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("tray"))
        {
            gameObject.SendMessage("OnHit", other.gameObject, SendMessageOptions.DontRequireReceiver);
        }
    }
}
