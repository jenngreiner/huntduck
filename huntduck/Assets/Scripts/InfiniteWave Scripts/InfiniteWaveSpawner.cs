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
        public float waveTime = 30f;
        public int ducksHitThisWave = 0;

        public InfiniteWave(int newWaveNumber, int newDuckCount, float newRate, float newWaveTime, int newDucksHitThisWave)
        {
            waveNumber = newWaveNumber;
            duckCount = newDuckCount;
            rate = newRate;
            waveTime = newWaveTime;
            ducksHitThisWave = newDucksHitThisWave;
        }
    }

    //public class BonusWave : InfiniteWave
    //{
    //    public int bonusWaveCount;
    //    public int vCount;

    //    public BonusWave(int newBonusWaveCount, int newVCount, int newWaveNumber, int newDuckCount, float newRate, float newWaveTime, int newDucksHitThisWave, int newDuckTotal) : base(newWaveNumber, newDuckCount, newRate, newWaveTime, newDucksHitThisWave, newDuckTotal)
    //    {
    //        Debug.Log("calling BW constructor");
    //        bonusWaveCount = newBonusWaveCount;
    //        vCount = newVCount;
    //    }
    //}

    public List<InfiniteWave> waves;
    private int thisWave;
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

    public int bonusWaveNumber;

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
        //thisWave = 0;
        //SetupWave();

        // note: we are in READY state, so first Frame after start will StartWave
    }

    void Update()
    {
        if (state == WaveState.READY)
        {
            StartCoroutine(StartWave(waves[thisWave]));
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
        thisWave = 0;
        ducksHitTotal = 0;
        SetupWave();
        state = WaveState.READY;
    }

    void SetupWave()
    {
        waveTimeRemaining = waves[thisWave].waveTime;
        ConvertTime();
        //currentWaveTime = (int)waveTimeRemaining;
        currentWaveNumber = waves[thisWave].waveNumber;
        waves[thisWave].ducksHitThisWave = 0;
        ducksLeft = waves[thisWave].duckCount;
        duckSpeed = waves[thisWave].rate;

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
        waveCountText.text = waves[thisWave].waveNumber.ToString();
        waveCountUI.SetActive(true);
        yield return new WaitForSecondsRealtime(timeDelay);
        waveCountUI.SetActive(false);

        // "Get Ready"
        getReadyUI.SetActive(true);
        yield return new WaitForSecondsRealtime(timeDelay);
        getReadyUI.SetActive(false);

        state = WaveState.WAVING;

        // loop through the amount of ducks you want to spawn
        for (int i = 0; i < _thisWave.duckCount; i++)
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
            Debug.Log("thisWave: " + thisWave);
            int nextWaveNumber = waves[thisWave].waveNumber + 1;
            Debug.Log("nextWave: " + nextWaveNumber);
            if (nextWaveNumber == 5) {
                // first bonus wave
                bonusWaveNumber = 1;
                waves.Add(new InfiniteWave(waves[thisWave].waveNumber + 1, bonusWaveNumber, bonusWaveNumber, 15f, waves[thisWave].ducksHitThisWave = 0));

                Debug.Log("starting first bonus wave");
                //waves.Add(new BonusWave(1, 1, waves[thisWave].waveNumber + 1, 0, 1.05f,15f, waves[thisWave].ducksHitThisWave = 0, waves[thisWave].duckTotal));
            } else if ((nextWaveNumber % 5) == 0) { // multiples of 5 
                // bonus wave
                bonusWaveNumber++;
                waves.Add(new InfiniteWave(waves[thisWave].waveNumber + 1, bonusWaveNumber, bonusWaveNumber * 1.05f, 15f, waves[thisWave].ducksHitThisWave = 0));

                Debug.Log("bonuswave: " + bonusWaveNumber);

                // public BonusWave(int newBonusWaveCount, int newWaveNumber, int newDuckCount, float newRate, int newDucksHitThisWave, int newHitTotal)
                //waves.Add(new BonusWave(waves[thisWave] + 1, waveCountText[thisWave].vCount, waves[thisWave].waveNumber + 1, waves[thisWave].vCount + 1, waves[thisWave].rate * 1.05f, waves[thisWave].ducksHitThisWave = 0, waves[thisWave].hitTotal));
            } else if ((nextWaveNumber % 5) == 1) {
                // reset time and duck count so shit dont get too crazy
                waves.Add(new InfiniteWave(waves[thisWave].waveNumber + 1, 2, waves[thisWave].rate * 1.05f, 30f, waves[thisWave].ducksHitThisWave = 0));

                Debug.Log("resetwave on wave #: " + (waves[thisWave].waveNumber + 1));
            } else {
                waves.Add(new InfiniteWave(waves[thisWave].waveNumber + 1, waves[thisWave].duckCount * 2, waves[thisWave].rate * 1.05f, waves[thisWave].waveTime + (5f * waves[thisWave].duckCount), waves[thisWave].ducksHitThisWave = 0));

                Debug.Log("infinitewave #: " + (waves[thisWave].waveNumber + 1));
            }
            thisWave++;
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
        waves[thisWave].ducksHitThisWave++;

        ducksLeft--;

        onDuckHit();
    }

    void SpawnDuck()
    {
        GameObject activeSpawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        activeSpawnPoint.GetComponent<DuckLauncher>().LaunchObj();
    }
}