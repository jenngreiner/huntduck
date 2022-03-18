using UnityEngine;
using huntduck;

[RequireComponent(typeof(Player))]
public class PlayerScore : MonoBehaviour
{
    private Player player;

    public delegate void ScoreUpdate();
    public static event ScoreUpdate onScoreUpdate;

    void Start()
    {
        player = GetComponent<Player>();
    }

    void OnEnable()
    {
        Duck.onDuckDied += UpdatePlayerScore;
        SurvivalWaveSpawner.onSurvivalWaveNoDamage += UpdatePlayerScore;
        InfiniteLevelManager.onStartInfinite += ResetPlayerScore; // SINGLESCENE: ResetScore subscribe
    }

    void OnDisable()
    {
        Duck.onDuckDied -= UpdatePlayerScore;
        SurvivalWaveSpawner.onSurvivalWaveNoDamage -= UpdatePlayerScore;
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