using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteWaveSpawner : MonoBehaviour
{
    public enum WaveState { READY, STARTING, WAVING, ENDING };
    private WaveState state = WaveState.READY;

    // makes the Wave class fields editable from Inspector
    [System.Serializable]
    public class InfiniteWave
    {
        public int waveNumber;
        public int duckCount;
        public float rate;
        public float waveTime = 60f;
        public int ducksHitThisWave = 0;
        public int duckTotal = 1000;

        public InfiniteWave(int newWaveNumber, int newDuckCount, float newRate, float newWaveTime, int newDucksHitThisWave, int newDuckTotal)
        {
            waveNumber = newWaveNumber;
            duckCount = newDuckCount;
            rate = newRate;
            waveTime = newWaveTime;
            ducksHitThisWave = newDucksHitThisWave;
            duckTotal = newDuckTotal;
        }
    }

    public List<InfiniteWave> waves;
    private int nextWave;
    public static int ducksHitTotal = 0;
    //public static int ducksHitThisWave;
    public static int ducksLeft;

    private float waveTimeRemaining;
    public float timeDelay = 1f;
    public static int currentWaveNumber;
    public static int currentWaveTime;

    public GameObject[] spawnPoints;

    public delegate void OnTimeChange();
    public static event OnTimeChange onTimeChange;

    public delegate void OnDuckHit();
    public static event OnDuckHit onDuckHit;

    public delegate void OnWaveCompleted();
    public static event OnWaveCompleted onWaveCompleted;

    public delegate void OnWaveChange();
    public static event OnWaveChange onWaveChange;

    public delegate void gameOver();
    public static event gameOver onGameOver;

    public static TimeSpan timerSeconds;
    public GameObject waveCountUI;
    public Text waveCountText;
    public GameObject getReadyUI;


    void Start()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawnpoints referenced");
        }

        nextWave = 0;

        SetupWave();
        // note: we are in READY state, so first Frame after start will StartWave
    }

    void Update()
    {
        if (state == WaveState.READY)
        {
            StartCoroutine(StartWave(waves[nextWave]));
        }

        if (state == WaveState.WAVING)
        {
            // we hit all ducks this wave, or ran out of time
            if (playerBeatWave() || !isTimeLeft())
            {
                WaveCompleted();
            }
            else
            {
                Timer();
            }
        }
    }

    void OnEnable()
    {
        BNG.Damageable.onInfiniteDuckHit += increaseDuckHitCount;
    }

    void OnDisable()
    {
        BNG.Damageable.onInfiniteDuckHit -= increaseDuckHitCount;
    }

    void SetupWave()
    {
        waveTimeRemaining = waves[nextWave].waveTime;
        currentWaveTime = (int)waveTimeRemaining;
        currentWaveNumber = waves[nextWave].waveNumber;
        waves[nextWave].ducksHitThisWave = 0;
        ducksLeft = waves[nextWave].duckCount;

        // call out an event: hey subs, I changed my wave, do what you will man
        if (onWaveChange != null)
        {
            onWaveChange();
        }
    }

    IEnumerator StartWave(InfiniteWave _thisWave)
    {
        state = WaveState.STARTING;

        // set wave number & show it
        waveCountText.text = waves[nextWave].waveNumber.ToString();
        waveCountUI.SetActive(true);
        yield return new WaitForSecondsRealtime(timeDelay);
        waveCountUI.SetActive(false);

        // "Get Ready"
        getReadyUI.SetActive(true);
        yield return new WaitForSecondsRealtime(timeDelay);
        getReadyUI.SetActive(false);

        state = WaveState.WAVING;

        // loop through the amount of ducks you want to spawn
        for (int i = 0; i < _thisWave.duckTotal; i++)
        {
            SpawnDuck();
            yield return new WaitForSeconds(1 / _thisWave.rate);
        }

        yield break;
    }

    void WaveCompleted()
    {
        state = WaveState.ENDING;

        if (!playerBeatWave())
        {
            if (onGameOver != null)
            {
                onGameOver();
            }
            StopAllCoroutines(); // stop ducks flying
        }
        else
        {
            waves.Add(new InfiniteWave(waves[nextWave].waveNumber + 1, waves[nextWave].duckCount * 2, waves[nextWave].rate * 1.05f, waves[nextWave].waveTime, waves[nextWave].ducksHitThisWave = 0, waves[nextWave].duckTotal));
            nextWave++;
            SetupWave();

            state = WaveState.READY;
        }
    }

    void Timer()
    {
        // decrement waveTimeRemaining once per second
        waveTimeRemaining -= Time.deltaTime;

        // set currentWaveTime to display to user on UI
        timerSeconds = TimeSpan.FromSeconds(waveTimeRemaining);
        currentWaveTime = timerSeconds.Seconds;

        if (onTimeChange != null)
        {
            onTimeChange();
        }
    }

    bool isTimeLeft()
    {
        if (waveTimeRemaining > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool playerBeatWave()
    {
        if (ducksLeft <= 0)
        {
            StopAllCoroutines(); // stop ducks flying
            return true;
        }
        return false;
    }

    public void increaseDuckHitCount()
    {
        ducksHitTotal++;
        waves[nextWave].ducksHitThisWave++;

        ducksLeft--;

        onDuckHit();
    }

    void SpawnDuck()
    {
        GameObject activeSpawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        activeSpawnPoint.GetComponent<ObjectLauncher>().ShootLauncher();
    }
}