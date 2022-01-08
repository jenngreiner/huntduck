using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    private InfiniteWaveSpawner iWS;

    public Text waveCountText;
    public static string waveCount;
    public Text waveTimeText;
    public static string waveTime;

    void Start()
    {
        iWS = FindObjectOfType<InfiniteWaveSpawner>();
    }

    private void OnEnable()
    {
        InfiniteWaveSpawner.onTimeChange += UpdateWaveUI;
        InfiniteWaveSpawner.onWaveChange += UpdateWaveUI;
    }

    private void OnDisable()
    {
        InfiniteWaveSpawner.onTimeChange -= UpdateWaveUI;
        InfiniteWaveSpawner.onWaveChange -= UpdateWaveUI;
    }

    void UpdateWaveUI()
    {
        waveCountText.text = iWS.currentWaveNumber.ToString();
        waveTimeText.text = iWS.currentWaveMinutes + ":" + iWS.currentWaveSeconds;
    }
}
