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

    public InfiniteWaveSpawner infiniteWaveSpawner;

    public AudioSource levelupSound;


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
        walletUIObj.SetActive(false);
        infiniteWaveSpawner.enabled = false;

        congratsUI.SetActive(true);
        yield return new WaitForSeconds(3);
        congratsUI.SetActive(false);

        helperUI.SetActive(true);
        helperText.text = "Your final score is " + finalScore;
        yield return new WaitForSecondsRealtime(3);
        levelupSound.Play();

        // present "Play again" UI
    }
}
