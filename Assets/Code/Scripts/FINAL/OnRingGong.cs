using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnRingGong : MonoBehaviour
{

    private OrderManager orderManager;
    public AudioClip clip;

    void Start()
    {
        orderManager = GameObject.FindObjectOfType<OrderManager>();
    }

    public void OnRing()
    {       
        orderManager.gameObject.SendMessage("CompleteOrder", null, SendMessageOptions.DontRequireReceiver);
    }
}
