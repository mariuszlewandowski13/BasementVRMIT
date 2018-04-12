#region Usings

using UnityEngine;

#endregion

public class OnLoadRoomLoadingScript : MonoBehaviour {

    #region Private Properties

    private bool created = false;

    private bool isToolbarActiveCheck = false;

    private bool tutorialStarted = false;

    private bool controllersLoaded;

    public static int  taskNumber = 0;

    #endregion

    #region Methods

    void Awake()
    {
        ApplicationStaticData.LoadAllData();
    }

    void Update()
    {
        if (!controllersLoaded)
        {
            ControlObjectsHelper.UpdateControllers();
            controllersLoaded = true;
        }

        if (PhotonNetwork.inRoom && !created)
        {
           GameObject centerLeft =  PhotonNetwork.Instantiate("RotationCenterLeft", new Vector3(1.0f, 1.0f, 1.0f), new Quaternion(), 0, null);
           GameObject centerRight = PhotonNetwork.Instantiate("RotationCenterRight", new Vector3(1.0f, 1.0f, 1.0f), new Quaternion(), 0, null);
           
            if (GameObject.Find("Player").transform.Find("controller_left") != null)
            {
                GameObject.Find("Player").transform.Find("controller_left").Find("ControlObject").GetComponent<ControlObjects>().rotationCenter = centerLeft;
            }
            if (GameObject.Find("Player").transform.Find("controller_right") != null)
            {
                GameObject.Find("Player").transform.Find("controller_right").Find("ControlObject").GetComponent<ControlObjects>().rotationCenter = centerRight;
            }

            created = true;
        }

        if (!isToolbarActiveCheck)
        {
            SocialScript.CheckAndSetPrivatePublicFunctions();
            isToolbarActiveCheck = true;
        }

    }
    #endregion

}
