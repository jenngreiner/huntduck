using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine.UI;

public class JoinMultiplayerNetwork : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private byte maxPlayersPerRoom = 0; // max player per room. if full, new room created. 0 = no max
    [SerializeField]
    private string sceneName = "GroupHunt";
    private GameObject player;
    private GameObject weapon;
    private bool isWeaponDestroyed;
    //private bool isWeaponDestroyed = false; //TODO: remove if not using coroutine to delay MP entry until gun destroyed

    [Tooltip("If true, do not destroy this object when moving to another scene")]
    public bool dontDestroyOnLoad = true;

    public string remotePlayerName;
    private GameObject spawnedPlayer;




    void Start()
    {
        // Connect to Random Room if Connected to Photon Server
        //if (PhotonNetwork.IsConnected)
        //{
        //    if (JoinRoomOnStart)
        //    {
        //        LogText("Joining Room : " + JoinRoomName);
        //        PhotonNetwork.JoinRoom(JoinRoomName);
        //    }
        //}
        //// Otherwise establish a new connection. We can then connect via OnConnectedToMaster
        //else
        //{
        //    PhotonNetwork.ConnectUsingSettings();
        //    PhotonNetwork.GameVersion = GameVersion;
        //}

        player = GameObject.FindGameObjectWithTag(TagManager.PLAYER_TAG);
        weapon = GameObject.FindGameObjectWithTag(TagManager.WEAPON_TAG); //TODO: remove if not using coroutine
        isWeaponDestroyed = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            JoinMultiplayer();
        }
    }

    void OnEnable()
    {
        base.OnEnable();
        MultiplayerTrigger.onMultiplayerTrigger += JoinMultiplayer;
    }

    void OnDisable()
    {
        base.OnDisable();
        MultiplayerTrigger.onMultiplayerTrigger -= JoinMultiplayer;
    }

    public void JoinMultiplayer()
    {
        //TODO: remove if not using coroutine
        StartCoroutine(DestroyWeaponThenJoinMultiplayer());
        //DestroyWeapon(); // can't bring non-networked weapon into multiplayer, best to destroy and start anew

        //SetNickname();
        //PhotonNetwork.AutomaticallySyncScene = true;
        //PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        // Try to join or create the "DuckIsland" room
        string _roomName = "DuckIsland";
        PhotonNetwork.JoinOrCreateRoom(_roomName, new RoomOptions { MaxPlayers = maxPlayersPerRoom }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room. Deffering Remote Player Representation.");
        // Master client loads level to sync for all clients
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(sceneName);
        }

        //Debug.Log("Joined Room. Creating Remote Player Representation.");
        //spawnedPlayer = PhotonNetwork.Instantiate(remotePlayerName, transform.position, transform.rotation);
        //BNG.NetworkPlayer np = spawnedPlayer.GetComponent<BNG.NetworkPlayer>();

        //if (np)
        //{
        //    np.AssignPlayerObjects();
        //    Debug.Log("Player created " + np.name);
        //}

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
            playerNickName = OculusPlatform.MyOculusID;
        }

        PhotonNetwork.NickName = playerNickName;
    }

    //TODO: remove if not using coroutine
    //Give enough time to destroy gun before joining MP to not cause errors on gun in networked scene
    private IEnumerator DestroyWeaponThenJoinMultiplayer()
    {
        DestroyWeapon(); // can't bring non-networked weapon into multiplayer, best to destroy and start anew

        while (!isWeaponDestroyed)
        {
            yield return null; // won't progress until gun fully destroyed
        }

        SetNickname();
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    private void DestroyWeapon()
    {
        Destroy(weapon);
        //weapon.SetActive(false);
        isWeaponDestroyed = true;
    }
}
