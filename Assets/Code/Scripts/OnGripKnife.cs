using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGripKnife : MonoBehaviour {

    private OvrAvatar avatar;

    public Transform knifePoseRight;
    public GameObject knifeModel;

    void OnGrabEnter(Hand hand)
    {
        avatar = hand.GetComponentInParent<OvrAvatar>();
        avatar.RightHandCustomPose = knifePoseRight;
        knifeModel.transform.localPosition = new Vector3(0.014f,.01f,.011f);
    }

    void OnGrabExit(Hand hand)
    {
        avatar = hand.GetComponentInParent<OvrAvatar>();
        avatar.RightHandCustomPose = null;
        knifeModel.transform.localPosition = new Vector3(-0.01f, .058f, .024f);
    }
}

