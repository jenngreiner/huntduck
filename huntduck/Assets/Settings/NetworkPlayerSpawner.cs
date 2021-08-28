using UnityEngine;
using Photon.Pun;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    public string RemotePlayerName;
    private GameObject spawnedPlayer;

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        spawnedPlayer = PhotonNetwork.Instantiate(RemotePlayerName, transform.position, transform.rotation);
        BNG.NetworkPlayer np = spawnedPlayer.GetComponent<BNG.NetworkPlayer>();
        if (np)
        {
            np.transform.name = "MyRemotePlayer";
            np.AssignPlayerObjects();
        }
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.Destroy(spawnedPlayer);
    }
}
