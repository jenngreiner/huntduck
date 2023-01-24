using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NW_WaveSpawner : MonoBehaviourPun
{
    public enum SpawnState { SPAWNING, WAITING, COUNTING };

    // makes the Wave class fields editable from Inspector
    [System.Serializable]
    public class Wave
    {
        public string roundNumber;
        public int count;
        public float rate;
    }

    public Wave[] waves;
    private int nextWave = 0;

    public float timeBetweenWaves = 5f;
    private float waveCountDown;
    private float searchCountDown = 1f;

    public GameObject[] spawnPoints;
    public RoundUIManager roundUI;
    public GameObject gameOverUI;

    private SpawnState state = SpawnState.COUNTING;


    void Start()
    {
        if (PhotonNetwork.InRoom)
        {
            this.photonView.RPC("RPC_SetupWave", RpcTarget.All);
        }
    }

    void Update()
    {
        if (PhotonNetwork.InRoom)
        {
            if (state == SpawnState.WAITING)
            {
                // check if ducks still left
                if (!DucksAreLeft())
                {
                    // start the next wave
                    this.photonView.RPC("RPC_WaveCompleted", RpcTarget.All);
                    Debug.Log("RPC_WaveCompleted");
                }
                else
                {
                    // ducks still left, so rounds not over
                    Debug.Log("We still got ducks left!");
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
                    this.photonView.RPC("RPC_WaveStart", RpcTarget.All);
                    Debug.Log("RPC_WaveStart");
                }
            }
            else
            {
                this.photonView.RPC("RPC_StartCountDown", RpcTarget.All);
                Debug.Log("RPC_StartCountDown");
            }
        }
    }

    [PunRPC]
    void RPC_StartCountDown()
    {
        waveCountDown -= Time.deltaTime;
    }

    [PunRPC]
    void RPC_SetupWave()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawnpoints referenced");
        }

        // set time before and between rounds
        waveCountDown = timeBetweenWaves;
    }

    [PunRPC]
    void RPC_WaveStart()
    {
        StartCoroutine(SpawnWave(waves[nextWave]));
    }

    [PunRPC]
    void RPC_WaveCompleted()
    {
        Debug.Log("Wave completed");

        state = SpawnState.COUNTING;
        waveCountDown = timeBetweenWaves;

        // game is over
        if ((nextWave + 1) > (waves.Length - 1))
        {
            Debug.Log("Game is over");
            gameOverUI.SetActive(true);
            this.enabled = false;
        }
        else
        {
            nextWave++;
            Debug.Log("Wave  is ending. Next wave is " + (nextWave + 1));
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
                Debug.Log("No ducks found");
                return false;
            }
        }

        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("Spawning Wave " + _wave.roundNumber);

        // set state to spawning to make sure only one SpawnWave at a time
        state = SpawnState.SPAWNING;

        roundUI.roundNumber.text = _wave.roundNumber;
        roundUI.gameObject.SetActive(true);
        //_roundUIManager.roundStartSound.Play();

        yield return new WaitForSeconds(1.5f);
        roundUI.gameObject.SetActive(false);

        // loop through the amount of ducks you want to spawn
        for (int i = 0; i < _wave.count; i++)
        {
            SpawnDuck();
            yield return new WaitForSeconds(1 / _wave.rate);
        }

        state = SpawnState.WAITING;
        Debug.Log("Back in waiting state");
        yield break;
    }

    void SpawnDuck()
    {
        // Spawn Duck
        Debug.Log("Spawning duck");

        GameObject activeSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        activeSpawnPoint.GetComponent<NW_ObjectLauncher>().ShootProjectile();
    }
}
