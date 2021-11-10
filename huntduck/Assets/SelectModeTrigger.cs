using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectModeTrigger : MonoBehaviour
{
    // TODO: create static event so other scripts can subscribe
    public delegate void SelectModeTriggered();
    public static SelectModeTriggered onSelectModeTriggered;

    public GameObject selectManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // start the round
            onSelectModeTriggered?.Invoke();
        }
    }
}
