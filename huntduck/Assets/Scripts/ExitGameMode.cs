using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGameMode : MonoBehaviour
{
    public GameObject returnMode;

    public GameObject hideThisUI;
    public GameObject hideThisMode;

    public delegate void ExitMode();
    public static event ExitMode onExitMode;

    public void exitMode()
    {
        // add something to hide the UI

        hideThisUI.SetActive(false);
        onExitMode?.Invoke();
        returnMode.SetActive(true);
        hideThisMode.SetActive(false);
    }
}
