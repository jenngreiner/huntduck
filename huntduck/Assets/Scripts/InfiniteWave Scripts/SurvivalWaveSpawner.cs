using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using huntduck;

public class SurvivalWaveSpawner : MonoBehaviour
{
    public enum WaveState { READY, STARTING, WAVING, ENDING };
    private WaveState state = WaveState.READY;

    [System.Serializable]
    public class InfiniteWave
    {
        // first wave
        public int waveNumber = 1;
        public int ducksThisWave = 3;
        public float rate = 1;
        public float waveTime = 60f;
        public int ducksHitThisWave = 0; // "reset" when wave constructed

        public enum WaveType { NORMAL, SURVIVAL, BONUS, GOLDEN }
        public WaveType waveType = WaveType.NORMAL;

        public InfiniteWave() { }

        public InfiniteWave(int newWaveNumber, int newDuckCount, float newRate, float newWaveTime, WaveType _waveType)
        {
            waveNumber = newWaveNumber;
            ducksThisWave = newDuckCount;
            rate = newRate;
            waveTime = newWaveTime;
            waveType = _waveType;
        }
    }

    public GameObject[] spawnPoints;
    public GameObject[] bonusSpawnPoints;

    public GameObject waveCountUI;
    public Text waveCountText;
    public GameObject getReadyUI;
    public Text getReadyText;
    public GameObject helperUI;
    public Text helperText;
    
    public TimeSpan timerSeconds;

    #region wave variables
    public List<InfiniteWave> waves;
    private int thisWave;
    public int ducksHitTotal;
    public int ducksLeft;
    public float duckSpeed = 1f;

    public int currentWaveNumber;
    public int currentWaveTime;
    private float waveTimeRemaining;
    public float timeDelay = 1f;
    public string currentWaveMinutes;
    public string currentWaveSeconds;
    public int bonusWaveNumber;
    private int waveSetNumber = 2;

    private int survivalBonusPoints = 100;

    private int duckBase;
    private float duckMultiplier = 1f;
    private int ducksThisWave;
    private InfiniteWave.WaveType nextWaveType;
    private float nextWaveTime;
    private float normieMultiplier = 0f;
    private float fastMultiplier = 0f;
    private float angryMultiplier = 0f;

    private int bonusGeeseVNumber;
    private int goldenGeeseNumber;
    #endregion

    #region events
    public delegate void TimeChange();
    public static event TimeChange onTimeChange;

    public delegate void DuckHit();
    public static event DuckHit onDuckHit;

    public delegate void WaveChange();
    public static event WaveChange onWaveChange;

    public delegate void FirstWaveStart();
    public static event FirstWaveStart onFirstWaveStart;

    public delegate void GameOver();
    public static event GameOver onGameOver;

    public delegate void SurvivalWaveNoDamage(int bonusPoints);
    public static event SurvivalWaveNoDamage onSurvivalWaveNoDamage;
    #endregion

    private PlayerData playerData;
    private float startHealthSurvival;

    // TODO: remove wavetime by making ducks dangerous, player has to survive

    void Start()
    {
        playerData = ObjectManager.instance.player;
        bonusGeeseVNumber = waveSetNumber - 1;
        goldenGeeseNumber = waveSetNumber - 1;
    }

    void OnEnable()
    {
        #region InitialWaveSetup
        // SINGLESCENE: Transfer this block to Start() for multi-scene
        InitialWaveSetup();
        if (spawnPoints.Length == 0) // make sure we have spawnPoints to spawn from
        {
            Debug.LogError("No spawnpoints referenced");
        }
        #endregion

        BNG.Damageable.onInfiniteDuckHit += increaseDuckHitCount;
        RestartGameMode.onRestartMode += ResetWaves;
    }

    void OnDisable()
    {
        BNG.Damageable.onInfiniteDuckHit -= increaseDuckHitCount;
        RestartGameMode.onRestartMode -= ResetWaves;
    }

    void Update()
    {
        if (state == WaveState.READY)
        {
            StartCoroutine(StartWave(waves[thisWave]));
        }

        if (state == WaveState.WAVING)
        {
            //if (playerBeatWave())
            //{
            //    WaveCompleted(); // removes time element
            //}
            if (playerBeatWave() || !isTimeLeft())
            {
                WaveCompleted(); // we hit all ducks this wave, or ran out of time
            }
            else
            {
                RunTimer();
            }
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            waveTimeRemaining = 0;
            Debug.Log("KeyDown.O: Times up, End game");
        }
    }

