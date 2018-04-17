using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RequestFor11Session : MonoBehaviour {

    private bool requested;

    private bool canGo;

    private List<PhotonPlayer> players = new List<PhotonPlayer>();

	void Update () {
        if (!requested && !ApplicationStaticData.IsSuperUser() && OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.RTouch) && OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            requested = true;
            CreateRequest();
        }

        if (canGo && !PhotonNetwork.inRoom)
        {
            LoadSession();
        }
	}

    private void LoadSession()
    {
        SceneManager.LoadScene("Scene1-1");
    }



    private void CreateRequest()
    {
        if (PhotonNetwork.inRoom)
        {
            GetComponent<PhotonView>().RPC("GetRequest", PhotonTargets.Others, PhotonNetwork.player);
        }
    }

    [PunRPC]
    private void GetRequest(PhotonPlayer player)
    {
        if (ApplicationStaticData.IsSuperUser())
        {
            CreateNewRequestObject(player);
        }
    }

    private void CreateNewRequestObject(PhotonPlayer player)
    {

    }


    public void SendReplyAndGoToSession(PhotonPlayer player)
    {
        if (PhotonNetwork.inRoom)
        {
            GetComponent<PhotonView>().RPC("GoToSession", player);
        }
        GoToSession();
    }

    private void GoToSession()
    {

    }

}
