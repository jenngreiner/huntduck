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
        public int ducksHitThisWave = 0;
        public float waveTime = 60f;
        public int duckTotal = 1000;

        public InfiniteWave(int newWaveNumber, int newDuckCount, float newRate, float newWaveTime, int newDuckTotal)
        {
            waveNumber = newWaveNumber;
            duckCount = newDuckCount;
            rate = newRate;
            waveTime = newWaveTime;
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
        SetupWave();
    }

    void Update()
    {
        // start next wave, as long as not already spawning
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
                return;
            }

            if (waveTimeRemaining > 0)
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
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawnpoints referenced");
        }

        nextWave = 0;
        currentWaveNumber = waves[nextWave].waveNumber;
        currentWaveTime = (int)waves[nextWave].waveTime;
        ducksLeft = waves[nextWave].duckCount;

        // call out an event: hey subs, I changed my wave, do what you will man
        if (onWaveChange != null)
        {
            onWaveChange();
        }
    }

        void WaveCompleted()
    {
        state = WaveState.ENDING;

        // infinite waves are over
        if (!playerBeatWave())
        {
            Debug.Log("Infinite waves are over");
            if (onGameOver != null)
            {
                onGameOver();
            }
            
        }
        else
        {
            waves.Add(new InfiniteWave(waves[nextWave].waveNumber + 1, waves[nextWave].duckCount * 2, waves[nextWave].rate * 1.05f, waves[nextWave].waveTime, waves[nextWave].duckTotal));
            nextWave++;
            currentWaveNumber = waves[nextWave].waveNumber;
            currentWaveTime = (int)waves[nextWave].waveTime;
            ducksLeft = waves[nextWave].duckCount;
            waves[nextWave].ducksHitThisWave = 0;

            if (onWaveCompleted != null)
            {
                onWaveCompleted();
            }

            state = WaveState.READY;
        }
    }

    IEnumerator StartWave(InfiniteWave _thisWave)
    {
        state = WaveState.STARTING;

        // reset wave time
        waveTimeRemaining = waves[nextWave].waveTime;

        if (onWaveChange != null)
        {
            onWaveChange();
        }

        // set wave number & show it
        waveCountText.text = waves[nextWave].waveNumber.ToString();
        waveCountUI.SetActive(true);
        yield return new WaitForSecondsRealtime(timeDelay);
        waveCountUI.SetActive(false);

        // "Get Ready"
        getReadyUI.SetActive(true);
        yield return new WaitForSecondsRealtime(timeDelay);
        getReadyUI.SetActive(false);

        // loop through the amount of ducks you want to spawn
        for (int i = 0; i < _thisWave.duckTotal; i++)
        {
            state = WaveState.WAVING;

            if (playerBeatWave())
            {
                state = WaveState.ENDING;
                _thisWave.duckTotal = 0;
                // stop spawning ducks
                break;
            }

            SpawnDuck();
            yield return new WaitForSeconds(1 / _thisWave.rate);
        }

        yield break;
    }

    void Timer()
    {
        // decrement waveTimeRemaining once per second
        waveTimeRemaining -= Time.deltaTime;

        // update timerSeconds to waveTimeRemaining
        timerSeconds = TimeSpan.FromSeconds(waveTimeRemaining);
        currentWaveTime = timerSeconds.Seconds;

        if (onTimeChange != null)
        {
            onTimeChange();
        }

        // Debug.Log seconds remaining in wave (wave timer)
        Debug.Log("Wave " + waves[nextWave].waveNumber + " time remaining: " + timerSeconds.Seconds);
    }

    bool isTimeLeft()
    {
        if (waveTimeRemaining >= 0)
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
        if (ducksLeft == 0)
        {
            return true;
        }
        return false;
    }

    public void increaseDuckHitCount()
    {
        ducksHitTotal++;
        waves[nextWave].ducksHitThisWave++;

        if (ducksLeft > 0)
        {
            ducksLeft--;
        }

        onDuckHit();
    }

    void SpawnDuck()
    {
        // Spawn Duck
        GameObject activeSpawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        activeSpawnPoint.GetComponent<ObjectLauncher>().ShootLauncher();
    }
}