using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// The BeginGameUI is set from WaveSpawner.cs
public class PracticeRangeManager : MonoBehaviour
{
    public GameObject beginGameUI;
    public Text beginGameText;
    public WaveSpawner roundWaves;
    public WeaponsManager weaponsManager;
    public Canvas walletCanvas;
    public GameObject carniDucks;

    void Start()
    {
        SetupPracticeRound();
    }

    void OnEnable()
    {
        WeaponsManager.onWeaponSelected += HideBeginUI;
        WeaponsManager.onWeaponSelected += StartRound;
        WeaponsManager.onWeaponSelected += StartPracticeRound;
    }

    void OnDisable()
    {
        WeaponsManager.onWeaponSelected -= HideBeginUI;
        WeaponsManager.onWeaponSelected -= StartRound;
        WeaponsManager.onWeaponSelected -= StartPracticeRound;
    }

    void SetupPracticeRound()
    {
        walletCanvas.gameObject.SetActive(false);
        carniDucks.gameObject.SetActive(false);
    }

    void StartPracticeRound()
    {
        walletCanvas.gameObject.SetActive(true);
        carniDucks.gameObject.SetActive(true);
    }


    // this is called in BeginGameTrigger.cs
    public void BeginGame(string GameMode)
    {
        Debug.Log("LET THE GAMES BEGIN!!");

        if (GameMode == "PracticeRange")
        {    
            StartCoroutine(PracticeRangeIntro());
        }
    }

    IEnumerator PracticeRangeIntro()
    {
        beginGameText.text = "Welcome to the Practice Range!";
        yield return new WaitForSeconds(3);
        beginGameText.text = "Select your weapon to begin";
        weaponsManager.ShowWeaponsWall();
        Debug.Log("running practicerangeintro coroutine");
    }

    void HideBeginUI()
    {
        beginGameUI.SetActive(false);
    }

    void StartRound()
    {
        // display wallet canvas
        walletCanvas.gameObject.SetActive(true);

        // get those carnival ducks spinnin!
        carniDucks.SetActive(true);

        //*****figure out how to start rounds once carnival ducks eliminated
        //roundWaves.enabled = true;
    }
}
