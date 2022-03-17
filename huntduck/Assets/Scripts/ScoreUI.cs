using UnityEngine;
using UnityEngine.UI;
using huntduck;

// this is a single player score implementation
public class ScoreUI : MonoBehaviour
{
    private Player player;

    public Text scoreText;
    private int scoreLength = 6;
    private string arcadeScore;

    void Start()
    {
        //// find player in the scene, grab its PlayerScore script
        player = ObjectManager.instance.player;

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
        string _scoreAsString = player.score.ToString();
        int numZeros = scoreLength - _scoreAsString.Length;

        arcadeScore = "";
        for (int i = 0; i < numZeros; i++)
        {
            arcadeScore += "0";
        }
        arcadeScore += _scoreAsString;
    }
}
