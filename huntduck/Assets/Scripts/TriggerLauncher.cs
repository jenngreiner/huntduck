using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLauncher : MonoBehaviour
{
    public WaveSpawner _waveSpawner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //start game (WaveSpawner)
            _waveSpawner.enabled = true;
            Debug.Log("OH, IT'S ON BOIS & GURLS");
        }
    }
}
