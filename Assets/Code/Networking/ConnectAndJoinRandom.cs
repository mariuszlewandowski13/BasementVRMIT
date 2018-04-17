#region Usings

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Analytics;
//using Steamworks;

#endregion

/// <summary>
/// This script automatically connects to Photon (using the settings file),
/// tries to join a random room and creates one if none was found (which is ok).
/// </summary>
public class ConnectAndJoinRandom : Photon.MonoBehaviour
{
    /// <summary>Connect automatically? If false you can set this to true later on or call ConnectUsingSettings in your own scripts.</summary>
    /// 
    #region Private Properties
    private bool AutoConnect = true;

    private float Version = 2.0f;

    private bool loaded = false;

    /// <summary>if we don't want to connect in Start(), we have to "remember" if we called ConnectUsingSettings()</summary>
    private bool ConnectInUpdate = false;

    private bool ConnectToRoom = false;

    #endregion


    public static RoomInfo[] rooms;

    #region Methods

    //public virtual void Start()
    //{
    //    // PhotonNetwork.autoJoinLobby = false;    //no need to join a lobby to get the list of rooms.
    //    //PhotonNetwork.isMessageQueueRunning = false;
    //    // PhotonNetwork.lobby = new TypedLobby("basement", LobbyType.Default);
    //}

    //void Update()
    //{


    //    if (ConnectInUpdate && AutoConnect && !PhotonNetwork.connected && SaveLoadManager.isLoaded)
    //    {
    //        ConnectInUpdate = false;
    //        //PhotonNetwork.ConnectToRegion(CloudRegionCode.eu, Version + "." + SceneManagerHelper.ActiveSceneBuildIndex);
    //        //PhotonNetwork.ConnectToMaster()
           
    //    }


    //    //if (!loaded)
    //    //{

    //    //    GetComponent<SaveLoadManager>().LoadGame();
    //    //    loaded = true;
    //    //}

    //    //if (!msgQueue && SaveLoadManager.isLoaded)
    //    //{
    //    //    msgQueue = true;
    //    //    PhotonNetwork.isMessageQueueRunning = true;
    //    //}

    //    //if(!PhotonNetwork.connected && isConnected)
    //    //{
    //    //    Debug.Log("connection lost");
    //    //    isConnected = false;
    //    //}
    //}

    public void Connect(bool connectToRoom)
    {
        ConnectToRoom = connectToRoom;
        PhotonNetwork.ConnectUsingSettings(Version.ToString());
    }


    // below, we implement some callbacks of PUN
    // you can find PUN's callbacks in the class PunBehaviour or in enum PhotonNetworkingMessage


    public virtual void OnConnectedToMaster()
    {
        Debug.Log("TESTTASRTAST");
        GetComponent<PhotonView>().RPC("UserJoinedNotification", PhotonTargets.Others);
        Debug.Log("Region:" + PhotonNetwork.networkingPeer.CloudRegion);
       // SendAnalyticsRegionConnection();
        if (ApplicationStaticData.roomToConnectName != "")
        {
            Debug.Log("OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room. Calling: PhotonNetwork.JoinOrCreateRoom( " + ApplicationStaticData.roomToConnectName + ");");
            PhotonNetwork.JoinOrCreateRoom(ApplicationStaticData.roomToConnectName, new RoomOptions() { MaxPlayers = 0, IsVisible = true }, new TypedLobby() { });
        }
        else
        {
            Debug.Log("RoomName is empty. OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room. Calling: PhotonNetwork.JoinRandomRoom( );");
            PhotonNetwork.JoinRandomRoom();
        }

    }

    private void Update()
    {
        if (PhotonNetwork.insideLobby)
        {
            rooms = PhotonNetwork.GetRoomList();
        }
    }

    public virtual void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby(). This client is connected and does get a room-list, which gets stored as PhotonNetwork.GetRoomList().");

        Debug.Log(PhotonNetwork.lobby.Name);
        //
        // PhotonNetwork.JoinRandomRoom();

    }


    public virtual void OnReceivedRoomListUpdate()
    {
        //CreateWorldScript.UpdateUserCounters();
        //CreateHallScript.UpdateUserCounters();
        rooms = PhotonNetwork.GetRoomList();
        // Debug.Log(roomsList.Length);
        if (ConnectToRoom)
        {
            Debug.Log("joining room" + ApplicationStaticData.roomToConnectName);
            PhotonNetwork.JoinOrCreateRoom(ApplicationStaticData.roomToConnectName, new RoomOptions() { MaxPlayers = 0, IsVisible = true }, new TypedLobby() { });
        } 
    }

    public virtual void OnPhotonRandomJoinFailed()
    {
        Debug.Log("OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one. Calling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 4 }, null);
    }

    // the following methods are implemented to give you some context. re-implement them as needed.

    public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        Debug.LogError("Cause: " + cause);
    }


    public void OnJoinedRoom()
    {
        // Debug.Log("ADAM");

        Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room. From here on, your game would be running. For reference, all callbacks are listed in enum: PhotonNetworkingMessage");
        // PhotonNetwork.isMessageQueueRunning = false;
        //isConnected = true;
        // PhotonNetwork.QuickResends = 3;

    }

    //private void SendAnalyticsRoomEntered()
    //{

    //    Analytics.CustomEvent("RoomEntered", new Dictionary<string, object>
    //        {
    //        { "RoomID", ApplicationStaticData.roomToConnectName },
    //      });
    //}

    //private void SendRoomEnteredToDatabase()
    //{
    //    string sql = "UPDATE ROOMS SET VIEWS_COUNT = VIEWS_COUNT + 1 WHERE NAME = '" + ApplicationStaticData.roomToConnectName + "'";
    //    string sql2 = "UPDATE APP_USERS SET OTHERS_ROOMS_VIEWS = OTHERS_ROOMS_VIEWS + 1 WHERE USER_STEAM_ID = '" + ApplicationStaticData.userID + "'";
    //    if (GetComponent<DatabaseHandler>() != null)
    //    {
    //        GetComponent<DatabaseHandler>().ExequteSQL(sql);
    //        GetComponent<DatabaseHandler>().ExequteSQL(sql2);
    //    }

    //}

    //private void SendAnalyticsRegionConnection()
    //{
    //    Analytics.CustomEvent("RegionConnection", new Dictionary<string, object>
    //        {
    //        { "Region", PhotonNetwork.networkingPeer.CloudRegion },
    //      });
    //}

    #endregion
}
