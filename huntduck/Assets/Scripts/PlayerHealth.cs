using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float playerHealth = 100f;
    public bool isPlayerDead = false;
    private float startingHealth;

    void Start()
    {
        startingHealth = playerHealth;
    }

    void Update()
    {
        if (playerHealth <= 0f && !isPlayerDead)
        {
            isPlayerDead = true;
            Debug.Log("GAMEOVER: Player is dead!");
        }
    }

    void OnEnable()
    {
        RestartGameMode.onRestartMode += ResetPlayerHealth;
    }

    void OnDisable()
    {
        RestartGameMode.onRestartMode -= ResetPlayerHealth;
    }

    public void ResetPlayerHealth()
    {
        playerHealth = startingHealth;
        isPlayerDead = false;
    }
}