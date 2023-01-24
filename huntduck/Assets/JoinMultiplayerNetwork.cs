using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class JoinMultiplayerNetwork : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private byte maxPlayersPerRoom = 0; // max player per room. if full, new room created. 0 = no max
    [SerializeField]
    private string sceneName = "GroupHunt";
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag(TagManager.PLAYER_TAG);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            JoinMultiplayer();
        }
    }

    public void JoinMultiplayer()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        // Try to join or create the "DuckIsland" room
        string _roomName = "DuckIsland";
        PhotonNetwork.JoinOrCreateRoom(_roomName, new RoomOptions { MaxPlayers = maxPlayersPerRoom }, TypedLobby.Default);
        SetNickname();

        //PhotonNetwork.NickName = player.name;

        Debug.Log("Player has connected to master and joined or created room called " + _roomName);
    }

    public override void OnJoinedRoom()
    {
        // Master client loads level to sync for all clients
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(sceneName);
            Debug.Log("Loaded level via photon");
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);

        // if the DuckIsland room is created and full, create a new one
        string _roomName = "DuckIsland Variant " + UnityEngine.Random.Range(0, 1000);
        PhotonNetwork.CreateRoom(_roomName, new Photon.Realtime.RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    private void SetNickname()
    {
         string playerNickName;

        if (UnityEngine.Application.isEditor)
        {
            playerNickName = player.name + PhotonNetwork.LocalPlayer.ActorNumber;

        }
        else
        {
            // check OculusID
            Debug.Log("Player Oculus ID = " + OculusPlatform.MyOculusID);

            playerNickName = OculusPlatform.MyOculusID;
        }

        PhotonNetwork.NickName = playerNickName;
        Debug.Log("PhotonNetwork.NickName is " + PhotonNetwork.NickName);
    }
}
