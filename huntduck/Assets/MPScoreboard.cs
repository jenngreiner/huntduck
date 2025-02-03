using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using ExitGames.Client.Photon;

//TODO: Currently an error when leaving and rejoining the room, scoreboard goes up for both payers. test again, if still an issue, consider adding back in OnCreatedRoom and OnJoinedRoom callbacks 
public class MPScoreboard : MonoBehaviourPunCallbacks
{
    public Text scoreboardText;

    void Start()
    {
        UpdateScoreboard();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        UpdateScoreboard();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        if (changedProps.ContainsKey("score"))
        {
            //make sure only one player is having score changed
            Debug.Log($"Score updated for {targetPlayer.NickName}: {changedProps["score"]}");

            UpdateScoreboard();
        }
    }

    public void UpdateScoreboard()
    {
        if (scoreboardText == null)
        {
            Debug.LogError("Scoreboard Text component is not assigned!");
            return;
        }

        string scoreboardString = "";
        
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            //int score = player.CustomProperties.TryGetValue("score", out object scoreObj) ? (int)scoreObj : 0;

            int score = 0;
            if (player.CustomProperties.TryGetValue("score", out object scoreObj) && scoreObj is int validScore)
            {
                score = validScore;
            }

            scoreboardString += "<color=orange>" + player.NickName + ": </color>" + score + "\n";
            Debug.Log(scoreboardString);
        }

        scoreboardText.text = scoreboardString;
    }
}
