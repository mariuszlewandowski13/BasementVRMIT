using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerPlate : MonoBehaviour
{
    private OrderManager orderManager;

    void Start()
    {
        orderManager = GameObject.FindObjectOfType<OrderManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("maki") || other.gameObject.CompareTag("sushi") || other.gameObject.CompareTag("tempura"))
        {
            orderManager.gameObject.SendMessage("AddToOrder", other.gameObject, SendMessageOptions.DontRequireReceiver);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("maki") || other.gameObject.CompareTag("sushi") || other.gameObject.CompareTag("tempura"))
        {
            orderManager.gameObject.SendMessage("RemoveFromOrder", other.gameObject, SendMessageOptions.DontRequireReceiver);
        }
    }
}
