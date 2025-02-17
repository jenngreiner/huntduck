using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private string playerPrefabName; // Ensure prefab exists in Resources/

    private bool hasSpawned = false;

    private void Start()
    {
        Debug.Log($"[NetworkPlayerSpawner] Start() called in scene: {SceneManager.GetActiveScene().name}");

        if (PhotonNetwork.InRoom)
        {
            Debug.Log($"[NetworkPlayerSpawner] {PhotonNetwork.LocalPlayer.NickName} is in the room on start. Spawning player...");
            NetworkLogger.LogNetworkMessage("<color=orange>" + PhotonNetwork.LocalPlayer.NickName + "</color> is in the room on start. Spawning player...");
            SpawnPlayer();
        }
        else
        {
            Debug.LogWarning("[NetworkPlayerSpawner] Not in a Photon room yet. Waiting for OnJoinedRoom()...");
            NetworkLogger.LogNetworkMessage("<color=orange>" + PhotonNetwork.LocalPlayer.NickName + "</color> Not in a Photon room yet. Waiting for OnJoinedRoom()...");
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"[NetworkPlayerSpawner] {PhotonNetwork.LocalPlayer.NickName} successfully joined the room.");
        NetworkLogger.LogNetworkMessage("<color=orange>" + PhotonNetwork.LocalPlayer.NickName + "</color> successfully joined the room.");

        if (!hasSpawned)
        {
            Debug.Log("[NetworkPlayerSpawner] Spawning player after joining the room...");
            NetworkLogger.LogNetworkMessage("<color=orange>" + PhotonNetwork.LocalPlayer.NickName + "</color> Spawning player after joining the room...");
            SpawnPlayer();
        }
    }

    private void SpawnPlayer()
    {
        if (hasSpawned)
        {
            Debug.LogWarning("[NetworkPlayerSpawner] SpawnPlayer() was already called. Skipping duplicate spawn.");
            NetworkLogger.LogNetworkMessage("<color=orange>" + PhotonNetwork.LocalPlayer.NickName + "</color> SpawnPlayer() was already called. Skipping duplicate spawn."); return;
        }

        if (string.IsNullOrEmpty(playerPrefabName))
        {
            Debug.LogError("[NetworkPlayerSpawner] ERROR: Player prefab name is null or empty!");
            NetworkLogger.LogNetworkMessage("<color=orange>" + PhotonNetwork.LocalPlayer.NickName + "</color> ERROR: Player prefab name is null or empty!");
            return;
        }

        Debug.Log($"[NetworkPlayerSpawner] Instantiating player prefab: {playerPrefabName}");
        NetworkLogger.LogNetworkMessage("<color=orange>" + PhotonNetwork.LocalPlayer.NickName + "</color> Instantiating player prefab:" + playerPrefabName);

        GameObject spawnedPlayer = PhotonNetwork.Instantiate(playerPrefabName, Vector3.zero, Quaternion.identity);

        if (spawnedPlayer != null)
        {
            Debug.Log("[NetworkPlayerSpawner] Successfully instantiated:" + spawnedPlayer.name);
            NetworkLogger.LogNetworkMessage("<color=orange>" + PhotonNetwork.LocalPlayer.NickName + "</color> Successfully instantiated:" + spawnedPlayer.name);
            StartCoroutine(DelayedAssign(spawnedPlayer));
        }
        else
        {
            Debug.LogError("[NetworkPlayerSpawner] ERROR: Failed to instantiate player!");
            NetworkLogger.LogNetworkMessage("<color=orange>" + PhotonNetwork.LocalPlayer.NickName + "</color> ERROR: Failed to instantiate player!");
        }

        hasSpawned = true; // ✅ Prevents duplicate spawning
    }

    private IEnumerator DelayedAssign(GameObject spawnedPlayer)
    {
        yield return new WaitForSeconds(0.1f); // ✅ Ensures PhotonView is fully initialized before assigning objects

        BNG.NetworkPlayer np = spawnedPlayer.GetComponent<BNG.NetworkPlayer>();
        if (np)
        {
            np.AssignPlayerObjects();
            np.gameObject.name = PhotonNetwork.NickName + "'s Remote Player";
            Debug.Log("[NetworkPlayerSpawner] Remote player setup completed for " + PhotonNetwork.NickName);
            NetworkLogger.LogNetworkMessage("<color=orange>" + PhotonNetwork.LocalPlayer.NickName + "</color> Remote player setup completed for " + PhotonNetwork.NickName);
        }
    }
}



