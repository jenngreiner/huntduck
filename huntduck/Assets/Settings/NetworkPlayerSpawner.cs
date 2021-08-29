using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    public string remotePlayerName;
    private GameObject spawnedPlayer;

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        spawnedPlayer = PhotonNetwork.Instantiate(remotePlayerName, transform.position, transform.rotation);
        BNG.NetworkPlayer np = spawnedPlayer.GetComponent<BNG.NetworkPlayer>();
        if (np)
        {
            np.transform.name = "MyRemotePlayer";
            np.AssignPlayerObjects();

            string PlayerNickName = "Player " + PhotonNetwork.LocalPlayer.ActorNumber;
            PhotonNetwork.NickName = PlayerNickName;
            Debug.Log("This player's nickname is " + PhotonNetwork.LocalPlayer.NickName);
            Debug.Log("This player's actorNumber is " + PhotonNetwork.LocalPlayer.ActorNumber);
            Debug.Log("This player's used ID is " + PhotonNetwork.LocalPlayer.UserId);
            Debug.Log("'This player is local' is " + PhotonNetwork.LocalPlayer.IsLocal);
            
            // consider seeding scoreboard here
            // player is added to PhotonNetwork.PlayerList
            // can use .Length() to run a for loop and update scoreboard using nickname + score value pair
        }
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.Destroy(spawnedPlayer);
    }
}
