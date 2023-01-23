using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    public string remotePlayerName;
    private GameObject spawnedPlayer;

    void Start()
    {
        spawnedPlayer = PhotonNetwork.Instantiate(remotePlayerName, transform.position, transform.rotation);
        BNG.NetworkPlayer np = spawnedPlayer.GetComponent<BNG.NetworkPlayer>();

        if (np)
        {
            np.AssignPlayerObjects();
        }
    }
}
