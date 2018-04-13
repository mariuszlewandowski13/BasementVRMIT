using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerRaycastScript : MonoBehaviour {

    public OVRInput.Controller controller;

    private bool active;
    private bool isPointing;

    private Vector3 hitPoint;

    private RaycastHit hit;

    private Transform actualPointing;

    private LineRenderer lineRenderer;

    void Start () {
        lineRenderer = GetComponent<LineRenderer>();
    }

	void Update () {

        if (ApplicationStaticData.CanInteract())
        {
            Ray ray = new Ray(transform.position, transform.forward);

            Physics.Raycast(ray, out hit, 1);
            if (hit.transform != null && hit.transform.GetComponent<IPointable>() != null)
            {
                isPointing = true;
                hitPoint = hit.point;
                CursorOn();

                if (hit.transform != actualPointing)
                {
                    if (actualPointing != null)
                    {
                        actualPointing.GetComponent<IPointable>().PointerOut();
                    }

                    actualPointing = hit.transform;
                    actualPointing.GetComponent<IPointable>().PointerIn();
                }
                bool pressedDown = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, controller);

                if (pressedDown)
                {
                    actualPointing.GetComponent<IPointable>().TriggerDown();
                }
            }
            else if (isPointing)
            {
                CursorOff();
                isPointing = false;

                if (actualPointing != null)
                {
                    actualPointing.GetComponent<IPointable>().PointerOut();
                    actualPointing = null;
                }
            }
        }
            
    }

    protected void CursorOn()
    {
        active = true;
        Vector3[] points = new Vector3[] { transform.position, hitPoint };
        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(points);
    }

    protected void CursorOff()
    {
        if (active)
        {
            active = false;
            lineRenderer.positionCount = 0;
        }

    }
}
