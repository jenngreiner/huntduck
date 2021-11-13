using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGameMode : MonoBehaviour
{
    public delegate void RestartMode();
    public static event RestartMode onExitMode;

    public BNG.Damageable damageableScript;
    public GameObject returnMode;

    public GameObject hideThisUI;
    public GameObject hideThisMode;

    public void exitMode()
    {
        // add something to hide the UI

        damageableScript.InstantRespawn();
        hideThisUI.SetActive(false);
        onExitMode?.Invoke();
        returnMode.SetActive(true);
        hideThisMode.SetActive(false);
    }
}