    IEnumerator StartWave(InfiniteWave _thisWave)
    {
        state = WaveState.STARTING;

        #region Show "Wave #" Sign
        waveCountText.text = waves[thisWave].waveNumber.ToString();
        waveCountUI.SetActive(true);
        yield return new WaitForSecondsRealtime(timeDelay);
        waveCountUI.SetActive(false);
        #endregion

        #region Show "Get Ready" Sign
        SetGetReadyText(_thisWave); // set get ready text based on wave type
        getReadyUI.SetActive(true);
        yield return new WaitForSecondsRealtime(timeDelay);
        getReadyUI.SetActive(false);
        getReadyText.text = "GET READY"; // reset text
        #endregion

        if (waves[thisWave].waveType == InfiniteWave.WaveType.SURVIVAL)
        {
            startHealthSurvival = playerData.currenthealth;
        }

        state = WaveState.WAVING;

        // Decide how many Ducks & Geese to spawn
        switch (waves[thisWave].waveType)
        {
            case InfiniteWave.WaveType.BONUS:
                // replace waveSetNumber -2 with BonusGeeseVNumber as parameter
                for (int i = 0; i < (bonusGeeseVNumber); i++)
                {
                    SpawnDuck(bonusSpawnPoints, ObjectManager.instance.bonusGeese);
                    yield return new WaitForSeconds(3f);
                }
                break;
            case InfiniteWave.WaveType.GOLDEN:
                // replace waveSetNumber - 1 with GoldenGooseNumber as parameter
                StartCoroutine(SpawnGoldenGoose(goldenGeeseNumber));
                StartCoroutine(SpawnWaveDucks(_thisWave));
                break;
            default: // NORMAL, SURVIVAL
                StartCoroutine(SpawnWaveDucks(_thisWave));
                break;
        }

        yield break;
    }

    public void WaveCompleted()
    {
        state = WaveState.ENDING;

        // if the player didn't shoot enough ducks, and its not a bonus wave, end game
        if (!playerBeatWave() && waves[thisWave].waveType != InfiniteWave.WaveType.BONUS)
        {
            onGameOver?.Invoke();
            StopAllCoroutines(); // stop ducks flying
            ResetWaves();
            enabled = false;
        }
        else
        {
            // If completed Survival Wave without taking damage, give bonus points
            if (waves[thisWave].waveType == InfiniteWave.WaveType.SURVIVAL)
            {
                if (startHealthSurvival == playerData.currenthealth)
                {
                    int survivalBonus = survivalBonusPoints * waveSetNumber;
                    onSurvivalWaveNoDamage(survivalBonus);
                    StartCoroutine(SurvivalUIController(survivalBonus));
                }
            }

            #region Configure Next Wave
            int nextWaveNumber = waves[thisWave].waveNumber + 1;
            duckSpeed = waves[thisWave].rate * 1.05f;
            nextWaveTime = waves[thisWave].waveTime + (2f * waves[thisWave].ducksThisWave);
            SetWaveType(nextWaveNumber);
            SetWaveDucks(nextWaveNumber, waveSetNumber); 

            // TODO: remove time element
            waves.Add(new InfiniteWave(nextWaveNumber, ducksThisWave, duckSpeed, nextWaveTime, nextWaveType));
            Debug.Log("next wave #: " + nextWaveNumber + " wave set #: " + waveSetNumber);

            thisWave++;
            SetupWave();
            #endregion

            state = WaveState.READY; // Begin Wave next Frame in Update()
        }
    }

    IEnumerator SurvivalUIController(int survivalBonus)
    {
        helperUI.SetActive(true);
        helperText.text = "YOU SURVIVED \n + $" + survivalBonus.ToString();
        yield return new WaitForSecondsRealtime(3f);
        helperUI.SetActive(false);
        // right now this flashes but the next UI overrides it, gotta figure out how to pause instead of yeild here
    }

