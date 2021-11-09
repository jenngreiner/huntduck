using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectManager : MonoBehaviour
{
    public GameObject helperUI;
    public Text helperText;

    public GameObject buttons;

    private void Start()
    {
        // start script here
        StartCoroutine(startSelectMode());
    }

    private void OnEnable()
    {
        WeaponsManager.onWeaponSelected += showButtons;
    }

    private void OnDisable()
    {
        WeaponsManager.onWeaponSelected -= showButtons;
    }

    void showButtons()
    {
        helperText.text = "Shoot Practice or Hunt to begin";
        // turn on button gameobject
        buttons.SetActive(true);
    }

    IEnumerator startSelectMode()
    {
        helperUI.SetActive(true);
        helperText.text = "Welcome to\nHunt Duck";
        yield return new WaitForSeconds(3f);
        helperText.text = "Select your weapon behind you to begin";
    }
}
