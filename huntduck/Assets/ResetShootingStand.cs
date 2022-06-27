using UnityEngine;

public class ResetShootingStand : MonoBehaviour
{
    void OnEnable()
    {
        RestartGameMode.onRestartMode += ResetStand;
    }

    void OnDisable()
    {
        RestartGameMode.onRestartMode -= ResetStand;
    }

    void ResetStand()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
