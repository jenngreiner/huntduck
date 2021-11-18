using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectModeTrigger : MonoBehaviour
{
    // TODO: create static event so other scripts can subscribe
    public delegate void SelectModeTriggered();
    public static SelectModeTriggered onSelectModeTriggered;

    private bool isSelectModeTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !isSelectModeTriggered)
        {
            // start the round
            isSelectModeTriggered = true;
            onSelectModeTriggered?.Invoke();
            Debug.Log("Select mode triggered");
        }
    }
}
