using UnityEngine;
using huntduck;

[RequireComponent(typeof(PlayerData))]
public class PlayerScore : MonoBehaviour
{
    private PlayerData playerData;

    public delegate void ScoreUpdate();
    public static event ScoreUpdate onScoreUpdate;

    void Start()
    {
        playerData = GetComponent<PlayerData>();
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
        playerData.score += points;
        Debug.Log("Player just gained " + points + " points! Player score is now " + playerData.score);

        playerData.duckKills++;
        Debug.Log("Player killed another duck! Player duck kill total is " + playerData.duckKills);
        
        onScoreUpdate?.Invoke(); // update score in ScoreUI.cs & WalletUI
    }

    // SINGLESCENE: reset the player score via "Play Again" or "Quit" buttons in Hunt Mode
    public void ResetPlayerScore()
    {
        playerData.score = 0;
        playerData.duckKills = 0;
        onScoreUpdate?.Invoke();
    }
}