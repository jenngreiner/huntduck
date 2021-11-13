using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// when you shoot the button, go to the corresponding game mode
public class ChooseGameMode : MonoBehaviour
{
    public GameObject gameMode;
    public GameObject selectMode;
    public BNG.Damageable damageableScript;

    public void changeGameMode()
    {
        damageableScript.InstantRespawn();
        gameMode.SetActive(true);
        // remove this part and deactivate the SelectWorld buttons section on death if you want more realism
        selectMode.SetActive(false);
    }
}
