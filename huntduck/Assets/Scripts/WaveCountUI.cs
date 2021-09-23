using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveCountUI : MonoBehaviour
{

    private const string PLAYER_TAG = "Player";
    private PlayerScore playerScoreScript;

    public Text waveText;
    public static string waveCount;

    void Start()
    {
        // get the playerscore script on player object
        playerScoreScript = GameObject.FindGameObjectWithTag(PLAYER_TAG).GetComponent<PlayerScore>();


        // start on wave 1
        waveText.text = "1";

    }

    // subscribe events
    private void OnEnable()
    {
        // update duck ui when new wave starts
        InfiniteWaveSpawner.onWaveCompleted += UpdateWaveUI;
    }

    // unsubscribe events
    private void OnDisable()
    {
        InfiniteWaveSpawner.onWaveCompleted -= UpdateWaveUI;
    }

    void UpdateWaveUI()
    {
        CreateWaveCount();
        waveText.text = waveCount;
    }

    void CreateWaveCount()
    {
        string _scoreAsString = InfiniteWaveSpawner.currentWave.ToString();
        waveCount = _scoreAsString;
    }


}
