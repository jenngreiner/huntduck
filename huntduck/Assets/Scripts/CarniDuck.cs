using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BNG.Damageable))]
public class CarniDuck : MonoBehaviour
{
    public int duckPoints = 500; // override in inspector, otherwise set to 500
    public Text duckPointsText;

    private const string PLAYER_TAG = "Player";
    private PlayerScore playerScoreScript;

    public delegate void DuckDied(Transform thisTransform);
    public static event DuckDied onDuckDied;

    void Start()
    {
        duckPointsText.text = "$" + duckPoints.ToString();
        playerScoreScript = GameObject.FindGameObjectWithTag(PLAYER_TAG).GetComponent<PlayerScore>();
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
            playerScoreScript.SendMessage("UpdatePlayerScore", duckPoints);
            Debug.Log("Duck died, player receives " + duckPoints + " duckpoints");
        }
    }
}
