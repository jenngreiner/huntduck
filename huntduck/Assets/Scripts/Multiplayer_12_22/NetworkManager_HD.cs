using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkManager_HD : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private byte maxPlayersPerRoom = 0; // max player per room. if full, new room created. 0 = no max
    public string sceneName;
    public string remotePlayerName = "RemotePlayer";
    public string debugObjName = "DebugText";
    public GameObject player;

    private Text debugText;
    private string roomName;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ConnectToServer();
        }
    }

    // fire this in using OnRespawn() section of Breakable button
    public void ConnectToServer()
    {
        // Initate Network Session on Photon server using the default server settings
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() 
    {
        base.OnConnectedToMaster();

        // Try to join or create the "DuckIsland" room
        string _roomName = "DuckIsland";
        PhotonNetwork.JoinOrCreateRoom("_roomName", new RoomOptions { MaxPlayers = maxPlayersPerRoom }, TypedLobby.Default);
        roomName = _roomName;
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        // Once we have created the room, load the specified scene
        if (PhotonNetwork.IsMasterClient) // sanity check that we are the master client
        {
            PhotonNetwork.LoadLevel(sceneName);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == sceneName)
        {
            // create player on network
            GameObject remotePlayer = PhotonNetwork.Instantiate(remotePlayerName, Vector3.zero, Quaternion.identity);

            //create tabletOfLogs on network
            GameObject tabletOfLogs = GameObject.Find(debugObjName);

            Debug.Log("Found tablet object named " + tabletOfLogs.name);

            // grab the actual text component in tabletOfLogs
            debugText = tabletOfLogs.GetComponentInChildren<Text>();

            if (PhotonNetwork.IsMasterClient)
            {
                LogText("<color=aqua>Created Room: </color>" + roomName);
            }
            else
            {
                LogText("<color=yellow>Joined Room: </color> " + roomName);
            }

            // map remote player (on network) local player to keep movement synchornized 
            BNG.NetworkPlayer np = remotePlayer.GetComponent<BNG.NetworkPlayer>();
            if (np)
            {
                np.transform.name = player.name + ": Remote Player";
                np.AssignPlayerObjects();
            }

            LogText("<color=orange>Player Networked Successfully: </color>" + np.playerNameTag + " created as remote player and mapped to local player: " + player.name);
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);

        // if the DuckIsland room is created and full, create a new one
        string _roomName = "DuckIsland Variant " + Random.Range(0, 1000);
        PhotonNetwork.CreateRoom(_roomName, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        roomName = _roomName;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        // log the name of the new player to the
        LogText("<color=magenta>Welcome New Player: </color>" + newPlayer.NickName);
    }

    void LogText(string message)
    {
        // Output to worldspace to help with debugging.
        if (debugText)
        {
            debugText.text += "\n" + message;
        }

        Debug.Log(message);
    }

    public override void OnEnable()
    {
        // Subscribe to events
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        // Unsubscribe from events
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}