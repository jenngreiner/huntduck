using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// The BeginGameUI is set from WaveSpawner.cs
public class BeginGameManager : MonoBehaviour
{
    public GameObject beginGameUI;
    public Text beginGameText;
    public WaveSpawner practiceWaves;
    public WeaponsManager weaponsManager;

    public static bool isGameStarted = false;

    void OnEnable()
    {
        WeaponsManager.onWeaponSelected += HideBeginUI;
        // we could also start the round once the carnival ducks are gone, or keep this out of practice round all together
        WeaponsManager.onWeaponSelected += StartRound;
    }

    void OnDisable()
    {
        WeaponsManager.onWeaponSelected -= HideBeginUI;
        WeaponsManager.onWeaponSelected -= StartRound;
    }


    // this is called in BeginGameTrigger.cs
    public void BeginGame(string GameMode)
    {
        isGameStarted = true;
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
        practiceWaves.enabled = true;
    }
}
