using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject welcomeUI;
    public GameObject selectGunUI;
    public Text selectGunText;
    public GameObject helperUI;
    public Text helperText;
    public GameObject buttons;
    public GameObject downArrow;

    private bool gunSelected = false;

    //[Header("Weapons")]
    //public GameObject weaponsWall;

    private void OnEnable()
    {
        StartSelectMode();
        WeaponsManager.onWeaponSelected += ShowGameModeButtons;
        //WallSlider.onPosition1Reached += ShowWeaponsWall;
    }

    private void OnDisable()
    {
        WeaponsManager.onWeaponSelected -= ShowGameModeButtons;
        downArrow.SetActive(false); // hide this arrow on sign, activate by script when needed

        //WallSlider.onPosition1Reached -= ShowWeaponsWall;
    }

    void StartSelectMode()
    {
        StartCoroutine(SelectModeIntro());
    }

    IEnumerator SelectModeIntro()
    {
        welcomeUI.SetActive(true);
        yield return new WaitForSeconds(3f);

        if (!gunSelected) // only show selectGunUI if we haven't grabbed gun yet
        {
            selectGunUI.SetActive(true);
            selectGunText.text = "SELECT YOUR WEAPON \n "; // give space for down arrow
            downArrow.SetActive(true);
        }
    }

    void ShowGameModeButtons()
    {
        welcomeUI.SetActive(false);
        selectGunUI.SetActive(false);
        gunSelected = true;

        helperUI.SetActive(true);
        helperText.text = "SHOOT PRACTICE OR HUNT TO BEGIN";
        buttons.SetActive(true);
    }

    //void ShowWeaponsWall()
    //{
    //    weaponsWall.SetActive(true);
    //}
}
