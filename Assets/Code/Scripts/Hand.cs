using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Hand.cs
 * This is the controller for the hand, it is the main source of interaction for the player such as hovering and grabbing objects.
 */
public class Hand : MonoBehaviour
{
    public Hand otherHand; //useful for 2 handed interactions

    public OVRInput.Controller controller;  //controller to use
    public OVRInput.Button grabButton;      //button used to grab

    private Vector3 lastPosition;           //useful for tracking velocity
    public float throwForce = 1.5f;         //how far to throw

    public List<Interactable> interactables;
    private Interactable nearestInteractable;


    public float hoverRange = 0.3f;
    public bool isHoveringLocked;
    [SerializeField]
    private Interactable _hovering;
    public Interactable hovering
    {
        get
        {
            return _hovering;
        }
        set
        {
            if (_hovering != value)
            {
                if (_hovering != null)
                    _hovering.SendMessage("OnHoverExit",this,SendMessageOptions.DontRequireReceiver);
                
                _hovering = value;

                if(_hovering != null)
                    _hovering.SendMessage("OnHoverEnter",this,SendMessageOptions.DontRequireReceiver);
            }

        }
    }

    private bool isGrabbing;
    [SerializeField]
    private Interactable _grabbing;
    public Interactable grabbing
    {
        get
        {
            return _grabbing;
        }
        set
        {
            if (_grabbing != value)
            {
                if (_grabbing != null)
                    _grabbing.SendMessage("OnGrabExit", this, SendMessageOptions.DontRequireReceiver);

                _grabbing = value;

                if (_grabbing != null)
                    _grabbing.SendMessage("OnGrabEnter", this, SendMessageOptions.DontRequireReceiver);
            }

        }
    }


    // Use this for initialization
    void Start () {
		GetInteractables();
	}
	
	// Update is called once per frame
	void Update ()
	{
        GetInteractables();         //Need reference to all active interactables
	    GetNearestInteractable();
        SendHoverStayMessage();
        SendGrabStayMessage();
        CheckGrabDeleted();         //No need to hold object if it gets destroyed in scene
        HandlePlayerInput();
        CacheLastPosition();
	}

    /// <summary>
    /// Get interactables in scene
    /// </summary>
    void GetInteractables()
    {
        interactables = new List<Interactable>(FindObjectsOfType<Interactable>());
    }
	/// <summary>
	/// Calculates center of object's boundary for grabbing
	/// NOTE: tag any children objects with "ExcludeBoundary" to exclude them from the boundary calculation.
	/// </summary>
	Bounds CalculateBounds(GameObject target)
    {
        Bounds bounds = new Bounds(target.transform.position, Vector3.zero);
        foreach (Renderer rend in target.GetComponentsInChildren<Renderer>())
        {
			if (rend.CompareTag("ExcludeBoundary"))
				bounds.Encapsulate(rend.bounds);
        }

        return bounds;
    }

    /// <summary>
    /// Get nearest interactable to hand
    /// </summary>
    void GetNearestInteractable()
    {
        //Ignore if hover is locked
        if (isHoveringLocked)
            return;

        nearestInteractable = null; 
        float nearestDistance = hoverRange;

        foreach (Interactable interactable in interactables)
        {
            Bounds interactableBounds = CalculateBounds(interactable.gameObject);            
            float distanceToInteractable = Vector3.Distance(transform.position, interactableBounds.center) + interactableBounds.extents.magnitude;
            if (distanceToInteractable < nearestDistance)
            {
                nearestDistance = distanceToInteractable;
                nearestInteractable = interactable;
            }
        }

        //Hover if nearest interactable found and other hand is not grabbing
        if (nearestInteractable != null && nearestInteractable != otherHand.grabbing)
            hovering = nearestInteractable;
        else
            hovering = null;
    }

    /// <summary>
    /// Send hover stay message
    /// </summary>
    void SendHoverStayMessage()
    {
        if (hovering == null)
            return;
        hovering.SendMessage("OnHoverStay", this, SendMessageOptions.DontRequireReceiver);
    }

    /// <summary>
    /// Send hover stay message
    /// </summary>
    void SendGrabStayMessage()
    {
        if (grabbing == null)
            return;
        grabbing.SendMessage("OnGrabStay", this, SendMessageOptions.DontRequireReceiver);
    }

    /// <summary>
    /// Reset if grabbed object has been deleted
    /// </summary>
    void CheckGrabDeleted()
    {
        if (grabbing == null && isGrabbing)
        {
            UnlockHovering();
            isGrabbing = false;
        }
    }

    /// <summary>
    /// Handles player input
    /// </summary>
    void HandlePlayerInput()
    {
        //Grab based on trigger press and hovering
        if (OVRInput.GetDown(grabButton, controller) && hovering)
        {        
            GrabBegin();
        }
        //Release based on trigger release and grabbing
        if (OVRInput.GetUp(grabButton, controller) && grabbing)
        {
            GrabEnd();
        }
    }

    /// <summary>
    /// Grab specified object
    /// </summary>
    /// <param name="grabbedObject"></param>
    void GrabBegin()
    {
        //Update status
        LockHovering(hovering);
        grabbing = hovering;
        hovering = null;

        //Reposition and parent grabbed object and update physics
        if (grabbing.snapGrabbingPosition)
        {
            Vector3 pivotDelta = grabbing.transform.position - CalculateBounds(grabbing.gameObject).center;
            grabbing.transform.SetPositionAndRotation(transform.position + pivotDelta, transform.rotation);
        }
        grabbing.transform.SetParent(transform);
        Rigidbody grabbingRigidBody = grabbing.GetComponent<Rigidbody>();
        if (grabbingRigidBody) grabbingRigidBody.isKinematic = true;

        isGrabbing = true;
    }

    /// <summary>
    /// Release specified object
    /// </summary>
    /// <param name="grabbedObject"></param>
    void GrabEnd()
    {
        isGrabbing = false;

        //Unparent grabbed object and update physics
        grabbing.transform.parent = null;
        Rigidbody grabbingRigidBody = grabbing.GetComponent<Rigidbody>();
        if(grabbingRigidBody) grabbingRigidBody.isKinematic = false;

        //Add force based on velocity
        Vector3 handVelocity = (transform.position - lastPosition) / Time.deltaTime;
        if (grabbingRigidBody) grabbingRigidBody.AddForce(handVelocity * throwForce, ForceMode.VelocityChange);

        //Update status
        hovering = grabbing;
        grabbing = null;
        UnlockHovering();
    }

    /// <summary>
    /// Prevent hovering of other objects (or force a hover)
    /// </summary>
    /// <param name="hoveringObject"></param>
    void LockHovering(Interactable hoveringObject)
    {
        //Prevent hovering of other objects (or force a hover)
        if (hovering != hoveringObject)
            hovering = hoveringObject;

        isHoveringLocked = true;
    }

    /// <summary>
    /// Allow hovering
    /// </summary>
    void UnlockHovering()
    {
        isHoveringLocked = false;
    }

    /// <summary>
    /// Cache last position of hand
    /// </summary>
    void CacheLastPosition()
    {
        lastPosition = transform.position;
    }

}
