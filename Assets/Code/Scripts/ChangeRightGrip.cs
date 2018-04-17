using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRightGrip : MonoBehaviour {

    private OvrAvatar avatar;

    private void Awake()
    {
        avatar = GetComponentInParent<OvrAvatar>();
    }

    void OnChangeGrip(Transform trans)
    {
        avatar.RightHandCustomPose = trans;
    }

    void OnResetGrip()
    {
        avatar.RightHandCustomPose = null;
    }
}
