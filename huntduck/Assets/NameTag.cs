using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class NameTag : MonoBehaviour
{
    public Text nameTag;

    void Start()
    {
        nameTag.text = PhotonNetwork.NickName;
        Debug.Log(transform.parent.name + "'s nameTag is " + nameTag.text);
    }
}
