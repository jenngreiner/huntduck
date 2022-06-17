using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using huntduck;

public class InfiniteLevelManager : MonoBehaviour
{
    // Singleton
    public static InfiniteLevelManager instance { get; private set; }

    [Header("Gameplay UI")]
    public GameObject helperUI;
    public Text helperText;
    public GameObject congratsUI;
    public GameObject gameplayUI; // this UI shows ducks, time, score
    public Text duckText; // duck count on sign
    public Text timeText; // time on sign
    public Text waveText;
    public Canvas walletCanvas;
    public GameObject gameOverUI;
    public GameObject replayExitUI;

    [Header("Leaderboard")]
    public GameObject leaderboard;
    public TextMeshProUGUI finalWavesText;
    public TextMeshProUGUI finalDucksText;
    public TextMeshProUGUI finalBucksText;

    [Header("Audio")]
    public AudioSource levelupSound;

    [Header("Wave Spawner")]
    public SurvivalWaveSpawner survivalWaveSpawner;

    private PlayerData playerData;

    public delegate void StartInfinite();
    public static event StartInfinite onStartInfinite;


    void Awake()
    {
        // if there is an instance, and it isn't me, delete me
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        // get the playerscore script on player object
        playerData = ObjectManager.instance.player;

        // SINGLESCENE: DISABLED
        //StartInfiniteWave();
    }

    void OnEnable()
    {
        // SINGLESCENE: reset the gameplay UIs and start
        ResetText();

        ChooseGameMode.onSwitchMode += StartInfiniteWave;
        RestartGameMode.onRestartMode += StartInfiniteWave;
        SurvivalWaveSpawner.onGameOver += EndInfiniteWave;
    }

    void OnDisable()
    {
        ChooseGameMode.onSwitchMode -= StartInfiniteWave;
        RestartGameMode.onRestartMode -= StartInfiniteWave;
        SurvivalWaveSpawner.onGameOver -= EndInfiniteWave;

        // SINGLESCENE: hide scoreboards on "Play Again"
        leaderboard.SetActive(false);
    }

    public void ResetText()
    {
        duckText.text = "-";
        timeText.text = "-";
        waveText.text = "-";
    }

    void StartInfiniteWave()
    {
        onStartInfinite?.Invoke();
        StartCoroutine(BeginInfiniteWave());
    }

    void EndInfiniteWave()
    {
        StartCoroutine(InfiniteWaveOutro());
    }

    IEnumerator BeginInfiniteWave()
    {
        helperUI.SetActive(true);
        helperText.text = "PREPARE TO HUNT";
        yield return new WaitForSecondsRealtime(3f);
        helperUI.SetActive(false);

        survivalWaveSpawner.enabled = true;
        gameplayUI.SetActive(true);
        yield return null;
    }

    IEnumerator InfiniteWaveOutro()
    {
        string finalScore = WalletUI.walletScore;
        int finalScoreInt = playerData.score;
        uint finalScoreUInt = (uint)playerData.score;

        // TODO: consider highest score implementation for PlayerPrefs
        // TODO: determine whether PlayerPrefs is local storage, and/or the correct storage for scores
        PlayerPrefs.SetInt("FinalScore", finalScoreInt);
        Debug.Log("Saving final score in PlayerPrefs as " + PlayerPrefs.GetInt("FinalScore"));

        // GAME OVER UI
        gameOverUI.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
        gameOverUI.SetActive(false);

        // show final UI with score rollup
        huntduck.PlatformManager.Leaderboards.SubmitMatchScores(finalScoreUInt);
        finalWavesText.text = survivalWaveSpawner.waves.Count.ToString();
        finalDucksText.text = survivalWaveSpawner.ducksHitTotal.ToString();
        finalBucksText.text = finalScore;

        // query for latest scores - this doesnt seem to be working yet
        huntduck.PlatformManager.Leaderboards.QueryHighScoreLeaderboard();

        // set leaderboard & buttons active
        leaderboard.SetActive(true);
        replayExitUI.SetActive(true);

        levelupSound.Play();
        yield return new WaitForSecondsRealtime(3f);
    }
}