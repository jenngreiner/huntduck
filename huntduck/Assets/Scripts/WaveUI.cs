using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    private SurvivalWaveSpawner iWS;

    public Text waveCountText;
    public static string waveCount;
    public Text waveTimeText;
    public static string waveTime;

    void Start()
    {
        iWS = FindObjectOfType<SurvivalWaveSpawner>();
    }

    private void OnEnable()
    {
        SurvivalWaveSpawner.onTimeChange += UpdateWaveUI;
        SurvivalWaveSpawner.onWaveChange += UpdateWaveUI;
    }

    private void OnDisable()
    {
        SurvivalWaveSpawner.onTimeChange -= UpdateWaveUI;
        SurvivalWaveSpawner.onWaveChange -= UpdateWaveUI;
    }

    void UpdateWaveUI()
    {
        waveCountText.text = iWS.currentWaveNumber.ToString();
        waveTimeText.text = iWS.currentWaveMinutes + ":" + iWS.currentWaveSeconds;
    }
}
