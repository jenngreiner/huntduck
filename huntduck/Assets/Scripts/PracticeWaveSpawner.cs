using System.Collections;
using UnityEngine;

public class PracticeWaveSpawner : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAVING, COUNTING };
    private SpawnState state = SpawnState.COUNTING;

    // makes the Wave class fields editable from Inspector
    [System.Serializable]
    public class ClayWave
    {
        public string roundNumber;
        public int count;
        public float rate;
    }

    public ClayWave[] waves;
    private int nextWave;
    public int claysHit;

    public float timeBetweenWaves = 1f;
    private float waveCountDown;
    private float searchCountDown = 1f;

    public GameObject[] spawnPoints;


    void Update()
    {
        if (state == SpawnState.WAVING)
        {
            // check if ducks still left
            if (hitAllClays())
            {
                Debug.Log("We hit 3 clays, calling WaveComplete");
                // start the next wave
                WaveCompleted();
            }
            else
            {
                // need more clays
                Debug.Log("PracticeWaveSpawner knows: We still got clays left!");
                return;
            }
        }

        Debug.Log("We are not in a waiting state");

        // check if no seconds left, if still seconds, drop down to else and remove 1 second per second
        if (waveCountDown <= 0)
        {
            // spawn wave, as long as not already spawning
            if (state != SpawnState.SPAWNING)
            {
                // start spawning wave
                StartCoroutine(SpawnWave(waves[nextWave]));
                Debug.Log("Spawning a new wave");
            }
        }
        else
        {
            waveCountDown -= Time.deltaTime;
        }
    }

    void OnEnable()
    {
        SetupWave(); // SINGLESCENE: in enabled instead of start for "Play Again"
        BNG.Damageable.onClayHit += HitClay;
    }

    void OnDisable()
    {
        BNG.Damageable.onClayHit -= HitClay;
    }

    void SetupWave()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawnpoints referenced");
        }

        // set time before and between rounds
        waveCountDown = timeBetweenWaves;
        nextWave = 0;
        claysHit = 0;
}

    void WaveCompleted()
    {
        Debug.Log("Wave completed");

        state = SpawnState.COUNTING;
        waveCountDown = timeBetweenWaves;

        // clay waves are over
        if ((nextWave + 1) > (waves.Length - 1))
        {
            Debug.Log("Clay waves are over");
            SetupWave(); //SINGLESCENE: setup waves in case you want to run them again
            enabled = false;
        }
        else
        {
            nextWave++;
            Debug.Log("Wave  is ending. Next wave is " + (nextWave + 1));
        }
    }

    void HitClay()
    {
        claysHit++;
    }

    bool hitAllClays()
    {
        if (claysHit >= 3)
        {
            return true;
        }

        return false;
    }

    bool ClaysAreLeft()
    {
        searchCountDown -= Time.deltaTime;

        if (searchCountDown <= 0f)
        {
            // reset search countdown just incase ducks are still left
            searchCountDown = 1f;

            // *****THIS MIGHT NOW BE HOW WE WANT TO KEEP TRACK OF DUCKS*****
            // search for remaining ducks
            if (GameObject.FindGameObjectsWithTag("PracticeClay").Length == 0)
            {
                Debug.Log("No clays found");
                return false;
            }
        }

        return true;
    }

    IEnumerator SpawnWave(ClayWave _wave)
    {
        Debug.Log("Spawning Wave " + _wave.roundNumber);

        // set state to spawning to make sure only one SpawnWave at a time
        state = SpawnState.SPAWNING;

        // loop through the amount of ducks you want to spawn
        for (int i = 0; i < _wave.count; i++)
        {
            SpawnClay();
            yield return new WaitForSeconds(1 / _wave.rate);

            if (hitAllClays())
            {
                // stop spawning clays
                break;
            }
        }

        state = SpawnState.WAVING;
        Debug.Log("Back in waiting state");
        yield break;
    }

    void SpawnClay()
    {
        // Spawn Duck
        Debug.Log("Spawning clay");

        GameObject activeSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        activeSpawnPoint.GetComponent<ObjectLauncher>().ShootLauncher();
    }
}