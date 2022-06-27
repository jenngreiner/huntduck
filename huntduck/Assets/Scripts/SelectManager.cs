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

    //[Header("Weapons")]
    //public GameObject weaponsWall;

    private void OnEnable()
    {
        StartSelectMode();
        WeaponsManager.onWeaponSelected += HideSelectUI;
        WeaponsManager.onWeaponSelected += ShowButtons;
        //WallSlider.onPosition1Reached += ShowWeaponsWall;
    }

    private void OnDisable()
    {
        WeaponsManager.onWeaponSelected -= HideSelectUI;
        WeaponsManager.onWeaponSelected -= ShowButtons;
        //WallSlider.onPosition1Reached -= ShowWeaponsWall;
        downArrow.SetActive(false); // hide this arrow on sign, activate by script when needed
    }

    void StartSelectMode()
    {
        StartCoroutine(SelectModeIntro());
    }

    IEnumerator SelectModeIntro()
    {
        welcomeUI.SetActive(true);
        yield return new WaitForSeconds(3f);
        welcomeUI.SetActive(false);

        selectGunUI.SetActive(true);
        selectGunText.text = "SELECT YOUR WEAPON \n "; // give space for down arrow
        downArrow.SetActive(true);
    }

    void HideSelectUI()
    {
        selectGunUI.SetActive(false);
    }

    void ShowButtons()
    {
        helperUI.SetActive(true);
        helperText.text = "SHOOT PRACTICE OR HUNT TO BEGIN";
        buttons.SetActive(true);
    }

    //void ShowWeaponsWall()
    //{
    //    weaponsWall.SetActive(true);
    //}
}
