using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// SINGLESCENE CLASS: when you shoot the button, go to the corresponding game mode
public class ChooseGameMode : MonoBehaviour
{
    public GameObject gameMode;
    public GameObject selectMode;

    public delegate void SwitchModes();
    public static event SwitchModes onSwitchMode;

    public delegate void StartHuntMode();
    public static event StartHuntMode onStartHuntMode;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("KeyDown.H: Go to Hunt Mode");
            if(gameMode.name == "HuntMode")
            {
                changeGameMode();
                onStartHuntMode?.Invoke();
            }
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            SceneManager.LoadScene("GroupHunt");
        }
    }

    public void changeGameMode()
    {
        gameMode.SetActive(true);
        selectMode.SetActive(false); // deactivate after delay (coroutine) if you want particles effect
        onSwitchMode?.Invoke();
    }
}
