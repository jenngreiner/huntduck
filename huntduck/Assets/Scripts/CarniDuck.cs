using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BNG.Damageable))]
public class CarniDuck : MonoBehaviour
{
    public int duckPoints = 500; // override in inspector, otherwise set to 500
    public Text duckPointsText;

    public delegate void DuckDied(int points);
    public static event DuckDied onDuckDied;

    void Start()
    {
        duckPointsText.text = "$" + duckPoints.ToString();
    }

    void OnEnable()
    {
        BNG.Damageable.onDuckDie += Die;
    }

    void OnDisable()
    {
        BNG.Damageable.onDuckDie -= Die;
    }

    public void Die(GameObject deadDuck)
    {
        if(deadDuck == gameObject)
        {
            onDuckDied?.Invoke(duckPoints); // subscribe in PlayerScore.cs
        }
    }
}
