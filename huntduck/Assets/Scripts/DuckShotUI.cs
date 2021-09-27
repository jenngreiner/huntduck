using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DuckShotUI : MonoBehaviour
{

    private const string PLAYER_TAG = "Player";
    private PlayerScore playerScoreScript;

    public Text waveDucksLeftText;

    public static string waveDucksLeft;
    
    void Start()
    {
        // get the playerscore script on player object
        playerScoreScript = GameObject.FindGameObjectWithTag(PLAYER_TAG).GetComponent<PlayerScore>();
    }

    // subscribe events
    private void OnEnable()
    {
        // update duck ui when player kills a duck
        //PlayerScore.onScoreUpdate += UpdateDucksUI;
        InfiniteWaveSpawner.onWaveChange += UpdateDucksUI;
        InfiniteWaveSpawner.onDuckHit += UpdateDucksUI;
    }

    // unsubscribe events
    private void OnDisable()
    {
        //PlayerScore.onScoreUpdate -= UpdateDucksUI;
        InfiniteWaveSpawner.onWaveChange -= UpdateDucksUI;
        InfiniteWaveSpawner.onDuckHit -= UpdateDucksUI;
    }

    void UpdateDucksUI()
    {
        CountDucksLeft();
        waveDucksLeftText.text = waveDucksLeft;
        //totalDucksHitText.text = totalDucksHit;
    }

    void CountDucksLeft()
    {
        //waveDucksHit = InfiniteWaveSpawner.ducksHitThisWave.ToString();
        //totalDucksHit = InfiniteWaveSpawner.ducksHitTotal.ToString();
        waveDucksLeft = InfiniteWaveSpawner.ducksLeft.ToString();
    }
}