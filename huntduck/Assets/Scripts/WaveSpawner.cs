using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, COUNTING };

    // makes the Wave class fields editable from Inspector
    [System.Serializable]
    public class Wave
    {
        public string name;
        //public Transform duck;
        public int count;
        public float rate;
    }

    public Wave[] waves;
    private int nextWave = 0;

    public GameObject[] spawnPoints;

    public float timeBetweenWaves = 5f;
    private float waveCountDown;

    private float searchCountDown = 1f;

    private SpawnState state = SpawnState.COUNTING;

    void Start()
    {
        //if (spawnPoints.Length == 0)
        //{
        //    Debug.LogError("No spawnpoints referenced");
        //}

        waveCountDown = timeBetweenWaves;
    }

    void Update()
    {
        if (state == SpawnState.WAITING)
        {
            // check if ducks still left
            if (!DucksAreLeft())
            {
                // start the next wave
                WaveCompleted();
            }
            else
            {
                // ducks still left, so rounds not over
                return;
            }
        }

        // check if no seconds left, if still seconds, drop down to else and remove 1 second per second
        if (waveCountDown <= 0)
        {
            // spawn wave, as long as not already spawning
            if (state != SpawnState.SPAWNING)
            {
                // start spawning wave
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountDown -= Time.deltaTime;
        }
    }

    void WaveCompleted()
    {
        Debug.Log("Wave completed");

        state = SpawnState.COUNTING;
        waveCountDown = timeBetweenWaves;

        // game is over
        if (nextWave+1 > waves.Length - 1)
        {
            // being the game again
            // ***** INSTEAD, LET'S SHOW GAME OVER AND SCORE*****
            // ***** OR WE COULD OPEN UP THE NEXT ISLAND FOR DUCK ISLAND*****
            nextWave = 0;
            Debug.Log("ALL WAVES COMPLETE. Looping..");
        }
        else
        {
            nextWave++;
        }
    }

    bool DucksAreLeft()
    {
        searchCountDown -= Time.deltaTime;

        if (searchCountDown <= 0f)
        {
            // reset search countdown just incase ducks are still left
            searchCountDown = 1f;

            // *****THIS MIGHT NOW BE HOW WE WANT TO KEEP TRACK OF DUCKS*****
            // search for remaining ducks
            if (GameObject.FindGameObjectsWithTag("Duck").Length == 0)
            {
                return false;
            }
        }

        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("Spawning Wave " + _wave.name);

        // set state to spawning to lock in a signle use of IEnumerator
        state = SpawnState.SPAWNING;

        // loop through the amount of ducks you want to spawn
        for (int i = 0; i < _wave.count; i++)
        {
            //SpawnDuck(_wave.duck);
            SpawnDuck();
            yield return new WaitForSeconds(1/_wave.rate);
        }

        state = SpawnState.WAITING;
        yield break;
    }

    void SpawnDuck()
    {
        // Spawn Duck
        Debug.Log("Spawning duck");

        GameObject activeSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        activeSpawnPoint.GetComponent<DuckSpawner>().DuckLaunch();

        
    }
}
