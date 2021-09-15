using UnityEngine;
using UnityEngine.UI;

public class WalletUI : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";
    private PlayerScore playerScoreScript;

    public Text dollarsText;
    public static string walletScore;

    void Start()
    {
        // get the playerscore script on player object
        playerScoreScript = GameObject.FindGameObjectWithTag(PLAYER_TAG).GetComponent<PlayerScore>();

        // reset score when game starts
        UpdateScoreUI();
        dollarsText.text = walletScore;
    }

    // subscribe events
    private void OnEnable()
    {
        // update score ui when the player's score changes
        PlayerScore.onScoreUpdate += UpdateScoreUI;
    }

    // unsubscribe events
    private void OnDisable()
    {
        PlayerScore.onScoreUpdate -= UpdateScoreUI;
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
