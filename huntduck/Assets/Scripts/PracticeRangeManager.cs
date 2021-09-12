using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// The BeginGameUI is set from WaveSpawner.cs
public class PracticeRangeManager : MonoBehaviour
{
    public GameObject helperUI;
    public Text helperText;
    public Canvas walletCanvas;

    public GameObject targetWall;
    public WaveSpawner clayWaves;
    public GameObject carniDucks;

    public WeaponsManager weaponsManager;

    void Start()
    {
        SetupRound();
    }

    void OnEnable()
    {
        // callback happens in SnapZone.cs
        WeaponsManager.onWeaponSelected += PrepTargetRound;
    }

    void OnDisable()
    {
        WeaponsManager.onWeaponSelected -= PrepTargetRound;
    }

    void SetupRound()
    {
        walletCanvas.gameObject.SetActive(false);
        carniDucks.gameObject.SetActive(false);
    }

    // called in BeginGameTrigger.cs
    public void BeginGame()
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

    public void BeginCarniDucks()
    {
        // TODO: add some UI here about getting money for ducks

        walletCanvas.enabled = true;
        carniDucks.SetActive(true);
    }

    IEnumerator PracticeRangeIntro()
    {
        helperText.text = "Welcome to the Practice Range!";
        yield return new WaitForSeconds(3);
        helperText.text = "Select your weapon behind you";
    }

    IEnumerator TargetRoundIntro()
    {
        helperText.text = "Shoot the targets to advance!";
        yield return new WaitForSeconds(3);
        helperUI.SetActive(false);
        targetWall.SetActive(true);
    }
}