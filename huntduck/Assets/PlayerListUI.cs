using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListUI : MonoBehaviour
{
    public Text playerListText;

    void OnEnable()
    {
        // Subscribe to Photon events
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    void OnDisable()
    {
        // Unsubscribe from Photon events
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    private void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == 1)
        {
            Debug.Log("Executing event: " + photonEvent.CustomData);

            // Execute the function
            UpdatePlayerListUI();
        }
    }

    void UpdatePlayerListUI()
    {
        // Clear the current player list
        playerListText.text = "";

        // Add the current player count to the player list
        playerListText.text += "Total Players: <color=orange>" + PhotonNetwork.PlayerList.Length + "</color>\n";

        // Add each player's nickname to the player list
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playerListText.text += "<color=orange>" + player.NickName + "</color>\n";
        }
    }
}
