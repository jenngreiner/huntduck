using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PracticeRangeManager : MonoBehaviour
{
    private enum PracticeState { INTRO, TARGET, CLAY, DUCKS, END };
    private PracticeState state;

    public WeaponsManager weaponsManager;

    public GameObject helperUI;
    public Text helperText;
    public GameObject congratsUI;
    public Canvas walletCanvas;

    public GameObject targetWall;
    public static List<GameObject> targetList;

    public PracticeWaveSpawner clayWavesManager;

    public GameObject carniDucks;
    public static List<GameObject> cduckList;

    public AudioSource levelupSound;


    void Start()
    {
        SetupRound();
    }

    void Update()
    {
        if (state == PracticeState.TARGET)
        {
            // check if we have hit all the targets
            if (targetList.Count == 0)
            {
                StartClayRound();
            }
            // targets still left
            return;
        }

        if (state == PracticeState.CLAY)
        {
            // check if we have hit 3 clays
            if (PracticeWaveSpawner.claysHit >= 3)
            {
                // stop waves, go on to next round
                clayWavesManager.enabled = false;
                StartCarniDucks();
            }
            Debug.Log("We've hit " + PracticeWaveSpawner.claysHit + "clays");
            return;
        }

        if (state == PracticeState.DUCKS)
        {
            // check if we have shot all the carni ducks
            if (cduckList.Count == 0)
            {
                carniDucks.SetActive(false);
                EndPracticeSession();
            }
            // carni ducks still left
            Debug.Log("We still got " + cduckList.Count + " cducks left!");
            return;
        }
    }

    void OnEnable()
    {
        // onWeaponsSelected callback happens in SnapZone.cs
        WeaponsManager.onWeaponSelected += PrepTargetRound;
        PracticeWaveSpawner.onClayWavesComplete += StartCarniDucks;
    }

    void OnDisable()
    {
        WeaponsManager.onWeaponSelected -= PrepTargetRound;
        PracticeWaveSpawner.onClayWavesComplete -= StartCarniDucks;
    }

    void SetupRound()
    {
        // set up lists to track gameobjects you shoot
        targetList = new List<GameObject>();
        cduckList = new List<GameObject>();

        foreach (Transform child in targetWall.transform)
        {
            targetList.Add(child.gameObject);
        }

        foreach (Transform child in carniDucks.transform)
        {
            cduckList.Add(child.gameObject);
            Debug.Log("Added " + child.gameObject + " to cducklist");
            Debug.Log("We've got " + cduckList.Count + "carniducks to shoot");
        }

        walletCanvas.enabled = false;
        carniDucks.gameObject.SetActive(false);
    }

    // called in BeginGameTrigger.cs
    public void StartPractice()
    {
        Debug.Log("LET THE GAMES BEGIN!!");
        StartCoroutine(PracticeRangeIntro());
    }

    void PrepTargetRound()
    {
        BeginTargetTrigger.isTargetReady = true;
        helperText.text = "Step up to the podium to start!";
    }

    // called in BeginTargetTrigger.cs
    public void StartTargetRound()
    {
        StartCoroutine(TargetRoundIntro());
    }

    void StartClayRound()
    {
        StartCoroutine(ClayRoundIntro());
    } 

    public void StartCarniDucks()
    {
        StartCoroutine(CarniDuckIntro());
    }

    void EndPracticeSession()
    {
        StartCoroutine(EndPracticeOutro());
    }


    IEnumerator PracticeRangeIntro()
    {
        state = PracticeState.INTRO;
        helperText.text = "Welcome to the Practice Range!";
        yield return new WaitForSeconds(3);
        helperText.text = "Select your weapon behind you";
    }

    IEnumerator TargetRoundIntro()
    {
        state = PracticeState.TARGET;
        helperText.text = "Shoot the targets to advance!";
        yield return new WaitForSeconds(3);

        helperUI.SetActive(false);
        targetWall.SetActive(true);
    }

    IEnumerator ClayRoundIntro()
    {
        state = PracticeState.CLAY;
        congratsUI.SetActive(true);
        yield return new WaitForSeconds(3);
        congratsUI.SetActive(false);

        helperUI.SetActive(true);
        helperText.text = "Shoot the clays to advance!";
        yield return new WaitForSeconds(3);

        helperUI.SetActive(false);
        clayWavesManager.enabled = true;
    }

    IEnumerator CarniDuckIntro()
    {
        state = PracticeState.DUCKS;
        congratsUI.SetActive(true);
        yield return new WaitForSeconds(3);
        congratsUI.SetActive(false);

        walletCanvas.enabled = true;
        helperUI.SetActive(true);
        helperText.text = "Shoot the ducks to make some bucks!";
        yield return new WaitForSeconds(3);

        helperUI.SetActive(false);
        carniDucks.SetActive(true);
        Debug.Log("Carni ducks are alive!!!");
    }

    IEnumerator EndPracticeOutro()
    {
        state = PracticeState.END;
        congratsUI.SetActive(true);
        yield return new WaitForSeconds(3);
        congratsUI.SetActive(false);

        helperUI.SetActive(true);
        helperText.text = "You have completed the practice round!";
        yield return new WaitForSeconds(3);

        helperText.text = "You have unlocked your duck license!";
        levelupSound.Play();
    }
}