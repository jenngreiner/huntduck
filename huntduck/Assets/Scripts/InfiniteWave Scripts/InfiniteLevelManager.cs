using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfiniteLevelManager : MonoBehaviour
{
    //public WeaponsManager weaponsManager;
    //private LeaderboardManager ilm_leaderboards;

    public GameObject helperUI;
    public Text helperText;
    public GameObject congratsUI;
    public GameObject gameplayUI; // this UI shows waves, time, ducks, score
    public Canvas walletCanvas;
    public GameObject gameOverUI; // "Game Over"
    public GameObject endLevelUI; // replay & exit button
    //public GameObject weaponsWall;

    public GameObject howYouDidUI;
    public TextMeshProUGUI finalWavesText;
    public TextMeshProUGUI finalDucksText;
    public TextMeshProUGUI finalBucksText;

    //public GameObject myScoresUI;
    public GameObject highestScoresUI;
    //public GameObject allScoresUI;

    public InfiniteWaveSpawner infiniteWaveSpawner;

    public AudioSource levelupSound;
    private PlayerScore playerScoreScript;


    void Start()
    {
        // TODO: some day don't be lazy, make a TagManager
        // get the playerscore script on player object
        playerScoreScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScore>();

        StartIntro(); 
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            EndInfiniteWave();
            Debug.Log("This shi is ova!");
        }
    }

    // used to be called in BeginLevelTrigger.cs, now called on Start()
    public void StartIntro()
    {
        // commenting out for single scene experiment
        //StartCoroutine(InfiniteWaveIntro());
        StartInfiniteWave();
    }

    void OnEnable()
    {
        WeaponsManager.onWeaponSelected += StartInfiniteWave;
        InfiniteWaveSpawner.onGameOver += EndInfiniteWave;
    }

    void OnDisable()
    {
        WeaponsManager.onWeaponSelected -= StartInfiniteWave;
        InfiniteWaveSpawner.onGameOver -= EndInfiniteWave;
    }

    void StartInfiniteWave()
    {
        //weaponsWall.SetActive(false);
        StartCoroutine(BeginInfiniteWave());
    }

    void EndInfiniteWave()
    {
        StartCoroutine(InfiniteWaveOutro());
    }


    IEnumerator InfiniteWaveIntro()
    {
        helperText.text = "WELCOME TO\n THE HUNT";
        yield return new WaitForSecondsRealtime(3);
        helperText.text = "SELECT YOUR WEAPON TO BEGIN";
        //weaponsWall.SetActive(true);
    }

    IEnumerator BeginInfiniteWave()
    {
        helperText.text = "PREPARE TO HUNT";
        yield return new WaitForSecondsRealtime(3f);
        helperUI.SetActive(false);

        infiniteWaveSpawner.enabled = true;
        gameplayUI.SetActive(true);
        yield return null;
    }

    IEnumerator InfiniteWaveOutro()
    {
        string finalScore = WalletUI.walletScore;
        int finalScoreInt = playerScoreScript.playerScore;
        uint finalScoreUInt = (uint)playerScoreScript.playerScore;

        // TODO: consider highest score implementation for PlayerPrefs
        // TODO: determine whether PlayerPrefs is local storage, and/or the correct storage for scores
        PlayerPrefs.SetInt("FinalScore", finalScoreInt);
        Debug.Log("Saving final score in PlayerPrefs as " + PlayerPrefs.GetInt("FinalScore"));

        // wave ends, hide game UI
        infiniteWaveSpawner.enabled = false;

        // GAME OVER UI
        gameOverUI.SetActive(true);
        yield return new WaitForSeconds(3);
        gameOverUI.SetActive(false);

        // show final UI with score rollup
        huntduck.PlatformManager.Leaderboards.SubmitMatchScores(finalScoreUInt);
        finalWavesText.text = (infiniteWaveSpawner.waves.Count).ToString();
        finalDucksText.text = infiniteWaveSpawner.ducksHitTotal.ToString();
        finalBucksText.text = finalScore;
        howYouDidUI.SetActive(true);
        endLevelUI.SetActive(true);

        // query for latest scores - this doesnt seem to be working yet
        huntduck.PlatformManager.Leaderboards.QueryHighScoreLeaderboard();
        // set leaderboard active
        highestScoresUI.SetActive(true);


        levelupSound.Play();
        yield return new WaitForSecondsRealtime(3);
    }
}