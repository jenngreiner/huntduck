using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public int playerScore = 0;
    public int duckKills = 0;

    // create a callback to hook to be able to update score in ScoreUI
    public delegate void ScoreUpdate();
    public static event ScoreUpdate onScoreUpdate;

    void OnEnable()
    {
        InfiniteLevelManager.onStartInfinite += ResetPlayerScore; // SINGLESCENE: ResetScore subscribe
    }

    void OnDisable()
    {
        InfiniteLevelManager.onStartInfinite -= ResetPlayerScore; // SINGLESCENE: ResetScore unsubscribe
    }

    public void UpdatePlayerScore(int points)
    {
        playerScore += points;
        Debug.Log("Player just gained " + points + " points! Player score is now " + playerScore);

        duckKills++;
        Debug.Log("Player killed another duck! Player duck kill total is " + duckKills);
        
        onScoreUpdate?.Invoke();
    }

    // SINGLESCENE: reset the player score via "Play Again" or "Quit" buttons in Hunt Mode
    public void ResetPlayerScore()
    {
        playerScore = 0;
        duckKills = 0;
        onScoreUpdate?.Invoke();
    }
}