using huntduck;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Oculus.Platform;
using Oculus.Platform.Models;

public class NameTag : MonoBehaviour
{
    public Text nameTag;
    
    // public m_platform
    // void Awake()
    // {
    // GameObject globalManagers = GameObject.Find("GlobalManagers");
    //  m_platform = globalManagers.GetComponent<PlatformManager>();
    // }
    void Start()
    {
        Debug.Log("GetLoggedInUserCallback PlatformManager.s_instance.m_myOculusID: " + PlatformManager.s_instance.m_myOculusID);

        string PlayerNickName = PlatformManager.s_instance.m_myOculusID;
        Debug.Log("GetLoggedInUserCallback PlayerNickName: " + PlayerNickName);

        PhotonNetwork.NickName = PlayerNickName;
        Debug.Log("GetLoggedInUserCallback PhotonNetwork.NickName: " + PhotonNetwork.NickName);
        // Debug.Log("GetLoggedInUserCallback PhotonNetwork.LocalPlayer.NickName: " + PhotonNetwork.LocalPlayer.NickName);
        // Debug.Log("GetLoggedInUserCallback PhotonNetwork.LocalPlayer.ActorNumber: " + PhotonNetwork.LocalPlayer.ActorNumber);
        // Debug.Log("GetLoggedInUserCallback PhotonNetwork.LocalPlayer.UserId: " + PhotonNetwork.LocalPlayer.UserId);
        // Debug.Log("GetLoggedInUserCallback PhotonNetwork.LocalPlayer.IsLocal: " + PhotonNetwork.LocalPlayer.IsLocal);  
        // Debug.Log("PhotonNetwork.NickName in Start is " + PhotonNetwork.NickName);

        // FIGURE OUT A WAY TO MAKE NICKNAME SPECIFIC TO THE OWNER / PLAYER_TAG
        nameTag.text = PlayerNickName;
        Debug.Log(transform.parent.parent.name + "'s nameTag is " + nameTag.text);
        PlayerPrefs.SetString("PlayerName", PlayerNickName);
    }

    // void GetLoggedInUserCallback(Message<User> msg)
    // {
    //     if (msg.IsError)
    //     {
    //         Debug.LogError("GetLoggedInUserCallback fucked up");
    //         return;
    //     }
    //
    //     // get the Oculus ID
    //     PhotonNetwork.NickName = msg.Data.OculusID;
    //     Debug.Log("PhotonNetwork.NickName in GetLoggedInUserCallback is " + PhotonNetwork.NickName);
    //
    //     //TransitionToState(State.WAITING_TO_PRACTICE_OR_MATCHMAKE);
    //     //Achievements.CheckForAchievmentUpdates();
    // }
}
