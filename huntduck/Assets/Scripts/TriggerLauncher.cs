using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class starts Rounds
public class TriggerLauncher : MonoBehaviour
{
    public delegate void BegintheGame();
    public static event BegintheGame onGameBegin;

    public WaveSpawner _waveSpawner;


    // we begin the game when the collider is entered by the player
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // Start Round 1
            _waveSpawner.enabled = true;
            
        }
    }


}
