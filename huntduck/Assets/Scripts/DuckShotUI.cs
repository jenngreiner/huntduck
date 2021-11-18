using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DuckShotUI : MonoBehaviour
{
    private InfiniteWaveSpawner infiniteWaveSpawnerScript;

    public Text waveDucksLeftText;
    public static string waveDucksLeft;

    void Start()
    {
        infiniteWaveSpawnerScript = FindObjectOfType<InfiniteWaveSpawner>();
    }

    void OnEnable()
    {
        InfiniteWaveSpawner.onWaveChange += UpdateDucksUI;
        InfiniteWaveSpawner.onDuckHit += UpdateDucksUI;
    }

    void OnDisable()
    {
        InfiniteWaveSpawner.onWaveChange -= UpdateDucksUI;
        InfiniteWaveSpawner.onDuckHit -= UpdateDucksUI;
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