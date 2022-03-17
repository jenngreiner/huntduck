using UnityEngine;
using huntduck;

public class PlayerHealth : MonoBehaviour
{
    //private float playerHealth;
    //private bool isPlayerDead;
    private Player player;
    private float startingHealth;

    void Start()
    {
        player = GetComponent<Player>();
        startingHealth = player.health;
    }

    void Update()
    {
        if (player.health <= 0f && !player.isDead)
        {
            player.isDead = true;
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
        player.health = startingHealth;
        player.isDead = false;
    }
}