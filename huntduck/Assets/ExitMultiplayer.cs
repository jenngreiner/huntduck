using UnityEngine;
using Photon.Pun;

public class ExitMultiplayer : MonoBehaviour
{
    public string sceneName;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            OnExitMultiplayer();
        }
    }

    public void OnExitMultiplayer()
    {
        PhotonNetwork.Disconnect();
    }
}
