using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    private InfiniteWaveSpawner infiniteWaveSpawnerScript;

    public Text waveCountText;
    public static string waveCount;
    public Text waveTimeText;
    public static string waveTime;

    void Start()
    {
        infiniteWaveSpawnerScript = FindObjectOfType<InfiniteWaveSpawner>();
    }

    private void OnEnable()
    {
        InfiniteWaveSpawner.onTimeChange += UpdateWaveUI;
        InfiniteWaveSpawner.onWaveChange += UpdateWaveUI;
        InfiniteWaveSpawner.onWaveCompleted += UpdateWaveUI;
    }

    private void OnDisable()
    {
        InfiniteWaveSpawner.onTimeChange -= UpdateWaveUI;
        InfiniteWaveSpawner.onWaveChange -= UpdateWaveUI;
        InfiniteWaveSpawner.onWaveCompleted -= UpdateWaveUI;
    }

    void UpdateWaveUI()
    {
        waveCountText.text = infiniteWaveSpawnerScript.currentWaveNumber.ToString();
        waveTimeText.text = infiniteWaveSpawnerScript.currentWaveTime.ToString();
    }
}