//using UnityEngine;
//using Photon.Pun;
//using UnityEngine.SceneManagement;
//using Photon.Realtime;

//public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
//{
//    [SerializeField] private string playerPrefabName; // Assign the prefab string name in the Inspector

//    private GameObject spawnedPlayer;

//    private void Start()
//    {
//        Debug.Log($"[NetworkPlayerSpawner] Start() called in scene: {SceneManager.GetActiveScene().name}. Spawning player...");
//        SpawnPlayer();
//    }

//    public override void OnJoinedRoom()
//    {
//        Debug.Log($"[NetworkPlayerSpawner] {PhotonNetwork.LocaalPlayer.NickName} joined the room. Spawning player...");

//        SpawnPlayer();
//    }

//    //public override void OnPlayerEnteredRoom(Player newPlayer)
//    //{
//    //    Debug.Log($"[NetworkPlayerSpawner] {PhotonNetwork.LocalPlayer.NickName} joined the room. Spawning player...");

//    //    SpawnPlayer();
//    //}

//    private void SpawnPlayer()
//    {
//        if (playerPrefabName == null)
//        {
//            Debug.LogError("[NetworkPlayerSpawner] ERROR: Player prefab is not assigned in the Inspector!");
//            return;
//        }

//        Debug.Log($"[NetworkPlayerSpawner] SpawnPlayer() is Instantiating player prefab: {playerPrefabName}");


//        if (spawnedPlayer == null)
//        {
//            spawnedPlayer = PhotonNetwork.Instantiate(playerPrefabName, Vector3.zero, Quaternion.identity);
//            BNG.NetworkPlayer np = spawnedPlayer.GetComponent<BNG.NetworkPlayer>();

//            if (np)
//            {
//                np.AssignPlayerObjects();
//                np.gameObject.name = PhotonNetwork.NickName + "'s Remote Player";
//                //Debug.Log("Remote player created for " + PhotonNetwork.NickName);
//            }
//        }

//        if (spawnedPlayer != null)
//        {
//            Debug.Log($"[NetworkPlayerSpawner] Successfully instantiated: {spawnedPlayer.name}");
//        }
//        else
//        {
//            Debug.LogError("[NetworkPlayerSpawner] ERROR: Failed to instantiate player!");
//        }
//    }
//}


////using UnityEngine;
////using UnityEngine.UI;
////using Photon.Pun;
////using Photon.Realtime;

////public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
////{
////    //public string remotePlayerName;
////    public GameObject remotePlayerPrefab;
////    private GameObject spawnedPlayer;
////    public static GameObject LocalPlayerInstance;

////    void Start()
////    {
////        if (photonView.IsMine)
////        {
////            SpawnPlayer();
////        }
////        else
////        {
////            Debug.Log("This GameObject does NOT belong to the local player.");
////        }
////    }

////    //public override void OnPlayerEnteredRoom(Player newPlayer)
////    //{
////    //    // Spawn player for new joiners
////    //    SpawnPlayer();
////    //}

////    private void SpawnPlayer()
////    {
////        //spawnedPlayer = PhotonNetwork.Instantiate(remotePlayerName, transform.position, transform.rotation);
////        spawnedPlayer = PhotonNetwork.Instantiate(remotePlayerPrefab.name, transform.position, transform.rotation);
////        BNG.NetworkPlayer np = spawnedPlayer.GetComponent<BNG.NetworkPlayer>();

////        if (np)
////        {
////            np.AssignPlayerObjects();
////            np.gameObject.name = PhotonNetwork.NickName + "'s Remote Player";
////            Debug.Log("Remote player created for " + PhotonNetwork.NickName);
////        }
////    }

////}
