using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDeliverPlate : MonoBehaviour {

    private bool isDelivering = false;

    private Vector3 destination;
    public float speed;

    void Update()
    {
        if (isDelivering)
        {
            if (transform.position != destination)
                transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            else
                isDelivering = false;
        }
    }

    void OnDeliver( Vector3 target )
    {
        destination = target;
        isDelivering = true;
    }
}
