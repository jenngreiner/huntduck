using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.PUN;

public class ExitMultiplayer : MonoBehaviourPunCallbacks
{
    public string sceneName;
    public GameObject PunVoiceManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            OnExitMultiplayer();
        }
    }

    public void OnExitMultiplayer()
    {
        //HD_CHANGE: unsubscribe PhotonVoiceClient & Destroy it
        if (PunVoiceClient.Instance != null)
        {
            PunVoiceClient.Instance.UnsubscribeFromStateChanges();
            Destroy(PunVoiceClient.Instance.gameObject);
        }

        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        SceneManager.LoadSceneAsync(sceneName);

    }
}

//PhotonNetwork.LeaveRoom();
//PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer); 

//if (PhotonNetwork.InRoom && PhotonNetwork.IsConnected)
//{
//    if (PhotonNetwork.Server != ServerConnection.MasterServer)
//    {
//        Debug.Log("On the master server, we won't leave room. Disconnecting instead");
//        PhotonNetwork.Disconnect();
//    }
//    else
//    {
//        PhotonNetwork.LeaveRoom();
//    }
//}
//else
//{
//    if (PhotonNetwork.IsConnected)
//    {
//        Debug.Log("Cannot leave room, disconnecting: either not in a room or not connected.");
//        PhotonNetwork.Disconnect();
//    }
//}

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

//public override void OnLeftRoom()
//{
//    base.OnLeftRoom();

//    PhotonNetwork.Disconnect();
//}