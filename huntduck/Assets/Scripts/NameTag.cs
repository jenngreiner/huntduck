using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class NameTag : MonoBehaviour
{
    public Text nameTag;

    void Start()
    {
        // set the network player gameObject name AND its nametag (child) to the PhotonNetwork nickname for that player
        PhotonView playerPV = gameObject.GetComponent<PhotonView>();
        gameObject.transform.name = nameTag.text = playerPV.Owner.NickName;
        Debug.Log("nameTag.text is " + nameTag.text);
    }
}
