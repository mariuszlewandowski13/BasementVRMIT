#region Usings

using UnityEngine;
using System.Collections;

#endregion

public class ToolbarRotationScript : MonoBehaviour {

    #region Events

    public delegate void MoveAction(float Speed);
    public static event MoveAction MoveEvent;


    #endregion

    #region Private Properties

    private float Speed = 0.00f;
    private float decreasingFactor = 0.003f;

    private object speedLock = new object();

    private object eventLock = new object();

    private int added = 0;

    public bool rotationLeft;
    public bool rotationRight;

    

    #endregion

    #region Methods
    void Start()
    {
        AddToRotationEvent();
    }
    void Update()
    {
        lock(speedLock)
        {
            if (Speed != 0.0f)
            {
                DecreaseSpeed(decreasingFactor);
                MoveEvent(Speed);
             //   Debug.Log("Move event");
            } 

        }
         
    }
    public void DecreaseSpeed(float decreasingFactor)
    {
        if (Speed < 0.0f)
        {
            Speed += decreasingFactor;
            if (Speed > 0.0f || !rotationRight) Speed = 0.0f;

        }
        else if (Speed > 0.0f)
        {
            Speed -= decreasingFactor;
            if (Speed < 0.0f || !rotationLeft) Speed = 0.0f;
        }   
    }

    public void SpeedStop()
    {
        Speed = 0.0f;
    }

    private void Rotation(Vector2 axis, bool first)
    {
      //  Debug.Log(axis);
        lock(speedLock)
        {
                Speed = axis.x/2.0f;
        }
        
            
    }

    public void inverseSpeed()
    {
        lock(speedLock)
        {
            Speed = -Speed;
        }
    }

    public void AddToRotationEvent()
    {
        lock(eventLock)
        {
            added++;
            if (added > 0)
            {
                ControllerTouchpadScript.TouchpadSwipe += Rotation;
            }
        }
    }

    public void RemoveFromRotationEvent()
    {
        lock (eventLock)
        {
            added--;
            if (added == 0)
            {
                ControllerTouchpadScript.TouchpadSwipe -= Rotation;
            }
            
        }
        
    }

    #endregion
}
