using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectManager : MonoBehaviour
{
    public GameObject helperUI;
    public Text helperText;

    public GameObject buttons;
    public GameObject weaponsWall;

    private void OnEnable()
    {
        SelectModeTrigger.onSelectModeTriggered += StartSelectMode;
        WeaponsManager.onWeaponSelected += ShowButtons;
    }

    private void OnDisable()
    {
        WeaponsManager.onWeaponSelected -= ShowButtons;
        SelectModeTrigger.onSelectModeTriggered -= StartSelectMode;
    }

    void ShowButtons()
    {
        helperText.text = "Shoot Practice or Hunt to begin";
        // turn on button gameobject
        buttons.SetActive(true);
    }

    void StartSelectMode()
    {
        StartCoroutine(SelectModeIntro());
    }

    IEnumerator SelectModeIntro()
    {
        helperUI.SetActive(true);
        helperText.text = "Welcome to\nHunt Duck";
        yield return new WaitForSeconds(3f);
        weaponsWall.SetActive(true);
        helperText.text = "Select your weapon behind you to begin";
    }
}
