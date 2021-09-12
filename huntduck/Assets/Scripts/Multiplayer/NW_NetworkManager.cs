using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

// connect to the network on first load
public class NW_NetworkManager : MonoBehaviourPunCallbacks
{
    // BEN: Added on 0911
    /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
    string gameVersion = "1";

    // BEN: Added on 0911
    /// Keep track of the current process. Connectino is asynh and based on several callbacks from Photon. Need to keep track of this to properly adjust the behavior when we receive call back by Photon.
    /// Typically this is used for the OnConnectedToMaster() callback.
    bool isConnecting;

    // BEN: Added on 0911
    private void Awake()
    {
        /// <summary>
        /// #Critical
        /// this makes sure we can use PhtonNetwork.LoadLevel() on the master client and all clients in the same room synctheir level automatically.
        /// </summary>
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        ConnectedToServer();
    }

    // BEN: Added on 0911
    private void ConnectedToServer()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
            Debug.Log("try connect to server...");
        }
    }

    // BEN: Disabled on 0911
    //private void ConnectedToServer()
    //{
    //    PhotonNetwork.ConnectUsingSettings();
    //    Debug.Log("try connect to server...");
    //}

    // BEN: Added this 0911
    public override void OnConnectedToMaster()
    {
        if (isConnecting)
        {
            // #Critical: The first we try to do is to join a potential existingroom. If there is, good, else we'll be caled back with OnJoinRandomFailed()
            PhotonNetwork.JoinRandomRoom();
            isConnecting = false;
        }
        else
        {
            Debug.Log("Connected to server.");
            base.OnConnectedToMaster();
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 10;
            roomOptions.IsVisible = true;
            roomOptions.IsOpen = true;
        }
    }

    // BEN: Disabled this 0911
    //public override void OnConnectedToMaster()
    //{
    //    Debug.Log("Connected to server.");
    //    base.OnConnectedToMaster();
    //    RoomOptions roomOptions = new RoomOptions();
    //    roomOptions.MaxPlayers = 10;
    //    roomOptions.IsVisible = true;
    //    roomOptions.IsOpen = true;

    //    PhotonNetwork.JoinOrCreateRoom("Room 1", roomOptions, TypedLobby.Default);
    //}

    // BEN: Added this 0911
    public override void OnJoinedRoom()
    {
        Debug.Log("NetWorkManager: OnJoinedRoom() was called by PUN. Now this client was in a room.");

        // only load if first player, else use 'PhotonNetwork.AutomaticallySyncScene
        Debug.Log("We load the 'MP_PracticeRange'");

        // Load the Master Client Level to be sure we are synced
        PhotonNetwork.LoadLevel("MP_PracticeRange");
    }

    // BEN: Disabled this 0911
    //public override void OnJoinedRoom()
    //{
    //    Debug.Log("Joined a room");
    //    base.OnJoinedRoom();
    //}

    // BEN: Added this 0911
    public override void OnDisconnected(DisconnectCause cause)
    {
        isConnecting = false;

        Debug.LogWarningFormat("Assets/Launcher: OnDisconnected() was called by PUN with the reason {0}", cause);
    }

    // BEN: I don't think we need this because we have an else statement in OnConnectedToMaster, but in case it doesn't work, try some version of this this
    //public override void OnJoinRandomFailed(short returnCode, string message)
    //{
    //    Debug.Log("NetworkManager: OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

    //    // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
    //    PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    //}

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //newPlayer.NickName = ("Player " + newPlayer.ActorNumber);
        Debug.Log("A new player name " + newPlayer.NickName + "joined the room");
        base.OnPlayerEnteredRoom(newPlayer);
    }
}
