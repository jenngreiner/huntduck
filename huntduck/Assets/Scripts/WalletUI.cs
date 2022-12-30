using UnityEngine;
using UnityEngine.UI;
using huntduck;

public class WalletUI : MonoBehaviour
{
    private PlayerData playerData;

    public Text dollarsText;
    public static string walletScore;

    void Start()
    {
        playerData = ObjectManager.instance.player.GetComponent<PlayerData>();
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
        string _scoreAsString = playerData.score.ToString();
        walletScore = "$";
        walletScore += _scoreAsString;
    }
}