using UnityEngine;
using huntduck;

[RequireComponent(typeof(PlayerData))]
public class PlayerHealth : MonoBehaviour
{
    private PlayerData playerData;
    private float startingHealth;

    public delegate void PlayerDied();
    public static event PlayerDied onPlayerDied;

    void Start()
    {
        playerData = GetComponent<PlayerData>();
        startingHealth = playerData.health;
    }

    void Update()
    {
        if (playerData.health <= 0f && !playerData.isDead)
        {
            playerData.isDead = true;
            onPlayerDied?.Invoke();
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
        playerData.health = startingHealth;
        playerData.isDead = false;
    }
}