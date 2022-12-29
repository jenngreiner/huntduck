using UnityEngine;
using Photon.Pun;

public class SetPlayerNickname : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.LocalPlayer.NickName = transform.name;
    }
}
