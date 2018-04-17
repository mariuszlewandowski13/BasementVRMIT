using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GrabController : MonoBehaviour
{
    public OVRInput.Controller controller;  //which controller to use
    public OVRInput.Button grabButton;      //button used to grab
    public List<Transform> grabbedItems;
    public Vector3 lastPosition;
    public float throwForce = 1.5f;         //how far to throw


    private void Start()
    {
        grabbedItems = new List<Transform>();
    }

    void Update()
    {
        HandleInput();
        lastPosition = transform.position;
    }

    /// <summary>
    /// Pickup an item
    /// </summary>
    /// <param name="item"></param>
    void Pickup(Transform item)
    {
        item.SetParent(transform);
        item.GetComponent<Rigidbody>().isKinematic = true;
    }

    /// <summary>
    /// Drop an item
    /// </summary>
    /// <param name="item"></param>
    void Drop(Transform item)
    {
        item.SetParent(null);
        item.GetComponent<Rigidbody>().isKinematic = false;

        Vector3 handVelocity = (transform.position - lastPosition) / Time.deltaTime;
        item.GetComponent<Rigidbody>().AddForce(handVelocity * throwForce, ForceMode.VelocityChange);
    }

    void OnTriggerStay(Collider col)
    {
        if (OVRInput.GetDown(grabButton, controller))
        {
            if (!grabbedItems.Contains(col.transform))
            {
                Pickup(col.transform);
                grabbedItems.Add(col.transform);
            }
        }
    }

    void HandleInput()
    {
        if (OVRInput.GetUp(grabButton, controller))
        {
            foreach (Transform item in grabbedItems)
            {
                Drop(item);
            }
            grabbedItems.Clear();
        }
    }
}