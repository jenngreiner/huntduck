using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PracticeRangeManager : MonoBehaviour
{
    private enum PracticeState { INTRO, TARGET, CLAY, DUCKS, END };
    private PracticeState state;

    public WeaponsManager weaponsManager;

    public BeginTargetTrigger beginTargetTrigger;

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
        SetupRound();
        StartCoroutine(PracticeRangeIntro());
    }

    // called in BeginGameTrigger.cs
    public void StartPractice()
    {
        Debug.Log("LET THE GAMES BEGIN!!");
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
            if (clayWavesManager.claysHit >= 3)
            {
                // stop waves, go on to next round
                //clayWavesManager.enabled = false;
                StartCarniDucks();
            }
            Debug.Log("We've hit " + clayWavesManager.claysHit + " clays");
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
        BNG.Damageable.onCarniDuckHit += RemoveCarniDuck;
        BNG.Damageable.onTargetHit += RemoveTarget;
    }

    void OnDisable()
    {
        WeaponsManager.onWeaponSelected -= PrepTargetRound;
        BNG.Damageable.onCarniDuckHit -= RemoveCarniDuck;
        BNG.Damageable.onTargetHit -= RemoveTarget;
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

    void PrepTargetRound()
    {
        beginTargetTrigger.isTargetReady = true;
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
        state = PracticeState.INTRO;
        helperText.text = "Welcome to the Practice Range!";
        yield return new WaitForSeconds(3);
        //commenting out and jumping to target round for single scene experiment
        //helperText.text = "Select your weapon behind you";
        StartCoroutine(TargetRoundIntro());
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