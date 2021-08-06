using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : MonoBehaviour
{

    private bool _isDead = false;
    public bool isDead { get; protected set; }

    [SerializeField]
    private int maxHealth = 100;

    private int currentHealth;

    void Awake()
    {
        SetDefaults();
    }

    public void TakeDamage(int _amount)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= _amount;

        Debug.Log(transform.name + " now has " + currentHealth + " health.");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log(transform.name + " is DEAD!");
    }

    public void SetDefaults()
    {
        currentHealth = maxHealth;
    }
}
