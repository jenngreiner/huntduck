using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public int playerScore = 0;
    public int duckKills = 0;

    // create a callback to hook to be able to update score
    public delegate void ScoreUpdate();
    public static event ScoreUpdate onScoreUpdate;

    void Awake()
    {
        Debug.Log(transform.name + " started with a score of " + playerScore);
        Debug.Log(transform.name + " started with " + duckKills + " duck kills");
    }

    public void PlayerScores(int points)
    {
        playerScore += points;
        Debug.Log("Player just gained " + points + " points! Player score is now" + playerScore);

        duckKills++;
        Debug.Log("Player killed another duck! Player duck kill total is " + duckKills);

        onScoreUpdate();
    }
}
