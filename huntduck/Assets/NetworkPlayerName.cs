using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkPlayerName : MonoBehaviour
{
    //public string updatedName;

    void OnEnable()
    {
        NetworkManager_HD.onNameChanged += SetPlayerName;
    }

    [PunRPC]
    public void SetPlayerName(string newName)
    {
        transform.root.name = newName;
    }
}
