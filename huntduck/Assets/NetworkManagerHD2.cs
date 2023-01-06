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
    private string sceneName;

    private bool showOnce = false;

    
    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        sceneName = "GroupHunt";
        UpdatePlayerListUI();
    }

    void Update()
    {
        //once the level has loaded
        if(PhotonNetwork.LevelLoadingProgress == 1 && !showOnce)
        {
            showOnce = true;
            LogText("Created room: <color=aqua>" + PhotonNetwork.CurrentRoom.Name + "</color>");
            LogText("Welcome new player: <color=orange>" + PhotonNetwork.LocalPlayer.NickName + "</color>");
            UpdatePlayerListUI();
        }
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        // Try to join or create the "DuckIsland" room
        string _roomName = "DuckIsland";
        PhotonNetwork.JoinOrCreateRoom(_roomName, new RoomOptions { MaxPlayers = maxPlayersPerRoom }, TypedLobby.Default);
        roomName = _roomName;
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        // Once we have created the room, load the specified scene
        if (PhotonNetwork.IsMasterClient) // sanity check that we are the master client
        {
            PhotonNetwork.LoadLevel(sceneName);
            //photonView.RPC("LogText", RpcTarget.All, "Created room: <color=aqua>" + roomName + "</color>");
        }
    }

    public override void OnJoinedRoom()
    {
        // Network Instantiate the object used to represent our player
        GameObject player = PhotonNetwork.Instantiate(remotePlayerName, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
        BNG.NetworkPlayer np = player.GetComponent<BNG.NetworkPlayer>();
        PhotonView playerPV = player.GetComponent<PhotonView>();
        if (np)
        {
            np.transform.name = "MyNetworkPlayer";
            GameObject localPlayerObject = np.AssignPlayerObjects();
            playerPV.Owner.NickName = localPlayerObject.name;

            Debug.Log("Created Network Player for <color=orange> " + playerPV.Owner.NickName + "</color>");
            LogText("Player is  <color=orange>" + localPlayerObject + "</color>");
            UpdatePlayerListUI();

            //photonView.RPC("LogText", RpcTarget.All, "Created Network Player for <color=orange> " + playerPV.Owner.NickName + "</color >");
            //photonView.RPC("UpdatePlayerListUI", RpcTarget.All);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        LogText("Welcome new player: <color=orange>" + newPlayer.NickName + "</color>");
        UpdatePlayerListUI();
    }

    public override void OnPlayerLeftRoom(Player leavingPlayer)
    {
        base.OnPlayerLeftRoom(leavingPlayer);

        LogText("Goodbye old friend: <color=red>" + leavingPlayer.NickName + "</color>");
        UpdatePlayerListUI();

        // log the name of the new player to the
        //photonView.RPC("LogText", RpcTarget.All, "Goodbye old friend: <color=red>" + leavingPlayer.NickName + "</color>");
        //photonView.RPC("UpdatePlayerListUI", RpcTarget.All);
    }

    //public override void OnRoomListUpdate(List<RoomInfo> roomList)
    //{
    //    base.OnRoomListUpdate(roomList);

    //    photonView.RPC("UpdatePlayerListUI", RpcTarget.All);
    //}
    
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

        if (PhotonNetwork.IsConnectedAndReady)
        {
            // Add the current player count to the player list
            playerListText.text += "Total Players: <color=orange>" + PhotonNetwork.CurrentRoom.PlayerCount + "</color>\n";

            // Add each player's nickname to the player list
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                playerListText.text += "<color=orange>" + player.NickName + "</color>\n";
            }
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
