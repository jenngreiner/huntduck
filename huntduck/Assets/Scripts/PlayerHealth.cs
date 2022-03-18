using UnityEngine;
using huntduck;

[RequireComponent(typeof(Player))]
public class PlayerHealth : MonoBehaviour
{
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
            // TODO: hook in GAMEOVER screen
            InfiniteLevelManager.instance.survivalWaveSpawner.WaveCompleted();
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