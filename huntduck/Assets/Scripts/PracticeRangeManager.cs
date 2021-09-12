using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// The BeginGameUI is set from WaveSpawner.cs
public class PracticeRangeManager : MonoBehaviour
{
    private enum PracticeState { INTRO, TARGET, CLAY, DUCKS, END };
    private PracticeState state;

    public GameObject helperUI;
    public Text helperText;
    public Canvas walletCanvas;
    public WeaponsManager weaponsManager;

    public GameObject targetWall;
    public static List<GameObject> targetList;

    //[System.Serializable]
    //public class ClayWave
    //{
    //    public int count;
    //    public float rate;
    //}
    //private ClayWave[] clayWaves;
    //public static int claysHit;
    //public GameObject[] spawnPoints;

    public PracticeWaveSpawner clayWavesManager;

    public GameObject carniDucks;


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
            // tagets still left
            Debug.Log("We still got " + targetList.Count + "targets left!");
            return;
        }

        if (state == PracticeState.CLAY)
        {
            // check if we have hit 3 clays
            if (PracticeWaveSpawner.claysHit >= 3)
            {
                // go on to next round
                StartCarniDucks();
            }
            Debug.Log("We still got clays left!");
            return;
        }
    }

    void OnEnable()
    {
        // callback happens in SnapZone.cs
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
        targetList = new List<GameObject>();

        foreach (Transform child in targetWall.transform)
        {
            targetList.Add(child.gameObject);
            Debug.Log("Added " + child.gameObject + " to targetList");
            Debug.Log("We've got " + targetList.Count + "targets to shoot");
        }

        //clayWave = new ClayWave();
        //claysHit = 0;

        walletCanvas.gameObject.SetActive(false);
        carniDucks.gameObject.SetActive(false);
    }

    // called in BeginGameTrigger.cs
    public void StartGame()
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
        //StartCoroutine(SpawnClayWave());
    } 

    public void StartCarniDucks()
    {
        // TODO: add some UI here about getting money for ducks

        walletCanvas.enabled = true;
        carniDucks.SetActive(true);
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
        //claysHit = 0;
        helperUI.SetActive(true);
        Debug.Log("Clay helper UI should now be showing");
        helperText.text = "Shoot the clays to advance!";
        yield return new WaitForSeconds(3);
        helperUI.SetActive(false);
        clayWavesManager.enabled = true;
    }

    //IEnumerator SpawnClayWave()
    //{
    //    Debug.Log("Spawning Clay Wave");

    //    for (int i = 0; i < clayWaves[0].count; i++)
    //    {
    //        SpawnClay();
    //        yield return new WaitForSeconds(1 / clayWaves[0].rate);
    //    }
    //}

    //void SpawnClay()
    //{
    //    Debug.Log("Spawning clays");
    //}

}