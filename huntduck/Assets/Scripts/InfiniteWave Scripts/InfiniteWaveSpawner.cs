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
    public int ducksHitTotal;
    //public static int ducksHitThisWave;
    public int ducksLeft;
    public float duckSpeed;

    private float waveTimeRemaining;
    public float timeDelay = 1f;
    public int currentWaveNumber;
    public int currentWaveTime;
    public string currentWaveMinutes;
    public string currentWaveSeconds;

    public GameObject[] spawnPoints;

    public delegate void OnTimeChange();
    public static event OnTimeChange onTimeChange;

    public delegate void OnDuckHit();
    public static event OnDuckHit onDuckHit;

    public delegate void OnWaveChange();
    public static event OnWaveChange onWaveChange;

    public delegate void gameOver();
    public static event gameOver onGameOver;

    public TimeSpan timerSeconds;
    public GameObject waveCountUI;
    public Text waveCountText;
    public GameObject getReadyUI;


    void Start()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawnpoints referenced");
        }

        // SINGLESCENE: DISABLED
        //nextWave = 0;
        //SetupWave();

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
        // SINGLESCENE: starts infinite play correctly via first play, play again, quit & return
        InitialWaveSetup();

        BNG.Damageable.onInfiniteDuckHit += increaseDuckHitCount;
    }

    void OnDisable()
    {
        BNG.Damageable.onInfiniteDuckHit -= increaseDuckHitCount;
    }

    void InitialWaveSetup()
    {
        nextWave = 0;
        ducksHitTotal = 0;
        SetupWave();
        state = WaveState.READY;
    }

    void SetupWave()
    {
        waveTimeRemaining = waves[nextWave].waveTime;
        ConvertTime();
        //currentWaveTime = (int)waveTimeRemaining;
        currentWaveNumber = waves[nextWave].waveNumber;
        waves[nextWave].ducksHitThisWave = 0;
        ducksLeft = waves[nextWave].duckCount;
        duckSpeed = waves[nextWave].rate;

        onWaveChange?.Invoke();
    }

    // SINGLESCENE: used on "Play Again"
    public void ResetWaves()
    {
        if (waves.Count > 1)
        {
            waves.RemoveRange(1, waves.Count - 1);
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
            onGameOver?.Invoke();
            StopAllCoroutines(); // stop ducks flying

            enabled = false;
        }
        else
        {
            int nextDucks = waves[nextWave].duckCount * 2;
            waves.Add(new InfiniteWave(waves[nextWave].waveNumber + 1, nextDucks, waves[nextWave].rate * 1.1f, waves[nextWave].waveTime + (5f * waves[nextWave].duckCount), waves[nextWave].ducksHitThisWave = 0, nextDucks));
            nextWave++;
            SetupWave();

            state = WaveState.READY;
        }
    }

    void Timer()
    {
        if (waveTimeRemaining >= 0)
        {
            ConvertTime();

            // decrement waveTimeRemaining once per second
            waveTimeRemaining -= Time.deltaTime;

            onTimeChange?.Invoke();
        }
    }

    void ConvertTime()
    {
        currentWaveMinutes = Mathf.FloorToInt(waveTimeRemaining / 60).ToString();
        float mathSeconds = Mathf.FloorToInt(waveTimeRemaining % 60);
        currentWaveSeconds = string.Format("{0:00}", mathSeconds);
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