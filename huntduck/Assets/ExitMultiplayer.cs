using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System.Collections;
using Photon.Realtime;

public class ExitMultiplayer : MonoBehaviourPunCallbacks
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
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom(); // Leave the room first
        }
        else
        {
            DisconnectAndLoadScene(); // If not in a room, disconnect immediately
        }
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left the multiplayer room.");
        DisconnectAndLoadScene();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Disconnected from Photon: {cause}");
        SceneManager.LoadScene(sceneName);
    }

    private void DisconnectAndLoadScene()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect(); // Disconnect from Photon completely
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
