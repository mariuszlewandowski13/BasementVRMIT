using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RequestFor11Session : MonoBehaviour {

    private bool requested;

    private bool canGo;

    private  Queue<PhotonPlayer> players;



    private void Start()
    {
         players = new Queue<PhotonPlayer>();
    }

    void Update () {
        if (!requested && !ApplicationStaticData.IsSuperUser() && OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.RTouch) && OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            requested = true;
            CreateRequest();
        }

        if (ApplicationStaticData.IsSuperUser() && OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.RTouch) && OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            GetFirstAndGoToSession();
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
        players.Enqueue(player);
        Highlight();
    }

    private void Highlight()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }

    public void GetFirstAndGoToSession()
    {
        if (players.Count > 0)
        {
            SendReplyAndGoToSession(players.Dequeue());
        }
        
    }

    private void SendReplyAndGoToSession(PhotonPlayer player)
    {
        if (PhotonNetwork.inRoom)
        {
            GetComponent<PhotonView>().RPC("GoToSession", player);
        }
        GoToSession();
    }
    
    [PunRPC]
    private void GoToSession()
    {
        canGo = true;
    }

}
