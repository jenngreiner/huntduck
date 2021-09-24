using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DuckShotUI : MonoBehaviour
{

    private const string PLAYER_TAG = "Player";
    private PlayerScore playerScoreScript;

    public Text hitsText;
    public static string duckScore;

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
        // update duck ui when player kills a duck
        PlayerScore.onScoreUpdate += UpdateScoreUI;
    }

    // unsubscribe events
    private void OnDisable()
    {
        PlayerScore.onScoreUpdate -= UpdateScoreUI;
    }

    void UpdateScoreUI()
    {
        CreateDuckScore();
        hitsText.text = duckScore;
    }

    void CreateDuckScore()
    {
        string _scoreAsString = PlayerScore.duckKills.ToString();
        duckScore = _scoreAsString;
    }
    

}
