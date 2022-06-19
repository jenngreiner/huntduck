using UnityEngine;
using huntduck;

[RequireComponent(typeof(PlayerData))]
public class PlayerHealth : MonoBehaviour
{
    public AudioClip playerDiesSound;
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
            BNG.VRUtils.Instance.PlaySpatialClipAt(playerDiesSound, transform.position, 1f, 1f);
            onPlayerDied?.Invoke(); // updates PlayerHealthUI.cs
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