using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteLevelManager : MonoBehaviour
{
    private enum PracticeState { INTRO, INFINITEWAVE, END };
    private PracticeState state;

    public WeaponsManager weaponsManager;

    public GameObject helperUI;
    public Text helperText;
    public GameObject congratsUI;
    public Canvas walletCanvas;

    public InfiniteWaveSpawner infiniteWaveSpawner;

    public AudioSource levelupSound;


    // called in BeginLevelTrigger.cs
    public void StartIntro()
    {
        Debug.Log("LET THE GAMES BEGIN!!");
        StartCoroutine(InfiniteWaveIntro());
    }

    void Update()
    {
        if (state == PracticeState.INTRO)
        {

        }

        if (state == PracticeState.INFINITEWAVE)
        {
            // run wave logic
        }

        if (state == PracticeState.END)
        {

        }

        //if (state == PracticeState.END)
        //{
        //    // check if we have shot all the carni ducks
        //    if (cduckList.Count == 0)
        //    {
        //        carniDucks.SetActive(false);
        //        EndPracticeSession();
        //    }
        //    // carni ducks still left
        //    Debug.Log("We still got " + cduckList.Count + " cducks left!");
        //    return;
        //}
    }

    void OnEnable()
    {
        // onWeaponsSelected callback happens in SnapZone.cs
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
        state = PracticeState.INTRO;

        helperText.text = "Welcome to the Infinite Wave";
        yield return new WaitForSecondsRealtime(3);
        helperText.text = "Select your weapon to begin";
    }

    IEnumerator BeginInfiniteWave()
    {
        state = PracticeState.INFINITEWAVE;

        helperText.text = "Welcome to the Infinite Wave";
        yield return new WaitForSecondsRealtime(3);
        helperUI.SetActive(false);

        infiniteWaveSpawner.enabled = true;
    }

    IEnumerator InfiniteWaveOutro()
    {
        state = PracticeState.END;
        infiniteWaveSpawner.enabled = false;

        congratsUI.SetActive(true);
        yield return new WaitForSeconds(3);
        congratsUI.SetActive(false);

        helperUI.SetActive(true);
        helperText.text = "Your final score is " + WalletUI.walletScore;
        yield return new WaitForSecondsRealtime(3);
        levelupSound.Play();

        // present "Play again" UI
    }
}
