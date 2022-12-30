//using UnityEngine;
//using UnityEngine.UI;
//using Photon.Pun;
//using Oculus;

//public class NameTag : MonoBehaviour
//{
//    public Text nameTag;

//    void Start()
//    {
//        // FIGURE OUT A WAY TO MAKE NICKNAME SPECIFIC TO THE OWNER / PLAYER_TAG
//        //PhotonNetwork.NickName = huntduck.PlatformManager.MyOculusID; // not working in editor, needs headset testing
//        nameTag.text = PhotonNetwork.NickName;
//        Debug.Log(transform.parent.parent.name + "'s nameTag is " + nameTag.text);
//        PlayerPrefs.SetString("PlayerName", PhotonNetwork.NickName);
//    }
//}



using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Oculus.Platform;
using Oculus.Platform.Models;

public class NameTag : MonoBehaviour
{
    public Text nameTag;

    void Start()
    {
        //commenting out while in editor
        //Users.GetLoggedInUser().OnComplete(GetLoggedInUserCallback);
        //Debug.Log("PhotonNetwork.NickName in Start is " + PhotonNetwork.NickName);
        ChangeMyName();
    }

    void OnEnable()
    {
        //subscribe
        NetworkManager_HD.onNameChanged += ChangeMyName;
    }

    void OnDisable()
    {
        //unsubscribe
        NetworkManager_HD.onNameChanged -= ChangeMyName;
    }

    void ChangeMyName()
    {
        if (transform.root.tag == TagManager.REMOTE_PLAYER_TAG)
        {
            PhotonNetwork.NickName = transform.root.name;
        }

        // FIGURE OUT A WAY TO MAKE NICKNAME SPECIFIC TO THE OWNER / PLAYER_TAG
        nameTag.text = PhotonNetwork.NickName;
        Debug.Log(transform.root.name + "'s nameTag is " + nameTag.text);
        PlayerPrefs.SetString("PlayerName", PhotonNetwork.NickName);
    }

    void GetLoggedInUserCallback(Message<User> msg)
    {
        if (msg.IsError)
        {
            Debug.LogError("GetLoggedInUserCallback fucked up");
            return;
        }

        // get the Oculus ID
        PhotonNetwork.NickName = msg.Data.OculusID;
        Debug.Log("PhotonNetwork.NickName in GetLoggedInUserCallback is " + PhotonNetwork.NickName);

        //TransitionToState(State.WAITING_TO_PRACTICE_OR_MATCHMAKE);
        //Achievements.CheckForAchievmentUpdates();
    }
}