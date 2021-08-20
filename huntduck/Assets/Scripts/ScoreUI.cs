using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    // single player implementation
    private PlayerScore playerScoreScript;
    private const string PLAYER_TAG = "Player";

    public Text scoreText;

    void Start()
    {
        // report player name and score on startup
        playerScoreScript = GameObject.FindGameObjectWithTag(PLAYER_TAG).GetComponent<PlayerScore>();
        Debug.Log(this.GetType().Name + " says that Player's name is " + playerScoreScript.transform.name + " with a score of " + playerScoreScript.playerScore);

        // reset score when game starts
        scoreText.text = playerScoreScript.playerScore.ToString();
        Debug.Log("Score UI is now reset");
    }

    // subscribe events
    private void OnEnable()
    {
        // update points text when the player's score changes
        PlayerScore.onScoreUpdate += UpdatePoints;
    }

    // unsubscribe events
    private void OnDisable()
    {
        PlayerScore.onScoreUpdate -= UpdatePoints;
    }

    void UpdatePoints()
    {
        scoreText.text = playerScoreScript.playerScore.ToString();
    }

  
}
