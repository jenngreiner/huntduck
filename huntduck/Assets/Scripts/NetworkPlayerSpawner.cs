using huntduck;
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
        NameTag npTag = spawnedPlayer.GetComponent<NameTag>();

        if (np)
        {
            np.transform.name = "MyRemotePlayer";
            np.AssignPlayerObjects();

            string playerNickName;

            if (Application.isEditor)
            {
                playerNickName = "Player " + PhotonNetwork.LocalPlayer.ActorNumber;

            }
            else
            {
                // check OculusID
                Debug.Log("Player Oculus ID = " + PlatformManager.s_instance.m_myOculusID);

                playerNickName = PlatformManager.s_instance.m_myOculusID;
            }

            
            PhotonNetwork.NickName = playerNickName;
            npTag.nameTag.text = PhotonNetwork.NickName;


            // WHAT WE GOT YO
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
