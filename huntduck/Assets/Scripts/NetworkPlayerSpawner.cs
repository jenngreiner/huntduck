using huntduck;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    public string remotePlayerName;
    private GameObject spawnedPlayer;
    private NameTag npTag;

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        spawnedPlayer = PhotonNetwork.Instantiate(remotePlayerName, transform.position, transform.rotation);
        BNG.NetworkPlayer np = spawnedPlayer.GetComponent<BNG.NetworkPlayer>();
        npTag = spawnedPlayer.GetComponent<NameTag>();

        if (np)
        {
            np.transform.name = "MyRemotePlayer";
            np.AssignPlayerObjects();


            photonView.RPC("SetNickName", RpcTarget.All);
            
            
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

    [PunRPC]
    void SetNickName()
    {
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
        Debug.Log("PhotonNetwork.NickName is " + PhotonNetwork.NickName);

        npTag.nameTag.text = PhotonNetwork.NickName;
        Debug.Log("npTag.nameTag.text is " + npTag.nameTag.text);


        //// WHAT WE GOT YO
        //Debug.Log("This player's nickname is " + PhotonNetwork.LocalPlayer.NickName);
        //Debug.Log("This player's actorNumber is " + PhotonNetwork.LocalPlayer.ActorNumber);
        //Debug.Log("This player's used ID is " + PhotonNetwork.LocalPlayer.UserId);
        //Debug.Log("'This player is local' is " + PhotonNetwork.LocalPlayer.IsLocal);
    }
}
