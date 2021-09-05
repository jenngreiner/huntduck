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
    public GameObject clayLauncher;

    void Start()
    {
        SetupRound();
    }

    void OnEnable()
    {
        WeaponsManager.onWeaponSelected += StartRound;
    }

    void OnDisable()
    {
        WeaponsManager.onWeaponSelected -= StartRound;
    }

    void SetupRound()
    {
        walletCanvas.gameObject.SetActive(false);
        carniDucks.gameObject.SetActive(false);
    }

    void StartRound()
    {
        beginGameUI.SetActive(false);
        walletCanvas.enabled = true;
        //carniDucks.SetActive(true);
        roundWaves.enabled = true;
    }


    // this is called in BeginGameTrigger.cs
    public void BeginGame()
    {
        Debug.Log("LET THE GAMES BEGIN!!");
        StartCoroutine(PracticeRangeIntro());
    }

    IEnumerator PracticeRangeIntro()
    {
        beginGameText.text = "Welcome to the Practice Range!";
        yield return new WaitForSeconds(3);
        beginGameText.text = "Select your weapon to begin";
        weaponsManager.ShowWeaponsWall();
        Debug.Log("running practicerangeintro coroutine");
    }
}
