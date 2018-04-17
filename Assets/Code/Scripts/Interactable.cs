using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/* Interactable.cs
 * This determines that an object is interactable.
 */
public class Interactable : MonoBehaviour
{

    public bool debug;
	public bool snapGrabbingPosition = false;

	public UnityEvent OnHoverEnterEvent;
    public UnityEvent OnHoverStayEvent;
    public UnityEvent OnHoverExitEvent;

    public UnityEvent OnGrabEnterEvent;
    public UnityEvent OnGrabStayEvent;
    public UnityEvent OnGrabExitEvent;

    void OnHoverEnter(Hand hand)
    {
        OnHoverEnterEvent.Invoke();
        if (!debug)
            return;
        Debug.Log(name+".OnHoverEnter received from "+hand.name);
    }

    void OnHoverExit(Hand hand)
    {
        OnHoverExitEvent.Invoke();        
        if (!debug)
            return;
        Debug.Log(name + ".OnHoverExit received from " + hand.name);
    }

    void OnHoverStay(Hand hand)
    {
        OnHoverStayEvent.Invoke();
        if (!debug)
            return;
        Debug.Log(name + ".OnHoverStay received from " + hand.name);
    }

    void OnGrabEnter(Hand hand)
    {
        OnHoverExitEvent.Invoke();
        OnGrabEnterEvent.Invoke();
        if (!debug)
            return;
        Debug.Log(name + ".OnGrabEnter received from " + hand.name);
    }

    void OnGrabStay(Hand hand)
    {
        OnGrabStayEvent.Invoke();
        OnGrabStayEvent.Invoke();
        if (!debug)
            return;
        Debug.Log(name + ".OnGrabStay received from " + hand.name);
    }

    void OnGrabExit(Hand hand)
    {
        OnGrabExitEvent.Invoke();
        OnHoverEnterEvent.Invoke();
        if (!debug)
            return;
        Debug.Log(name + ".OnGrabExit received from " + hand.name);
    }

}
