using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NetworkLogger : MonoBehaviourPunCallbacks
{
    public Text networkLogs;
    public Text playerListText;

    bool hasLoadedScene;

    public static NetworkLogger instance { get; private set; }

    void Start()
    {
        // when scene (and thus this script) loads on network, update the player list for everyone
        Debug.Log("NetworkLogger is now operational");

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("LogText", RpcTarget.AllBufferedViaServer, "Current Room is: <color=aqua>" + PhotonNetwork.CurrentRoom.Name + "</color>");
            photonView.RPC("LogText", RpcTarget.AllBufferedViaServer, "The allmighty room creator is: <color=orange>" + PhotonNetwork.MasterClient.NickName + "</color>");
            photonView.RPC("UpdatePlayerListUI", RpcTarget.All);
        }

    }

    // TODO: try switching logtext functions to these callbacks
    //public override void OnJoinedRoom()
    //{
    //    base.OnJoinedRoom();
    //}

    //public override void OnLeftRoom()
    //{
    //    base.OnLeftRoom();
    //}

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        //will only fire on non-master clients because of this callback fires before PhotonNetwork.LoadLevel is completed by master
        photonView.RPC("LogText", RpcTarget.AllBufferedViaServer, "Welcome new player: <color=orange>" + newPlayer.NickName + "</color>");
        photonView.RPC("UpdatePlayerListUI", RpcTarget.All);
    }

    public override void OnPlayerLeftRoom(Player leavingPlayer)
    {
        base.OnPlayerLeftRoom(leavingPlayer);

        photonView.RPC("LogText", RpcTarget.AllBufferedViaServer, "Goodbye old friend: <color=red>" + leavingPlayer.NickName + "</color>");
        photonView.RPC("UpdatePlayerListUI", RpcTarget.All);
    }

    [PunRPC]
    void UpdatePlayerListUI()
    {
        // Clear the current player list
        playerListText.text = "";

        if (PhotonNetwork.IsConnectedAndReady)
        {
            // Add the current player count to the player list
            playerListText.text += "Total Players: <color=orange>" + PhotonNetwork.CurrentRoom.PlayerCount + "</color>\n";

            // Add each player's nickname to the player list
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                playerListText.text += "<color=orange>" + player.NickName + "</color>\n";
            }
        }
    }

    [PunRPC]
    void LogText(string message)
    {
        // Output to worldspace to help with debugging.
        if (networkLogs)
        {
            networkLogs.text += "\n" + message;
        }

        Debug.Log(message);
    }
}