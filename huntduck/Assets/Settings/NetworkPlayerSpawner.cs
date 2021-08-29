using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

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
            Debug.Log("This player's nickname is " + PhotonNetwork.LocalPlayer.NickName);
            Debug.Log("This player's actorNumber is " + PhotonNetwork.LocalPlayer.ActorNumber);
            Debug.Log("'This player is local' is " + PhotonNetwork.LocalPlayer.IsLocal);
        }
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.Destroy(spawnedPlayer);
    }
}
