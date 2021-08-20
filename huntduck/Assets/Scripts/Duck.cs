using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BNG.Damageable))]
public class Duck : MonoBehaviour
{

    private bool _isDead = false;
    public bool isDead { get; protected set; }

    // override in inspector
    [SerializeField]
    private int duckPoints = 1;

    private PlayerScore playerScoreScript;
    private const string PLAYER_TAG = "Player";

    void Start()
    {
        playerScoreScript = GameObject.FindGameObjectWithTag(PLAYER_TAG).GetComponent<PlayerScore>();
    }


    public void Die()
    {
        isDead = true;
        Debug.Log(transform.name + " is DEAD!");

        // since this duck was killed, add points to the score
        Debug.Log("Duck just died, sending player " + duckPoints + " duckpoints");
        playerScoreScript.SendMessage("PlayerScores", duckPoints);

        // for multiplayer we will need to refactor to know who killed the duck
    }

    // OLD SCRIPT BELOW

    //[SerializeField]
    //private int maxHealth = 100;
    //private int currentHealth;



    //void Awake()
    //{
    //    ////SetDefaults();
    //}

    //public void TakeDamage(int _amount)
    //{
    //    if (isDead)
    //    {
    //        return;
    //    }

    //    currentHealth -= _amount;

    //    Debug.Log(transform.name + " now has " + currentHealth + " health.");

    //    if (currentHealth <= 0)
    //    {
    //        Die();
    //    }
    //}

    //public void Die()
    //{
    //    isDead = true;
    //    Debug.Log(transform.name + " is DEAD!");

    //}

    //public void SetDefaults()
    //{
    //    currentHealth = maxHealth;
    //}
}
