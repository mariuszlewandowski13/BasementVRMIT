using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGripDrumstick : MonoBehaviour
{
    private OvrAvatar avatar;
    public Transform knifePoseRight;

    public GameObject drumChild;
    void OnGrabEnter(Hand hand)
    {
        avatar = hand.GetComponentInParent<OvrAvatar>();
        avatar.RightHandCustomPose = knifePoseRight;

        drumChild.transform.localPosition = new Vector3(.0187f, -.0149f, .0176f);
    }

    void OnGrabExit(Hand hand)
    {
        avatar = hand.GetComponentInParent<OvrAvatar>();
        avatar.RightHandCustomPose = null;
    }

}
