using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public int playerScore = 0;
    public int duckKills = 0;

    public delegate void ScoreUpdate();
    public static event ScoreUpdate onScoreUpdate;

    void OnEnable()
    {
        Duck.onDuckDied += UpdatePlayerScore;
        InfiniteLevelManager.onStartInfinite += ResetPlayerScore; // SINGLESCENE: ResetScore subscribe
    }

    void OnDisable()
    {
        Duck.onDuckDied -= UpdatePlayerScore;
        InfiniteLevelManager.onStartInfinite -= ResetPlayerScore; // SINGLESCENE: ResetScore unsubscribe
    }

    public void UpdatePlayerScore(int points)
    {
        playerScore += points;
        Debug.Log("Player just gained " + points + " points! Player score is now " + playerScore);

        duckKills++;
        Debug.Log("Player killed another duck! Player duck kill total is " + duckKills);
        
        onScoreUpdate?.Invoke(); // update score in ScoreUI.cs
    }

    // SINGLESCENE: reset the player score via "Play Again" or "Quit" buttons in Hunt Mode
    public void ResetPlayerScore()
    {
        playerScore = 0;
        duckKills = 0;
        onScoreUpdate?.Invoke();
    }
}