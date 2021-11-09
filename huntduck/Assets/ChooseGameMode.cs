using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// when you shoot the button, go to the corresponding game mode
public class ChooseGameMode : MonoBehaviour
{
    public GameObject gameMode;
    public GameObject selectMode;

    public void changeGameMode()
    {
        gameMode.SetActive(true);
        selectMode.SetActive(false);
    }
}
