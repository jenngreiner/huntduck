using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    public string remotePlayerName;
    private GameObject spawnedPlayer;

    void Start()
    {
        Debug.Log("NetworkPlayerSpawner has started");
        spawnedPlayer = PhotonNetwork.Instantiate(remotePlayerName, transform.position, transform.rotation);
        BNG.NetworkPlayer np = spawnedPlayer.GetComponent<BNG.NetworkPlayer>();

        if (np)
        {
            np.transform.name = "MyNetworkPlayer";
            np.AssignPlayerObjects();
            Debug.Log("NetworkPlayerSpawner has spawned networkplayer");
        }
    }
}