    void SetWaveType(int _nextWaveNumber)
    {
        switch (_nextWaveNumber % 5)
        {
            // TODO: consider adding case 1 with nextWaveTime = 30f * waveSetBonus
            // concern is as waves increase not enough time at outset, coming off bonus of 30f
            case 1: // Wave 1,6,11, so on
                nextWaveType = InfiniteWave.WaveType.NORMAL;
                nextWaveTime = 60f * waveSetNumber;
                break;
            case 2: // 2, 7, 12, so on
                //nextWaveType = InfiniteWave.WaveType.SURVIVAL;
                if (_nextWaveNumber <= 5) // 2: no survival wave on 1st wave set
                {
                    nextWaveType = InfiniteWave.WaveType.NORMAL;
                }
                else // 7, 12, so on: survival wave starts 2nd wave set
                {
                    nextWaveType = InfiniteWave.WaveType.SURVIVAL;
                }
                break;
            case 3: // 3, 8, 13, so on:
                if (_nextWaveNumber <= 5) // no golden wave on 1st wave set
                {
                    nextWaveType = InfiniteWave.WaveType.NORMAL;
                }
                else // 8, 13, so on: Golden Wave starts on 2nd wave set
                {
                    nextWaveType = InfiniteWave.WaveType.GOLDEN;
                }
                break;
            case 0: // 5, 10, 15 so on: Bonus wave every 5th wave
                nextWaveType = InfiniteWave.WaveType.BONUS;
                nextWaveTime = 30f;
                break;
            default: // 1st & 4th wave out of every wave set are normal
                nextWaveType = InfiniteWave.WaveType.NORMAL;
                break;
        }
    }

    void SetWaveDucks(int _nextWaveNumber, int _waveSetNumber)
    {
        if (_nextWaveNumber < 5) // waves 2-4 (wave 1 setup in OnEnable)
        {
            switch (_nextWaveNumber)
            {
                case 2:
                    duckBase = 4;
                    break;
                case 3:
                    duckBase = 6;
                    break;
                case 4:
                    duckBase = 6;
                    fastMultiplier = 0.5f;
                    break;
                default:
                    break;
            }
        }
        else // run logic for wave #s 6-10, 11-15, 16-20, so on
        {
            switch (_nextWaveNumber % 5)
            {
                case 1:
                    duckBase = UnityEngine.Random.Range(6, 9);
                    break;
                case 2:
                    duckBase = UnityEngine.Random.Range(10, 13);
                    break;
                case 3:
                    duckBase = UnityEngine.Random.Range(14, 17);
                    break;
                case 4:
                    duckBase = UnityEngine.Random.Range(18, 20);
                    break;
                case 0:
                    duckBase = waveSetNumber; // # of flying Vs
                    waveSetNumber++;
                    break;
            }

            switch (_waveSetNumber) // starts on Wave 6 (2nd wave set)
            {
                case 1: // wave #s 6-10: 1/2 normies, fast
                    duckMultiplier = 1f;
                    normieMultiplier = fastMultiplier = (0.5f);
                    break;
                case 2: // wave #s 10-15: 1/3: normie, fast, angry (*1.5)
                    duckMultiplier = 1.5f;
                    normieMultiplier = fastMultiplier = angryMultiplier = (0.33f);
                    break;
                case 3: // wave #s 15-20: 1/2 fast, angry (*2)
                    duckMultiplier = 2f;
                    fastMultiplier = angryMultiplier = (0.5f);
                    break;
                case 4: // wave #s 20-25: 1/3 fast, 2/3 angry (*3)
                    duckMultiplier = 3f;
                    fastMultiplier = (0.33f);
                    angryMultiplier = (0.66f);
                    break;
                default: // wave #s 25+: angry only 
                    duckMultiplier = 1f * _waveSetNumber;
                    duckBase = UnityEngine.Random.Range(17, 25);
                    angryMultiplier = 1f;
                    break;
            }
        }

        // Set this wave's ducks before adding
        ducksThisWave = (int)(duckMultiplier * duckBase);
    }

    void InitialWaveSetup()
    {
        // create empty waves list, add first wave with default values
        waves = new List<InfiniteWave> { new InfiniteWave() };
        thisWave = 0;
        ducksHitTotal = 0;
        SetupWave();
        state = WaveState.READY;
        onFirstWaveStart?.Invoke();
        Debug.Log("Setting up initial wave with " + waves.Count + " waves");
    }

