using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{

    private const string PLAYER_TAG = "Player";
    private PlayerScore playerScoreScript;

    public Text waveCountText;
    public static string waveCount;
    public Text waveTimeText;
    public static string waveTime;

    void Start()
    {
        // get the playerscore script on player object
        playerScoreScript = GameObject.FindGameObjectWithTag(PLAYER_TAG).GetComponent<PlayerScore>();


        // start on wave 1
        UpdateWaveUI();

    }

    void Update()
    {
        UpdateWaveUI();
    }

    // subscribe events
    private void OnEnable()
    {
        // update duck ui when new wave starts
        InfiniteWaveSpawner.onTimeChange += UpdateWaveUI;
        InfiniteWaveSpawner.onWaveChange += UpdateWaveUI;
        InfiniteWaveSpawner.onWaveCompleted += UpdateWaveUI;

    }

    // unsubscribe events
    private void OnDisable()
    {
        InfiniteWaveSpawner.onTimeChange -= UpdateWaveUI;
        InfiniteWaveSpawner.onWaveChange -= UpdateWaveUI;
        InfiniteWaveSpawner.onWaveCompleted -= UpdateWaveUI;
    }

    void UpdateWaveUI()
    {
        waveCountText.text = InfiniteWaveSpawner.currentWaveNumber.ToString();
        waveTimeText.text = InfiniteWaveSpawner.currentWaveTime.ToString();
    }
}
