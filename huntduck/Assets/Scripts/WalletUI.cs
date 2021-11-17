using UnityEngine;
using UnityEngine.UI;

public class WalletUI : MonoBehaviour
{
    public PlayerScore playerScoreScript;

    public Text dollarsText;
    public static string walletScore;

    void Start()
    {
        // reset score when game starts
        UpdateScoreUI();
    }

    void OnEnable()
    {
        // update score ui when the player's score changes
        PlayerScore.onScoreUpdate += UpdateScoreUI;
        InfiniteWaveSpawner.onDuckHit +=  UpdateScoreUI;
    }

    void OnDisable()
    {
        PlayerScore.onScoreUpdate -= UpdateScoreUI;
        InfiniteWaveSpawner.onDuckHit -= UpdateScoreUI;
    }

    void UpdateScoreUI()
    {
        CreateWalletScore();
        dollarsText.text = walletScore;
    }

    void CreateWalletScore()
    {
        string _scoreAsString = playerScoreScript.playerScore.ToString();
        walletScore = "$";
        walletScore += _scoreAsString;
    }
}