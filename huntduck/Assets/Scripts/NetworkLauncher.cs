using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkLauncher : MonoBehaviourPunCallbacks
{
    string gameVersion = "1";
    string roomName = "DuckIsland";
    byte maxPlayers = 6;
    string targetScene = "0_GroupHunt";
    bool _starting;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            StartMultiplayer();
            Debug.Log("KeyDown.G: StartMultiplayer(), go to 0_GroupHunt");
        }
    }

    public void StartMultiplayer()
    {
        if (_starting) return; // guards "double clicks"
        _starting = true;

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = gameVersion;

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRoom(roomName);
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRoom(roomName);
    }
    public override void OnJoinRoomFailed(short code, string msg)
    {
        PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = maxPlayers });
    }
    public override void OnJoinedRoom()
    {
        // Only master clients should load the level over network. Clients will automatically sync
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(targetScene);
        }
    }
}
