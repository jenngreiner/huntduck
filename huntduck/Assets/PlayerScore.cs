using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public int playerScore = 0;
    public int duckKills = 0;

    void Awake()
    {
        Debug.Log(transform.name + " started with a score of " + playerScore);
        Debug.Log(transform.name + " started with " + duckKills + " duck kills");
    }
}
