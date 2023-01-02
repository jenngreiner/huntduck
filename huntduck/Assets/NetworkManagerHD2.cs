using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System.Collections.Generic;

public class NetworkManagerHD2 : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private byte maxPlayersPerRoom = 0; // max player per room. if full, new room created. 0 = no max
    public string remotePlayerName = "RemotePlayer";
    public Text networkLogs;
    public Text playerListText;

    private string roomName;

    
    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        // Try to join or create the "DuckIsland" room
        string _roomName = "DuckIsland";
        PhotonNetwork.JoinOrCreateRoom(_roomName, new RoomOptions { MaxPlayers = maxPlayersPerRoom }, TypedLobby.Default);
        roomName = _roomName;
    }

    public override void OnJoinedRoom()
    {
        // log whether we created or joined the room
        if (PhotonNetwork.IsMasterClient)
        {
            LogText("Created room: <color=aqua>" + roomName + "</color>");
        }
        else
        {
            LogText("Joined room: <color=yellow>" + roomName + "</color>");
        }

        // Network Instantiate the object used to represent our player
        GameObject player = PhotonNetwork.Instantiate(remotePlayerName, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
        BNG.NetworkPlayer np = player.GetComponent<BNG.NetworkPlayer>();
        if (np)
        {
            np.transform.name = "MyNetworkPlayer";
            GameObject localPlayerObject = np.AssignPlayerObjects();
            PhotonNetwork.LocalPlayer.NickName = localPlayerObject.name;

            LogText("Created Network Player for <color=orange>" + localPlayerObject.name + "</color>");

            UpdatePlayerListUI();
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        // log the name of the new player to the
        LogText("Welcome new player: <color=magenta>" + newPlayer.NickName + "</color>");

        float playerCount = PhotonNetwork.IsConnected && PhotonNetwork.CurrentRoom != null ? PhotonNetwork.CurrentRoom.PlayerCount : 0;

        LogText("Connected players <color=orange>: " + playerCount + "</color>");
    }

    public override void OnPlayerLeftRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        // log the name of the new player to the
        LogText("Goodbye old friend: <color=red>" + newPlayer.NickName + "</color>");

        float playerCount = PhotonNetwork.IsConnected && PhotonNetwork.CurrentRoom != null ? PhotonNetwork.CurrentRoom.PlayerCount : 0;

        LogText("Connected players <color=orange>: " + playerCount + "</color>");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        UpdatePlayerListUI();
    }
    
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);

        // if the DuckIsland room is created and full, create a new one
        string _roomName = "DuckIsland Variant " + Random.Range(0, 1000);
        PhotonNetwork.CreateRoom(_roomName, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        roomName = _roomName;
    }

    void UpdatePlayerListUI()
    {
        // Clear the current player list
        playerListText.text = "";

        // Add the current player count to the player list
        playerListText.text += "Total Players: <color=orange>" + PhotonNetwork.CurrentRoom.PlayerCount + "</color>\n";

        // Add each player's nickname to the player list
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playerListText.text += "<color=orange>" + player.NickName + "</color>\n";
        }
    }

    void LogText(string message)
    {
        // Output to worldspace to help with debugging.
        if (networkLogs)
        {
            networkLogs.text += "\n" + message;
        }

        Debug.Log(message);
    }
}