    void SetupWave()
    {
        waveTimeRemaining = waves[thisWave].waveTime;
        SetCurrentWaveSeconds();
        currentWaveNumber = waves[thisWave].waveNumber;
        waves[thisWave].ducksHitThisWave = 0;
        if (waves[thisWave].waveType == InfiniteWave.WaveType.BONUS)
        {
            ducksLeft = (bonusGeeseVNumber) * ObjectManager.instance.bonusGeese.transform.childCount;
        }
        else if (waves[thisWave].waveType == InfiniteWave.WaveType.GOLDEN)
        {

            ducksLeft = waves[thisWave].ducksThisWave + goldenGeeseNumber; // ducks + golden ducks
        }
        else
        {
            ducksLeft = waves[thisWave].ducksThisWave; // its a normal wave, yo.
        }

        onWaveChange?.Invoke();
    }

    void SetGetReadyText(InfiniteWave _thisWave)
    {
        switch (_thisWave.waveType)
        {
            case InfiniteWave.WaveType.SURVIVAL:
                getReadyText.text = "SURVIVAL WAVE";
                break;
            case InfiniteWave.WaveType.BONUS:
                getReadyText.text = "BONUS GEESE";
                break;
            case InfiniteWave.WaveType.GOLDEN:
                getReadyText.text = "FIND THE GOLDEN GOOSE";
                break;
            default:
                getReadyText.text = "GET READY";
                break;
        }
    }

    IEnumerator SpawnWaveDucks(InfiniteWave _thisWave)
    {
        for (int i = 0; i < _thisWave.ducksThisWave; i++) // loop through the amount of ducks you want to spawn
        {
            float randomNum = UnityEngine.Random.value;
            if (randomNum < angryMultiplier)
            {
                SpawnDuck(spawnPoints, ObjectManager.instance.angryDuck);
            }
            else if (randomNum < (angryMultiplier + fastMultiplier))
            {
                SpawnDuck(spawnPoints, ObjectManager.instance.fastDuck);
            }
            else
            {
                SpawnDuck(spawnPoints, ObjectManager.instance.normDuck);
            }
            //ChooseDuckToSpawn();
            yield return new WaitForSeconds(1 / _thisWave.rate);
        }
    }

    IEnumerator SpawnGoldenGoose(int _waveSetNumber)
    {
        for (int g = 0; g < _waveSetNumber; g++) // spawn "i" golden geese based on how many waveSets
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 30f)); // wait a random amount before spawning
            SpawnDuck(bonusSpawnPoints, ObjectManager.instance.goldenGoose); // spawn one golden goose 
        }
    }

    void SpawnDuck(GameObject[] _spawnPoints, GameObject duckToLaunch)
    {
        GameObject activeSpawnPoint = _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Length)];
        DuckLauncher duckLauncher = activeSpawnPoint.GetComponent<DuckLauncher>();
        duckLauncher.LaunchObj(duckToLaunch);
    }

    public void increaseDuckHitCount()
    {
        ducksHitTotal++;
        waves[thisWave].ducksHitThisWave++;

        ducksLeft--;

        onDuckHit();
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
        if (playerData.currenthealth > 0 && ducksLeft <= 0)
        {
            StopAllCoroutines(); // stop launching ducks - TODO: check if this is still needed, might be relic of og"flight"
            return true;
        }
        return false;
    }

    void RunTimer()
    {
        if (waveTimeRemaining >= 0)
        {
            SetCurrentWaveSeconds();

            // decrement waveTimeRemaining once per second
            waveTimeRemaining -= Time.deltaTime;

            onTimeChange?.Invoke();
        }
    }

    void SetCurrentWaveSeconds()
    {
        currentWaveMinutes = Mathf.FloorToInt(waveTimeRemaining / 60).ToString();
        float mathSeconds = Mathf.FloorToInt(waveTimeRemaining % 60);
        currentWaveSeconds = string.Format("{0:00}", mathSeconds);
    }

    private void ResetWaves()
    {
        // SINGLESCENE: used on "Play Again"
        // reset all variables to initial state
        duckSpeed = 1f;
        survivalBonusPoints = 100;
        duckMultiplier = 1f;
        waveSetNumber = 1;
        normieMultiplier = 0f;
        fastMultiplier = 0f;
        angryMultiplier = 0f;
        onWaveChange();
    }
}