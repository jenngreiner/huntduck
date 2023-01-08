using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

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
        PhotonNetwork.Disconnect();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        SceneManager.LoadSceneAsync(sceneName);
    }
}
