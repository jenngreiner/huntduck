using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 100;

    private int currentHealth;

    void Awake()
    {
        SetDefaults();
    }

    public void TakeDamage(int _amount)
    {
        currentHealth -= _amount;

        Debug.Log(transform.name + " now has " + currentHealth + " health.");
    }

    public void SetDefaults()
    {
        currentHealth = maxHealth;
    }
}
