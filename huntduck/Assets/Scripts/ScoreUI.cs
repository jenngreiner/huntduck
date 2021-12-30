using UnityEngine;
using UnityEngine.UI;

// this is a single player score implementation
public class ScoreUI : MonoBehaviour
{
    private PlayerScore playerScoreScript;

    public Text scoreText;
    private int scoreLength = 6;
    private string arcadeScore;

    void Start()
    {
        //// find player in the scene, grab its PlayerScore script
        playerScoreScript = GameObject.FindGameObjectWithTag(TagManager.PLAYER_TAG).GetComponent<PlayerScore>();

        // reset score when game starts
        CreateArcadeScore();
        scoreText.text = arcadeScore;
        Debug.Log("Score UI is now reset to " + arcadeScore);
    }

    void OnEnable()
    {
        // update score ui when the player's score changes
        PlayerScore.onScoreUpdate += UpdateScoreUI;
    }

    void OnDisable()
    {
        PlayerScore.onScoreUpdate -= UpdateScoreUI;
    }

    void UpdateScoreUI()
    {
        // implementation with leading zeros to give arcade feel
        CreateArcadeScore();
        scoreText.text = arcadeScore;
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
