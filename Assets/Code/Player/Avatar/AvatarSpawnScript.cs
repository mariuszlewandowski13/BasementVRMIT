#region Usings

using UnityEngine;
using System.Collections;

#endregion

public class AvatarSpawnScript : MonoBehaviour {

    #region Public Properties

    public GameObject myAvatar;
    public bool isAvatarSet = false;


    #endregion

    void Update () {
        if (!isAvatarSet && PhotonNetwork.inRoom)
        {
            myAvatar = SpawnMyAvatar();
            isAvatarSet = true;
        }
	}


    private GameObject SpawnMyAvatar()
    {
        GameObject oldObject = GameObject.Find("Player").GetComponent<WorldObjects>().avatar;
        GameObject newObject = PhotonNetwork.Instantiate(oldObject.name, new Vector3(1000.0f, 1000.0f, 1000.0f), new Quaternion(), 0, null);
        //Debug.Log(newObject);
        newObject.GetComponent<AvatarFollowScript>().isEnabled = true;

        Color newColor = ApplicationStaticData.GetAvatarColor();
        newColor.a = 0.5f;

        newObject.GetComponent<AvatarScript>().SetAvatarColor(newColor);
        newObject.GetComponent<AvatarScript>().SetAvatarColorByNetwork(newColor);

        newObject.GetComponent<AvatarScript>().SetAvatarName(ApplicationStaticData.userName);
        newObject.GetComponent<AvatarScript>().SetAvatarNameByNetwork(ApplicationStaticData.userName);




        newObject.GetComponent<AvatarScript>().SetLayer("MyAvatar");

        if (newObject.transform.Find("rekaLewa") != null && GameObject.Find("Player").transform.Find("hand_left") != null)
        {
            newObject.transform.Find("rekaLewa").GetComponent<FollowScript>().parentObject = GameObject.Find("Player").transform.Find("hand_left").gameObject;
        }

        if (newObject.transform.Find("rekaPrawa") != null && GameObject.Find("Player").transform.Find("hand_right") != null)
        {
            newObject.transform.Find("rekaPrawa").GetComponent<FollowScript>().parentObject = GameObject.Find("Player").transform.Find("hand_right").gameObject;
        }

        if (newObject.transform.Find("glowa") != null && GameObject.Find("Player").transform.Find("OVRCameraRig").Find("TrackingSpace").Find("CenterEyeAnchor") != null)
        {
            newObject.transform.Find("glowa").GetComponent<FollowScript>().parentObject = GameObject.Find("Player").transform.Find("OVRCameraRig").Find("TrackingSpace").Find("CenterEyeAnchor").gameObject;
        }


        return newObject;
    }

   
}


