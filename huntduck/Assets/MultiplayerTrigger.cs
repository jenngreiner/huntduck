using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerTrigger : MonoBehaviour
{
    //public JoinMultiplayerNetwork jmpscript;

    public delegate void OnMultiplayerTrigger();
    public static event OnMultiplayerTrigger onMultiplayerTrigger;


    private void OnTriggerEnter()
    {
        onMultiplayerTrigger();
    }
}
