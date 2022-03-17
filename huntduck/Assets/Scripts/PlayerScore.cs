using UnityEngine;
using huntduck;

public class PlayerScore : MonoBehaviour
{
    private Player player;

    public delegate void ScoreUpdate();
    public static event ScoreUpdate onScoreUpdate;

    void Start()
    {
        player = ObjectManager.instance.player;
    }

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
        player.score += points;
        Debug.Log("Player just gained " + points + " points! Player score is now " + player.score);

        player.duckKills++;
        Debug.Log("Player killed another duck! Player duck kill total is " + player.duckKills);
        
        onScoreUpdate?.Invoke(); // update score in ScoreUI.cs & WalletUI
    }

    // SINGLESCENE: reset the player score via "Play Again" or "Quit" buttons in Hunt Mode
    public void ResetPlayerScore()
    {
        player.score = 0;
        player.duckKills = 0;
        onScoreUpdate?.Invoke();
    }
}