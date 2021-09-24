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
        InfiniteWaveSpawner.onWaveChange += UpdateWaveUI;
        InfiniteWaveSpawner.onWaveCompleted += UpdateWaveUI;

    }

    // unsubscribe events
    private void OnDisable()
    {
        InfiniteWaveSpawner.onWaveChange -= UpdateWaveUI;
        InfiniteWaveSpawner.onWaveCompleted -= UpdateWaveUI;
    }

    void UpdateWaveUI()
    {
        CreateWaveCount();
        CreateWaveTime();
        waveCountText.text = waveCount;
        waveTimeText.text = waveTime;
    }

    void CreateWaveCount()
    {
        //string _waveCount = InfiniteWaveSpawner.currentWaveNumber.ToString();
        //waveCount = _waveCount;
        waveCount = InfiniteWaveSpawner.currentWaveNumber.ToString();
    }

    void CreateWaveTime()
    {
        // capture the time for the current wave in a way we can convert and set to UI text
        // set waveTime
        waveTime = InfiniteWaveSpawner.timerSeconds.Seconds.ToString();
    }


}
