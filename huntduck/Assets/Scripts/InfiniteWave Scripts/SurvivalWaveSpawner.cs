using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        public float waveTime = 30f;
        public int ducksHitThisWave = 0; // "reset" when wave constructed

        public enum WaveType { NORMAL, SURVIVAL, BONUS, GOLDEN }
        public WaveType waveType = WaveType.NORMAL;

        // override when adding wave
        public int normieDucks = 0;
        public int fastDucks = 0;
        public int angryDucks = 0;
        public int bonusGeese = 0;
        public int goldenGeese = 0;

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

    #region wave variables
    public List<InfiniteWave> waves;
    private int thisWave;
    public int ducksHitTotal;
    public int ducksLeft;
    public float duckSpeed = 1f;
    private float waveTimeRemaining;
    public float timeDelay = 1f;
    public int currentWaveNumber;
    public int currentWaveTime;
    public string currentWaveMinutes;
    public string currentWaveSeconds;
    public int bonusWaveNumber;
    private int waveSetNumber = 0;

    //new
    private int duckBase;
    private float duckMultiplier;
    private int ducksThisWave;
    private InfiniteWave.WaveType nextWaveType;
    private float waveTime;
    private float bonusWaveTime = 15f;
    private float normieMultiplier = 0f;
    private float fastMultiplier = 0f;
    private float angryMultiplier = 0f;
    #endregion

    public GameObject[] spawnPoints;
    public GameObject[] bonusSpawnPoints;

    public TimeSpan timerSeconds;
    public GameObject waveCountUI;
    public Text waveCountText;
    public GameObject getReadyUI;
    public Text getReadyText;
    

    #region events
    public delegate void OnTimeChange();
    public static event OnTimeChange onTimeChange;

    public delegate void OnDuckHit();
    public static event OnDuckHit onDuckHit;

    public delegate void OnWaveChange();
    public static event OnWaveChange onWaveChange;

    public delegate void gameOver();
    public static event gameOver onGameOver;
    #endregion


    // TODO: remove wavetime by making ducks dangerous, player has to survive
    // TODO: create bonus points for survival wave


    void Start()
    {
        // first wave is added & set up in OnEnable
        if (spawnPoints.Length == 0) // make sure we have spawnPoints to spawn from
        {
            Debug.LogError("No spawnpoints referenced");
        }

        #region SINGLESCENE: DISABLED
        // InitialWaveSetup()
        #endregion
    }

    void Update()
    {
        if (state == WaveState.READY)
        {
            StartCoroutine(StartWave(waves[thisWave]));
        }

        if (state == WaveState.WAVING)
        {
            if (playerBeatWave() || !isTimeLeft())
            {
                WaveCompleted(); // we hit all ducks this wave, or ran out of time
            }
            else
            {
                RunTimer();
            }
        }
    }

    void OnEnable()
    {
        // SINGLESCENE: starts infinite play correctly via first play, play again, quit & return
        InitialWaveSetup();

        BNG.Damageable.onInfiniteDuckHit += increaseDuckHitCount;
        VFly.onFlyingVHit += increaseDuckHitCount;
    }

    void OnDisable()
    {
        BNG.Damageable.onInfiniteDuckHit -= increaseDuckHitCount;
        VFly.onFlyingVHit -= increaseDuckHitCount;
    }

    void InitialWaveSetup()
    {
        waves.Add(new InfiniteWave()); // add first wave with default values
        thisWave = 0;
        ducksHitTotal = 0;
        SetupWave();
        state = WaveState.READY;
    }

    void SetupWave()
    {
        waveTimeRemaining = waves[thisWave].waveTime;
        SetCurrentWaveSeconds();
        currentWaveNumber = waves[thisWave].waveNumber;
        waves[thisWave].ducksHitThisWave = 0;
        ducksLeft = waves[thisWave].ducksThisWave;

        onWaveChange?.Invoke();
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
        SetGetReadyText(_thisWave); // set get ready text based on wave type
        getReadyUI.SetActive(true);
        yield return new WaitForSecondsRealtime(timeDelay);
        getReadyUI.SetActive(false);
        getReadyText.text = "GET READY"; // reset text

        state = WaveState.WAVING;

        // spawn ducks based on waveType
        if (waves[thisWave].waveType == InfiniteWave.WaveType.BONUS)
        {
            for (int i = 0; i < _thisWave.ducksThisWave; i++) // loop through the amount of ducks you want to spawn
            {
                SpawnDuck(bonusSpawnPoints, DuckManager.instance.bonusGeese);
                yield return new WaitForSeconds(1 / _thisWave.rate);
            }
        }
        else
        {
            for (int i = 0; i < _thisWave.ducksThisWave; i++) // loop through the amount of ducks you want to spawn
            {
                ChooseDuckToSpawn();
                Debug.Log("Spawned a duck in a non bonus wave " + DateTime.Now);
                yield return new WaitForSeconds(1 / _thisWave.rate);

                if (waves[thisWave].waveType == InfiniteWave.WaveType.GOLDEN)
                {
                    for (int g = 0; g < waveSetNumber; g++) // spawn "i" golden geese based on how many waveSets
                    {
                        yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 30f)); // wait a random amount before spawning
                        SpawnDuck(bonusSpawnPoints, DuckManager.instance.goldenGoose); // spawn one golden goose 
                        Debug.Log("Spawned a golden goose in non bonus wave " + DateTime.Now);
                    }
                }
            }
        }
        yield break;
    }

    void SetGetReadyText(InfiniteWave _thisWave)
    {
        switch (_thisWave.waveType)
        {
            case InfiniteWave.WaveType.SURVIVAL:
                getReadyText.text = "SURVIVAL WAVE";
                break;
            case InfiniteWave.WaveType.BONUS:
                getReadyText.text = "GEESE";
                break;
            case InfiniteWave.WaveType.GOLDEN:
                getReadyText.text = "BEWARE THE GOLDEN GOOSE";
                break;
            default:
                getReadyText.text = "GET READY";
                break;
        }
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
            // set up next wave
            int nextWaveNumber = waves[thisWave].waveNumber + 1;
            duckSpeed = waves[thisWave].rate * 1.05f;
            waveTime = waves[thisWave].waveTime + (2f* waves[thisWave].ducksThisWave); // TODO: remove time element

            // Calculate wave type & ducks
            if (nextWaveNumber <= 5) // if first five waves
            {
                RunFirstFiveWaveLogic(nextWaveNumber);
            }
            else // for 6-10+ all remaining waves
            {
                RunInfiniteWaveLogic(nextWaveNumber, waveSetNumber);
                if (nextWaveNumber % 5 == 1) // i think this is each "6th" wave
                {
                    waveSetNumber++;
                    Debug.Log("Increased wavesetnumber to " + waveSetNumber);
                }
            }

            //finally, add wave to the list of waves
            waves.Add(new InfiniteWave(nextWaveNumber, ducksThisWave, duckSpeed, waveTime, nextWaveType));

            thisWave++;
            SetupWave();

            // get ready to begin
            state = WaveState.READY;
        }
    }

    void RunFirstFiveWaveLogic(int _nextWaveNumber)
    {
        #region logic: first 5 waves (Ducks)
        // 1 - Normal (3 norm)
        // 2 - Survival (4 norm) // TODO: create survival bonus
        // 3 - Normal (6 norm)
        // 4 - Normal (3 - norm, 3 - fast)
        // 5 - Bonus Geese (1v)
        #endregion

        // TODO: make switch case
        // wave 1 was set up in OnEnable, so start on wave 2
        if (_nextWaveNumber == 2)
        {
            nextWaveType = InfiniteWave.WaveType.SURVIVAL;
            ducksThisWave = 4;
        }
        else if (_nextWaveNumber == 3)
        {
            nextWaveType = InfiniteWave.WaveType.NORMAL;
            ducksThisWave = 6;
        }
        else if (_nextWaveNumber == 4)
        {
            nextWaveType = InfiniteWave.WaveType.NORMAL;
            ducksThisWave = 6;
            fastMultiplier = 0.5f;
        }
        else if (_nextWaveNumber == 5)
        {
            nextWaveType = InfiniteWave.WaveType.BONUS;
            ducksThisWave = 1; // bonus geese start with 1 V
            waveTime = bonusWaveTime;
        }
    }

    void RunInfiniteWaveLogic(int _nextWaveNumber, int _repeatSetCount)
    {
        #region logic: repeats for remaining waves
        // 6 - 10: 1/2: normies, fast
        // 10-15: 1/3: normie, fast, angry (*1.5)
        // 15-20: 1/2 fast, angry (*2)
        // 20-25: 1/3 fast, 2/3 angry (*3)
        // 25+: angry only (*5)

        // 6 - Normal
        // 7 - Survival
        // 8 - Golden Goose
        // 9 - Normal
        // 10 - Bonus Goose
        #endregion

        // Set Wave Multipliers & Base Duck Counts
        // TODO: tweak duckmultipliers, likely too high
        switch (_repeatSetCount)
        {
            case 1: // waves 6-10: 1/2 normies, fast
                duckMultiplier = 1f;
                duckBase = UnityEngine.Random.Range(6, 12);
                normieMultiplier = fastMultiplier = (0.5f);
                break;
            case 2: // waves 10-15: 1/3: normie, fast, angry (*1.5)
                duckMultiplier = 1.5f;
                duckBase = UnityEngine.Random.Range(9, 15);
                normieMultiplier = fastMultiplier = angryMultiplier = (0.33f);
                break;
            case 3: // waves 15-20: 1/2 fast, angry (*2)
                duckMultiplier = 2f;
                duckBase = UnityEngine.Random.Range(12, 17);
                fastMultiplier = angryMultiplier = (0.5f);
                break;
            case 4: // waves 20-25: 1/3 fast, 2/3 angry (*3)
                duckMultiplier = 3f;
                duckBase = UnityEngine.Random.Range(16, 20);
                fastMultiplier = (0.33f);
                angryMultiplier = (0.66f);
                break;
            default: // waves 25+: angry only 
                duckMultiplier = 5f;
                duckBase = UnityEngine.Random.Range(17, 25);
                angryMultiplier = 1f;
                break;
        }

        // Set this wave's ducks before adding
        ducksThisWave = (int)(duckMultiplier * duckBase);
        waves[thisWave].normieDucks = (int)(normieMultiplier * ducksThisWave);
        waves[thisWave].fastDucks = (int)(fastMultiplier * ducksThisWave);
        waves[thisWave].angryDucks = (int)(angryMultiplier * ducksThisWave);

        // assign wave type by wave number
        if ((_nextWaveNumber % 5) == 2) // TODO: make survival give extra bonus if no damage taken by player
        {
            nextWaveType = InfiniteWave.WaveType.SURVIVAL;
        }
        else if ((_nextWaveNumber % 5) == 3)
        {
            nextWaveType = InfiniteWave.WaveType.GOLDEN;
            waves[thisWave].goldenGeese = _repeatSetCount; // 1 goldengoose for each set of levels (e.g., 6-10)
        }
        else if ((_nextWaveNumber % 5) == 0) // bonus geese
        {
            nextWaveType = InfiniteWave.WaveType.BONUS;
            ducksThisWave = _repeatSetCount; // 1 flying V for each set of levels (e.g., 6-10)
            waveTime = bonusWaveTime;
        }
        else
        {
            nextWaveType = InfiniteWave.WaveType.NORMAL; // waves 1 & 4 are normal
        }
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
            StopAllCoroutines(); // stop launching ducks - TODO: check if this is still needed, might be relic of original "flight"
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

    void ChooseDuckToSpawn()
    {
        // logic to select duck from possible ducks, remove that entry
        if (UnityEngine.Random.value < angryMultiplier)
        {
            SpawnDuck(spawnPoints, DuckManager.instance.angryDuck);
        }
        else if (UnityEngine.Random.value < (angryMultiplier + fastMultiplier))
        {
            SpawnDuck(spawnPoints, DuckManager.instance.fastDuck);
        }
        else
        {
            SpawnDuck(spawnPoints, DuckManager.instance.normDuck);
        }
    }
    
    void SpawnDuck(GameObject[] _spawnPoints, GameObject duckToLaunch)
    {
        GameObject activeSpawnPoint = _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Length)];
        DuckLauncher duckLauncher = activeSpawnPoint.GetComponent<DuckLauncher>();
        duckLauncher.LaunchObj(duckToLaunch);
    }

    // SINGLESCENE: used on "Play Again"
    public void ResetWaves()
    {
        if (waves.Count > 1)
        {
            waves.RemoveRange(1, waves.Count - 1);
        }
    }
}