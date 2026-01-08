using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

// connect to the network on first load
public class NetworkManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        ConnectedToServer();
    }

    private void ConnectedToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("NetworkManager.cs: try connect to server...");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("NetworkManager.cs: Connected to server.");
        base.OnConnectedToMaster();
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        PhotonNetwork.JoinOrCreateRoom("Room 1", roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("NetworkManager.cs: Joined a room");
        base.OnJoinedRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //newPlayer.NickName = ("Player " + newPlayer.ActorNumber);
        Debug.Log("NetworkManager.cs: A new player name " + newPlayer.NickName + "joined the room");
        base.OnPlayerEnteredRoom(newPlayer);
    }
}
