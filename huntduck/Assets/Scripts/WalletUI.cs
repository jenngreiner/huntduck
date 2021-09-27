using UnityEngine;
using UnityEngine.UI;

public class WalletUI : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";
    private PlayerScore playerScoreScript;

    public Text dollarsText;
    public static string walletScore;

    public Text totalDucksHitText;
    public static string totalDucksHit;


    void Start()
    {
        // get the playerscore script on player object
        playerScoreScript = GameObject.FindGameObjectWithTag(PLAYER_TAG).GetComponent<PlayerScore>();

        // reset score when game starts
        UpdateScoreUI();
    }

    // subscribe events
    private void OnEnable()
    {
        // update score ui when the player's score changes
        PlayerScore.onScoreUpdate += UpdateScoreUI;
        InfiniteWaveSpawner.onDuckHit +=  UpdateScoreUI;
    }

    // unsubscribe events
    private void OnDisable()
    {
        PlayerScore.onScoreUpdate -= UpdateScoreUI;
        InfiniteWaveSpawner.onDuckHit -= UpdateScoreUI;
    }

    void UpdateScoreUI()
    {
        CreateWalletScore();
        dollarsText.text = walletScore;
        totalDucksHitText.text = totalDucksHit;
    }

    void CreateWalletScore()
    {
        string _scoreAsString = PlayerScore.playerScore.ToString();
        walletScore = "$";
        walletScore += _scoreAsString;
        totalDucksHit = InfiniteWaveSpawner.ducksHitTotal.ToString();
    }
}