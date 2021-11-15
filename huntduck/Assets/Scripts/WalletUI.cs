using UnityEngine;
using UnityEngine.UI;

public class WalletUI : MonoBehaviour
{
    public PlayerScore playerScoreScript;
    private InfiniteWaveSpawner infiniteWaveSpawnerScript;

    public Text dollarsText;
    public static string walletScore;

    //public Text totalDucksHitText;
    //private string totalDucksHit;


    void Start()
    {
        // get the playerscore script on player object
        //playerScoreScript = GameObject.FindGameObjectWithTag(TagManager.PLAYER).GetComponent<PlayerScore>();

        // don't you change the name of InfiniteWaveManager yo!
        infiniteWaveSpawnerScript = GameObject.Find("InfiniteWaveManager").GetComponent<InfiniteWaveSpawner>();

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
        //totalDucksHitText.text = totalDucksHit;
    }

    void CreateWalletScore()
    {
        string _scoreAsString = playerScoreScript.playerScore.ToString();
        walletScore = "$";
        walletScore += _scoreAsString;
        //totalDucksHit = infiniteWaveSpawnerScript.ducksHitTotal.ToString();
    }
}