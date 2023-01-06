using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class PlayerList : MonoBehaviourPunCallbacks
{
    // Assume that playerListText is a public Text variable that refers to a UI Text component
    public Text networkLogs;
    public Text playerListText;

    void Start()
    {
        UpdatePlayerList();
    }



    public override void OnPlayerEnteredRoom(Player player)
    {
        UpdatePlayerList();
        //LogText("Joined room: <color=acqua>" + NetworkManagerHD2.roomName + "</color>");
    }

    public override void OnPlayerLeftRoom(Player player)
    {
        UpdatePlayerList();
    }

    void UpdatePlayerList()
    {
        //// Clear the player list text
        //playerListText.text = "";

        //// Add each player's name to the player list text
        //foreach (Player player in PhotonNetwork.PlayerList)
        //{
        //    playerListText.text += player.NickName + "\n";
        //}

        // Clear the current player list
        playerListText.text = "";

        // Add the current player count to the player list
        playerListText.text += "Total Players: <color=orange>" + PhotonNetwork.CurrentRoom.PlayerCount + "</color>\n";

        // Add each player's nickname to the player list
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playerListText.text += "<color=orange>" + player.NickName + "</color>\n";
        }
    }

    void LogText(string message)
    {
        // Output to worldspace to help with debugging.
        if (networkLogs)
        {
            networkLogs.text += "\n" + message;
        }

        //Debug.Log(message);
    }
}