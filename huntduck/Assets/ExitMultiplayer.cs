using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System.Collections;

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
        //PhotonNetwork.Disconnect();
        //PhotonNetwork.LeaveRoom();

        if (PhotonNetwork.InRoom && PhotonNetwork.IsConnected)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            Debug.LogWarning("Cannot leave room: either not in a room or not connected.");
        }

    }

    //public override void OnLeftRoom()
    //{
    //    base.OnLeftRoom();
    //    StartCoroutine(DelayedSceneLoad());
    //}

    //private IEnumerator DelayedSceneLoad()
    //{
    //    yield return new WaitForSeconds(0.5f);  // Adjust as needed
    //    SceneManager.LoadSceneAsync(sceneName);
    //}

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        PhotonNetwork.Disconnect();
        SceneManager.LoadSceneAsync(sceneName);
    }
}
