using UnityEngine;
using UnityEngine.UI;
using huntduck;

public class WalletUI : MonoBehaviour
{
    private Player player;

    public Text dollarsText;
    public static string walletScore;

    void Start()
    {
        player = ObjectManager.instance.player;
        // reset score when game starts
        UpdateScoreUI();
    }

    void OnEnable()
    {
        // update score ui when the player's score changes
        PlayerScore.onScoreUpdate += UpdateScoreUI;
        SurvivalWaveSpawner.onDuckHit +=  UpdateScoreUI;
    }

    void OnDisable()
    {
        PlayerScore.onScoreUpdate -= UpdateScoreUI;
        SurvivalWaveSpawner.onDuckHit -= UpdateScoreUI;
    }

    void UpdateScoreUI()
    {
        CreateWalletScore();
        dollarsText.text = walletScore;
    }

    void CreateWalletScore()
    {
        string _scoreAsString = ObjectManager.instance.player.score.ToString();
        walletScore = "$";
        walletScore += _scoreAsString;
    }
}