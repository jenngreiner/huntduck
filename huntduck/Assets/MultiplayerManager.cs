//using UnityEngine;
//using UnityEngine.SceneManagement;
//using Photon.Pun;
//using Photon.Realtime;
//using System.Collections.Generic;
//using System.Collections;
//using UnityEngine.UI;

//public class MultiplayerManager : MonoBehaviourPunCallbacks
//{
//    public static MultiplayerManager Instance { get; private set; }

//    [SerializeField]
//    private byte maxPlayersPerRoom = 10;
//    [SerializeField]
//    private string multiplayerSceneName = "GroupHunt";
//    [SerializeField]
//    private string menuSceneName = "0_HD_AppLab";
//    private const string RoomPrefix = "DuckIsland";
//    private static int roomCounter = 0;
//    private List<RoomInfo> availableRooms = new List<RoomInfo>();

//    private GameObject player;
//    private GameObject weapon;
//    private bool isWeaponDestroyed;

//    //[Header("UI Elements")]
//    //public GameObject lobbyUI;
//    //public Button startHuntButton;
//    //public Button exitMultiplayerButton;

//    private void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject);
//            //PhotonNetwork.AutomaticallySyncScene = true;
//        }
//        else
//        {
//            Destroy(gameObject);
//            return;
//        }
//    }

//    void Start()
//    {
//        player = GameObject.FindGameObjectWithTag(TagManager.PLAYER_TAG);
//        weapon = GameObject.FindGameObjectWithTag(TagManager.WEAPON_TAG);
//        isWeaponDestroyed = false;
//    }

//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.M))
//        {
//            JoinLobby();
//        }
//    }

//    public void JoinLobby()
//    {
//        PhotonNetwork.AutomaticallySyncScene = true;
//        StartCoroutine(DestroyWeaponThenJoinLobby());
//    }


//    //Give enough time to destroy gun before joining MP to not cause errors on gun in networked scene
//    private IEnumerator DestroyWeaponThenJoinLobby()
//    {
//        DestroyWeapon(); // can't bring non-networked weapon into multiplayer, best to destroy and start anew

//        while (!isWeaponDestroyed)
//        {
//            yield return null; // won't progress until gun fully destroyed
//        }

//        if (!PhotonNetwork.IsConnected)
//        {
//            Debug.Log("[MultiplayerManager] Connecting to Photon Master Server");
//            PhotonNetwork.ConnectUsingSettings();
//        }
//        else
//        {
//            Debug.LogWarning($"[MultiplayerManager] Cannot join lobby, current state: {PhotonNetwork.NetworkClientState}");
//        }
//    }

//    private void DestroyWeapon()
//    {
//        Destroy(weapon);
//        isWeaponDestroyed = true;
//    }
//    private void SetNickname()
//    {
//        string playerNickName;

//        if (UnityEngine.Application.isEditor)
//        {
//            playerNickName = player.name + PhotonNetwork.LocalPlayer.ActorNumber;

//        }
//        else
//        {
//            playerNickName = OculusPlatform.MyOculusID;
//        }

//        PhotonNetwork.NickName = playerNickName;
//    }

//    public override void OnConnectedToMaster()
//    {
//        Debug.Log("[MultiplayerManager] Connected to Master, joining lobby");
//        SetNickname();
//        //PhotonNetwork.JoinLobby();
//        AssignOrCreateRoom();
//    }

//    public override void OnRoomListUpdate(List<RoomInfo> roomList)
//    {
//        availableRooms = roomList;
//    }

//    public override void OnJoinedLobby()
//    {
//        base.OnJoinedLobby();

//        AssignOrCreateRoom();
//    }

//    private void AssignOrCreateRoom()
//    {
//        RoomOptions roomOptions = new RoomOptions { MaxPlayers = maxPlayersPerRoom };
//        string roomName = RoomPrefix + roomCounter;

//        foreach (RoomInfo room in availableRooms)
//        {
//            if (room.PlayerCount < maxPlayersPerRoom)
//            {
//                PhotonNetwork.JoinRoom(room.Name);
//                return;
//            }
//        }

//        roomCounter++;
//        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
//    }

//    public override void OnJoinedRoom()
//    {
//        base.OnJoinedRoom();

//        if (PhotonNetwork.IsMasterClient)
//        {
//            PhotonNetwork.LoadLevel(multiplayerSceneName);
//            Debug.Log("[MultiplayerManager] Master Client has loaded scene");
//        }
//    }

//    public void StartGame()
//    {
//        Debug.Log("[MultiplayerManager] Game started by shooting the START object. Loading multiplayer scene...");
//        //start some version of WaveManager

//    }

//    public void ExitMultiplayer()
//    {
//        if (PhotonNetwork.InRoom)
//        {
//            PhotonNetwork.LeaveRoom();
//        }
//        else
//        {
//            LoadMenuScene();
//        }
//    }

//    public override void OnLeftRoom()
//    {
//        base.OnLeftRoom();

//        Debug.Log("[MultiplayerManager] Left Room. Disconnecting and returning to lobby...");
//        PhotonNetwork.Disconnect();
//    }

//    public override void OnDisconnected(DisconnectCause cause)
//    {
//        base.OnDisconnected(cause);

//        Debug.Log("[MultiplayerManager] Disconnected from Photon. Loading lobby scene.");
//        LoadMenuScene();
//    }

//    private void LoadMenuScene()
//    {
//        SceneManager.LoadSceneAsync(menuSceneName);
//    }
//}
