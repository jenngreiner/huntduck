using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int playerScore = 0;

    void Awake()
    {

        Debug.Log(transform.name + " now has a starting score of " + playerScore);
    }
}
