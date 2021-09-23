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
    public GameObject walletUIObj;
    public Canvas walletCanvas;
    public GameObject endLevelUI;

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

    // called in BeginLevelTrigger.cs
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
        helperText.text = "Welcome to the Infinite Wave";
        yield return new WaitForSecondsRealtime(3);
        helperText.text = "Select your weapon to begin";
    }

    IEnumerator BeginInfiniteWave()
    {
        helperText.text = "Welcome to the Infinite Wave";
        yield return new WaitForSecondsRealtime(3);
        helperUI.SetActive(false);

        infiniteWaveSpawner.enabled = true;
    }

    IEnumerator InfiniteWaveOutro()
    {
        string finalScore = WalletUI.walletScore;
        int finalScoreInt = PlayerScore.playerScore;

        // TODO: consider highest score implementation for PlayerPrefs
        // TODO: determine whether PlayerPrefs is local storage, and/or the correct storage for scores
        PlayerPrefs.SetInt("FinalScore", finalScoreInt);
        Debug.Log("Saving final score in PlayerPrefs as " + PlayerPrefs.GetInt("FinalScore"));

        walletUIObj.SetActive(false);
        infiniteWaveSpawner.enabled = false;

        congratsUI.SetActive(true);
        yield return new WaitForSeconds(3);
        congratsUI.SetActive(false);

        helperUI.SetActive(true);
        endLevelUI.SetActive(true);
        helperText.text = "Your final score is " + finalScore;
        yield return new WaitForSecondsRealtime(3);
        levelupSound.Play();

        // present "Play again" UI
    }
}