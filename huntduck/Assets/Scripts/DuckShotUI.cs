using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DuckShotUI : MonoBehaviour
{
    private SurvivalWaveSpawner infiniteWaveSpawnerScript;

    public Text waveDucksLeftText;
    public static string waveDucksLeft;

    void Start()
    {
        infiniteWaveSpawnerScript = FindObjectOfType<SurvivalWaveSpawner>();
    }

    void OnEnable()
    {
        SurvivalWaveSpawner.onWaveChange += UpdateDucksUI;
        SurvivalWaveSpawner.onDuckHit += UpdateDucksUI;
    }

    void OnDisable()
    {
        SurvivalWaveSpawner.onWaveChange -= UpdateDucksUI;
        SurvivalWaveSpawner.onDuckHit -= UpdateDucksUI;
    }

    void UpdateDucksUI()
    {
        CountDucksLeft();
        waveDucksLeftText.text = waveDucksLeft;
    }

    void CountDucksLeft()
    {
        waveDucksLeft = infiniteWaveSpawnerScript.ducksLeft.ToString();
    }
}