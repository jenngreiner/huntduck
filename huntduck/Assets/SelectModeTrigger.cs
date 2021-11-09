using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectModeTrigger : MonoBehaviour
{
    public GameObject selectManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // start the round
            selectManager.SetActive(true);
        }
    }
}
