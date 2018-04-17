using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionEat : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("tray"))
        {
            gameObject.SendMessage("OnEat", other.gameObject, SendMessageOptions.DontRequireReceiver);
        }
    }
}
