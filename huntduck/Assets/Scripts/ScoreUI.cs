using UnityEngine;
using UnityEngine.UI;

// this is a single player score implementation
public class ScoreUI : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";
    private PlayerScore playerScoreScript;

    public Text scoreText;
    private int scoreLength = 6;
    private string arcadeScore;

    void Start()
    {
        //// find player in the scene, grab its PlayerScore script
        //playerScoreScript = GameObject.FindGameObjectWithTag(PLAYER_TAG).GetComponent<PlayerScore>();
        playerScoreScript = GetComponent<PlayerScore>();

        // reset score when game starts
        CreateArcadeScore();
        scoreText.text = arcadeScore;
        Debug.Log("Score UI is now reset to " + arcadeScore);

        //scoreText.text = playerScoreScript.playerScore.ToString();
        //Debug.Log("Score UI is now reset to " + playerScoreScript.playerScore);
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
        // implementation with leading zeros to give arcade feel
        CreateArcadeScore();
        scoreText.text = arcadeScore;

        // implementation without leading zeros
        // scoreText.text = playerScoreScript.playerScore.ToString();
    }

    void CreateArcadeScore()
    {
        string _scoreAsString = playerScoreScript.playerScore.ToString();
        int numZeros = scoreLength - _scoreAsString.Length;

        arcadeScore = "";
        for (int i = 0; i < numZeros; i++)
        {
            arcadeScore += "0";
        }
        arcadeScore += _scoreAsString;
    }
}
