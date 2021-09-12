using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// The BeginGameUI is set from WaveSpawner.cs
public class PracticeRangeManager : MonoBehaviour
{
    public GameObject helperUI;
    public Text helperText;
    public WaveSpawner clayWaves;
    public WeaponsManager weaponsManager;
    public Canvas walletCanvas;
    public GameObject carniDucks;

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

    void PrepTargetRound()
    {
        helperText.text = "Step up to the podium to start!";
    }

    public void StartTargetRound()
    {
        helperText.text = "Shoot the targets to advance!";
        //clayWaves.enabled = true;
    }

    // this is called in BeginGameTrigger.cs
    public void BeginGame()
    {
        Debug.Log("LET THE GAMES BEGIN!!");
        StartCoroutine(PracticeRangeIntro());
    }

    IEnumerator PracticeRangeIntro()
    {
        helperText.text = "Welcome to the Practice Range!";
        yield return new WaitForSeconds(3);
        helperText.text = "Select your weapon behind you";
    }

    public void BeginCarniDucks()
    {
        // TODO: add some UI here about getting money for ducks

        walletCanvas.enabled = true;
        carniDucks.SetActive(true);
    }
}