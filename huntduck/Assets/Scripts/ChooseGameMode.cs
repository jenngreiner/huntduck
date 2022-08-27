using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SINGLESCENE CLASS: when you shoot the button, go to the corresponding game mode
public class ChooseGameMode : MonoBehaviour
{
    public GameObject gameMode;
    public GameObject selectMode;

    public delegate void SwitchModes();
    public static event SwitchModes onSwitchMode;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("KeyDown.H: Go to Hunt Mode");
            if(gameMode.name == "HuntMode")
            {
                changeGameMode();
            }
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("KeyDown.M: Go to Multiplayer Mode");
            if(gameMode.name == "SquadUp2")
            {
                changeGameMode();
            }
        }
    }

    public void changeGameMode()
    {
        gameMode.SetActive(true);
        selectMode.SetActive(false); // deactivate after delay (coroutine) if you want particles effect
        onSwitchMode?.Invoke();
    }
}
