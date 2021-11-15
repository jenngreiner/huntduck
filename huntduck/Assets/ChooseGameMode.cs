using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// when you shoot the button, go to the corresponding game mode
public class ChooseGameMode : MonoBehaviour
{
    public GameObject gameMode;
    public GameObject selectMode;

    public delegate void SwitchModes();
    public static event SwitchModes onSwitchMode;

    public void changeGameMode()
    {
        gameMode.SetActive(true);
        selectMode.SetActive(false); // deactivate after delay (coroutine) if you want particles effect
        onSwitchMode?.Invoke();
    }
}
