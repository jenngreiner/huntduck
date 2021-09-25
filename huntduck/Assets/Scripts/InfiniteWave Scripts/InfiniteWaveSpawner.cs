using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InfiniteWaveSpawner : MonoBehaviour
{
    public enum WaveState { STARTING, WAITING, COUNTING };
    private WaveState state = WaveState.COUNTING;

    // makes the Wave class fields editable from Inspector
    [System.Serializable]
    public class InfiniteWave
    {
        public int waveNumber;
        public int duckCount;
        public float rate;
        public float waveTime = 3f;

        public InfiniteWave(int newWaveNumber, int newDuckCount, float newRate, float newWaveTime)
        {
            waveNumber = newWaveNumber;
            duckCount = newDuckCount;
            rate = newRate;
            waveTime = newWaveTime;
        }
    }

    public List<InfiniteWave> waves;
    private int nextWave;
    public static int ducksHit = 0;

    private float waveTimeRemaining;
    public float timeBetweenWaves = 1f;
    private float waveCountDown;
    public static int currentWaveNumber;
    public static int currentWaveTime;

    public GameObject[] spawnPoints;

    public delegate void gameOver();
    public static event gameOver onGameOver;

    public delegate void OnWaveCompleted();
    public static event OnWaveCompleted onWaveCompleted;

    public delegate void OnWaveChange();
    public static event OnWaveChange onWaveChange;

    public delegate void OnTimeChange();
    public static event OnTimeChange onTimeChange;

    public static TimeSpan timerSeconds;

    void Start()
    {
        SetupWave();
    }

    void Update()
    {
        if (state == WaveState.WAITING)
        {
            if (waveTimeRemaining > 0)
            {
                Timer();
            }

            // we hit all ducks this wave, or ran out of time
            if (playerBeatWave() || !isTimeLeft())
            {
                // start the next wave
                WaveCompleted();
            }
            else
            {
                waveTimeRemaining -= Time.deltaTime;
                return;
            }
        }

        // check if no seconds left, if still seconds, drop down to else and remove 1 second per second
        if (waveCountDown <= 0)
        {
            // spawn wave, as long as not already spawning
            if (state != WaveState.STARTING)
            {
                // start next wave
                StartCoroutine(StartWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountDown -= Time.deltaTime;
            //currentWaveTime = (int)waveTimeRemaining;
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

        if (onTimeChange != null)
        {
            onTimeChange();
        }

        // call out an event: hey subs, I changed my wave, do what you will man
        if (onWaveChange != null)
        {
            onWaveChange();
        }

        // set time before and between rounds
        waveCountDown = timeBetweenWaves;
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

    void WaveCompleted()
    {
        state = WaveState.COUNTING;
        waveCountDown = timeBetweenWaves;

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
            waves.Add(new InfiniteWave((waves[nextWave].waveNumber + 1), (waves[nextWave].duckCount * 2), (waves[nextWave].rate * 1.05f), waves[nextWave].waveTime));
            nextWave++;
            currentWaveNumber = waves[nextWave].waveNumber;
            currentWaveTime = (int)waves[nextWave].waveTime;

            if (onWaveCompleted != null)
            {
                onWaveCompleted();
            }
        }
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
        if (ducksHit >= waves[nextWave].duckCount)
        {
            return true;
        }
        return false;
    }

    public void increaseDuckHitCount()
    {
        ducksHit++;
        //Debug.Log("We hit a duck!");
    }

    IEnumerator StartWave(InfiniteWave _thisWave)
    {
        // set state to spawning to make sure only one SpawnWave at a time
        state = WaveState.STARTING;

        // reset wave time
        waveTimeRemaining = waves[nextWave].waveTime;

        if (onWaveChange != null)
        {
            onWaveChange();
        }


        // loop through the amount of ducks you want to spawn
        for (int i = 0; i < _thisWave.duckCount; i++)
        {
            state = WaveState.WAITING;
            SpawnDuck();
            yield return new WaitForSeconds(1 / _thisWave.rate);

            if (playerBeatWave())
            {
                // stop spawning ducks
                break;
            }
        }

        yield break;
    }

    void SpawnDuck()
    {
        // Spawn Duck
        GameObject activeSpawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        activeSpawnPoint.GetComponent<ObjectLauncher>().ShootLauncher();
    }
}