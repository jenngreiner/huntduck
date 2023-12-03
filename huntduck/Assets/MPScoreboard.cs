using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MPScoreboard : MonoBehaviourPunCallbacks
{
    public GameObject scoreboardUI; // scoreboard gameobject
    public Text scoreboardText;     // text for displaying scores


    void Start()
    {
        if (scoreboardUI != null)
            scoreboardUI.SetActive(true);
        UpdateScoreboard();
    }

    private void UpdateScoreboard()
    {
        string scoreboardString = "";
        foreach (var player in PhotonNetwork.PlayerList)
        {
            int score = (int)player.CustomProperties["score"];
            scoreboardString += player.NickName + ": " + score + "\n";
            Debug.Log(scoreboardString);
        }

        scoreboardText.text = scoreboardString;
        Debug.Log("Scoreboard updated");
    }

    public void UpdatePlayerScore(Player player, int score)
    {
        ExitGames.Client.Photon.Hashtable scoreProperty = new ExitGames.Client.Photon.Hashtable();
        scoreProperty["score"] = score;
        player.SetCustomProperties(scoreProperty);
        Debug.Log("Player score set to " + score);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        if (changedProps.ContainsKey("score"))
        {
            UpdateScoreboard();
        }
    }
}
