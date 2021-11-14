using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PracticeRangeManager : MonoBehaviour
{
    private enum PracticeState { INTRO, TARGET, CLAY, DUCKS, END };
    private PracticeState state;

    public WeaponsManager weaponsManager;

    //public BeginTargetTrigger beginTargetTrigger;

    public GameObject helperUI;
    public Text helperText;
    public GameObject congratsUI;
    public Canvas walletCanvas;
    public GameObject endLevelUI; // replay & exit button

    public GameObject targetWall;
    public List<GameObject> targetList;

    public PracticeWaveSpawner clayWavesManager;

    public GameObject carniDucks;
    public List<GameObject> cduckList;

    public AudioSource levelupSound;


    void Start()
    {
        //SetupRound();
        Debug.Log("Start method complete");
    }

    void Update()
    {
        if (state == PracticeState.TARGET)
        {
            // check if we have hit all the targets
            if (targetList.Count == 0)
            {
                RespawnTargets();
                targetWall.SetActive(false);
                StartClayRound();
            }
            // targets still left
            return;
        }

        if (state == PracticeState.CLAY)
        {
            Debug.Log("We've hit " + clayWavesManager.claysHit + " clays");

            // check if we have hit 3 clays
            if (clayWavesManager.claysHit >= 3)
            {
                Debug.Log("Hit 3 clays, moving on to ducks");
                // stop waves, go on to next round
                //clayWavesManager.enabled = false;
                StartCarniDucks();
            }
            return;
        }

        if (state == PracticeState.DUCKS)
        {
            // check if we have shot all the carni ducks
            if (cduckList.Count == 0)
            {
                RespawnDucks();
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
        BNG.Damageable.onCarniDuckHit += RemoveCarniDuck;
        BNG.Damageable.onTargetHit += RemoveTarget;

        // these events are needed when switching game mode in the same scene
        ChooseGameMode.onSwitchMode += StartPracticeSession;
        RestartGameMode.onRestartMode += StartPracticeSession;
        //ExitGameMode.onExitMode += ResetPracticeSession;
        //MoveGameWorld.onWorldPosition1Reached += StartPracticeSession;
    }

    void OnDisable()
    {
        BNG.Damageable.onCarniDuckHit -= RemoveCarniDuck;
        BNG.Damageable.onTargetHit -= RemoveTarget;

        // these events are needed when switching game mode in the same scene
        ChooseGameMode.onSwitchMode -= StartPracticeSession;
        RestartGameMode.onRestartMode -= StartPracticeSession;
        //ExitGameMode.onExitMode -= ResetPracticeSession;
        //MoveGameWorld.onWorldPosition1Reached -= StartPracticeSession;
    }

    void SetupRound()
    {
        // set up lists to track gameobjects you shoot
        targetList = new List<GameObject>();
        cduckList = new List<GameObject>();

        foreach (Transform child in targetWall.transform)
        {
            targetList.Add(child.gameObject);
            Debug.Log("Added " + child.gameObject + " to targetlist");
            Debug.Log("We've got " + targetList.Count + "targets to shoot");
        }

        foreach (Transform child in carniDucks.transform)
        {
            cduckList.Add(child.gameObject);
            Debug.Log("Added " + child.gameObject + " to cducklist");
            Debug.Log("We've got " + cduckList.Count + "carniducks to shoot");
        }

        walletCanvas.enabled = false;
        carniDucks.gameObject.SetActive(false);

        Debug.Log("SetupRound complete");
    }

    void RespawnTargets()
    {
        foreach (Transform target in targetWall.transform)
        {
            Debug.Log("target found named " + target.name);
            BNG.Damageable damageableScript = target.GetComponent<BNG.Damageable>();
            damageableScript.InstantRespawn();
        }
    }

    void RespawnDucks()
    {
        foreach (Transform cduck in carniDucks.transform)
        {
            Transform duck = cduck.GetChild(0);
            Debug.Log("duck found named " + duck.name);
            BNG.Damageable damageableScript = duck.GetComponent<BNG.Damageable>();
            damageableScript.InstantRespawn();
        }
    }

    //void PrepTargetRound()
    //{
    //    beginTargetTrigger.isTargetReady = true;
    //    // helperText.text = "Step up to the podium to start!";
    //}

    void StartPracticeSession()
    {
        SetupRound();
        StartCoroutine(PracticeRangeIntro());
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

    //void RestartPracticeSession()
    //{
    //    SetupRound();
    //    StartPracticeSession();
    //}

    //void ResetPracticeSession()
    //{
    //    SetupRound();
    //}

    void RemoveCarniDuck(GameObject carniDuck)
    {
        cduckList.Remove(carniDuck);
        Debug.Log("One less carniduck in cduck list! Count is now " + cduckList.Count);
    }

    void RemoveTarget(GameObject target)
    {
        targetList.Remove(target);
        Debug.Log("One less target in target list! Count is now " + targetList.Count);
    }

    IEnumerator PracticeRangeIntro()
    {
        yield return new WaitForSeconds(0.1f); // within same scene, we gotta let the first frame load when switching over from selectmode
        Debug.Log("Starting PracticeRange Intro");
        state = PracticeState.INTRO;
        helperUI.SetActive(true);

        helperText.text = "Welcome to the Practice Range!";
        yield return new WaitForSeconds(3);
        StartTargetRound();
        Debug.Log("PracticeRangeIntro fired StartTargetRound");
    }

    IEnumerator TargetRoundIntro()
    {
        state = PracticeState.TARGET;
        helperText.text = "Shoot ALL targets to advance!";
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
        helperText.text = "Shoot 3 clays to advance!";
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

        // walletCanvas.enabled = true;
        helperUI.SetActive(true);
        helperText.text = "Shoot the ducks to make some bucks!";
        yield return new WaitForSeconds(3);

        helperText.text = "Make $2,100 to buy your duck license!";
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

        endLevelUI.SetActive(true);

        helperText.text = "You have unlocked your duck license!";
        levelupSound.Play();
    }
}