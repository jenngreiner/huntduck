using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public static int playerScore = 0;
    public static int duckKills = 0;

    // create a callback to hook to be able to update score in ScoreUI
    public delegate void ScoreUpdate();
    public static event ScoreUpdate onScoreUpdate;

    public void UpdatePlayerScore(int points)
    {
        playerScore += points;
        Debug.Log("Player just gained " + points + " points! Player score is now " + playerScore);

        duckKills++;
        Debug.Log("Player killed another duck! Player duck kill total is " + duckKills);

        if (onScoreUpdate != null)
        {
            onScoreUpdate();
        }
    }
}