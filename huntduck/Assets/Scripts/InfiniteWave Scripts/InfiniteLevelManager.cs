using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteLevelManager : MonoBehaviour
{
    public WeaponsManager weaponsManager;

    public GameObject helperUI;
    public Text helperText;
    public GameObject congratsUI;
    public GameObject gameUI; // this UI shows waves, time, ducks, score
    public Canvas walletCanvas;
    public GameObject gameOverUI; // "Game Over"
    public GameObject endLevelUI; // replay & exit button, score rollup

    public InfiniteWaveSpawner infiniteWaveSpawner;

    public AudioSource levelupSound;

    void Start()
    {
        StartIntro(); 
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            Debug.Log("Yo final score was " + PlayerPrefs.GetInt("FinalScore"));
        }
    }

    // used to be called in BeginLevelTrigger.cs
    public void StartIntro()
    {
        Debug.Log("LET THE GAMES BEGIN!!");
        StartCoroutine(InfiniteWaveIntro());
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
        StartCoroutine(BeginInfiniteWave());
    }

    void EndInfiniteWave()
    {
        StartCoroutine(InfiniteWaveOutro());
    }


    IEnumerator InfiniteWaveIntro()
    {
        helperText.text = "Welcome to\nHunt Duck";
        yield return new WaitForSecondsRealtime(3);
        helperText.text = "Select your weapon to begin";
    }

    IEnumerator BeginInfiniteWave()
    {
        helperText.text = "Prepare to Hunt!";
        yield return new WaitForSecondsRealtime(3f);
        helperUI.SetActive(false);

        infiniteWaveSpawner.enabled = true;
        gameUI.SetActive(true);
        yield return null;
    }

    IEnumerator InfiniteWaveOutro()
    {
        string finalScore = WalletUI.walletScore;
        int finalScoreInt = PlayerScore.playerScore;

        // TODO: consider highest score implementation for PlayerPrefs
        // TODO: determine whether PlayerPrefs is local storage, and/or the correct storage for scores
        PlayerPrefs.SetInt("FinalScore", finalScoreInt);
        Debug.Log("Saving final score in PlayerPrefs as " + PlayerPrefs.GetInt("FinalScore"));

        // wave ends, hide game UI
        infiniteWaveSpawner.enabled = false;
        gameUI.SetActive(false);

        // GAME OVER UI
        gameOverUI.SetActive(true);
        yield return new WaitForSeconds(3);
        gameOverUI.SetActive(false);

        // show final UI with score rollup
        helperUI.SetActive(true);
        endLevelUI.SetActive(true);
        helperText.text = "Your final score is " + finalScore;
        levelupSound.Play();
        yield return new WaitForSecondsRealtime(3);
    }
}