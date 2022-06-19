using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject welcomeUI;
    public GameObject helperUI;
    public Text helperText;
    public GameObject buttons;

    [Header("Weapons")]
    public GameObject weaponsWall;

    [Header("Sounds")]
    public AudioClip backgroundSound;

    private void OnEnable()
    {
        SelectModeTrigger.onSelectModeTriggered += StartSelectMode;
        WallSlider.onPosition1Reached += ShowWeaponsWall;
        WeaponsManager.onWeaponSelected += ShowButtons;

        BNG.VRUtils.Instance.PlaySpatialClipAt(backgroundSound, transform.position, 1f, 1f);
    }

    private void OnDisable()
    {
        WeaponsManager.onWeaponSelected -= ShowButtons;
        WallSlider.onPosition1Reached -= ShowWeaponsWall;
        SelectModeTrigger.onSelectModeTriggered -= StartSelectMode;
    }

    void ShowButtons()
    {
        helperText.text = "SHOOT PRACTICE OR HUNT TO BEGIN";
        // turn on button gameobject
        buttons.SetActive(true);
    }

    void StartSelectMode()
    {
        StartCoroutine(SelectModeIntro());
    }

    void ShowWeaponsWall()
    {
        weaponsWall.SetActive(true);
    }

    IEnumerator SelectModeIntro()
    {
        welcomeUI.SetActive(true);
        yield return new WaitForSeconds(3f);
        welcomeUI.SetActive(false);

        helperUI.SetActive(true);
        helperText.text = "SELECT YOUR WEAPON TO BEGIN";
    }
}
