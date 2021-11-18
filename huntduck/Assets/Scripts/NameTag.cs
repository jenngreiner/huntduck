using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class NameTag : MonoBehaviour
{
    public Text nameTag;

    void Start()
    {
        // FIGURE OUT A WAY TO MAKE NICKNAME SPECIFIC TO THE OWNER / PLAYER_TAG
        nameTag.text = PhotonNetwork.NickName;
        Debug.Log(transform.parent.parent.name + "'s nameTag is " + nameTag.text);
        PlayerPrefs.SetString("PlayerName", PhotonNetwork.NickName);
    }
}
