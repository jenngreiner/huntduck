using huntduck;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Oculus.Platform;
using Oculus.Platform.Models;

public class NameTag : MonoBehaviourPunCallbacks
{
    public Text nameTag;

    void Start()
    {
        //photonView.RPC("SetNickName", RpcTarget.OthersBuffered);
        SetNickName();
    }

    //[PunRPC]
    void SetNickName()
    {
        string playerNickName;

        if (UnityEngine.Application.isEditor)
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


        nameTag.text = photonView.Owner.NickName;
        Debug.Log("npTag.nameTag.text is " + nameTag.text);


        //// WHAT WE GOT YO
        //Debug.Log("This player's nickname is " + PhotonNetwork.LocalPlayer.NickName);
        //Debug.Log("This player's actorNumber is " + PhotonNetwork.LocalPlayer.ActorNumber);
        //Debug.Log("This player's used ID is " + PhotonNetwork.LocalPlayer.UserId);
        //Debug.Log("'This player is local' is " + PhotonNetwork.LocalPlayer.IsLocal);
    }
}
