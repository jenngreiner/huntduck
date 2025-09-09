using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkLauncher : MonoBehaviourPunCallbacks
{
    [SerializeField] string gameVersion = "1";
    [SerializeField] string roomName = "DuckIsland";
    [SerializeField] byte maxPlayers = 6;
    [SerializeField] string targetScene = "Group_Hunt";
    bool _starting;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            StartMultiplayer();
            Debug.Log("KeyDown.G: StartMultiplayer(), go to Group_Hunt");
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
        // This is the key: enter the gameplay scene via Photon.
        PhotonNetwork.LoadLevel(targetScene);
    }
}
