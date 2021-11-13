using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartGameMode : MonoBehaviour
{
    public delegate void RestartMode();
    public static event RestartMode onRestartMode;

    public GameObject hideThisUI;

    public void restartModeEvent()
    {
        onRestartMode?.Invoke();
        hideThisUI.SetActive(false);
    }
}
