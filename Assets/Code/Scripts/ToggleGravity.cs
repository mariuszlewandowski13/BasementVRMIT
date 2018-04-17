using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGravity : MonoBehaviour {

    public bool useGravity;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = useGravity;
    }
    void Update ()
    {
        rb.useGravity = useGravity;
    }

}
