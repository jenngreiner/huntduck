using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartGameMode : MonoBehaviour
{
    public GameObject hideThisUI;

    public delegate void RestartMode();
    public static event RestartMode onRestartMode;

    public void restartModeEvent()
    {
        onRestartMode?.Invoke();
        hideThisUI.SetActive(false);
    }
}
