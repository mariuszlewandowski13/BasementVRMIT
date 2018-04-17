using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGrabbable : MonoBehaviour {

	// Use this for initialization
    private Rigidbody rb;

	void Start ()
	{
	    rb = GetComponent<Rigidbody>();
        rb.gameObject.layer = LayerMask.NameToLayer("Grabbable");
    }
}
