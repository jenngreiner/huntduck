using UnityEngine;
using huntduck;

[RequireComponent(typeof(PlayerData))]
public class PlayerHealth : MonoBehaviour
{
    public AudioClip playerDiesSound;
    public HealthBar healthBar;

    private PlayerData player;

    public delegate void PlayerTookDamage();
    public static event PlayerTookDamage onPlayerTookDamage;

    public delegate void PlayerDied();
    public static event PlayerDied onPlayerDied;

    void Start()
    {
        player = GetComponent<PlayerData>();
        player.currenthealth = player.maxHealth;
        healthBar.SetMaxHealth(player.maxHealth);
    }

    void Update()
    {
        if (player.currenthealth <= 0f && !player.isDead)
        {
            player.isDead = true;
            BNG.VRUtils.Instance.PlaySpatialClipAt(playerDiesSound, transform.position, 1f, 1f);
            onPlayerDied?.Invoke(); // updates PlayerHealthUI.cs
            InfiniteLevelManager.instance.survivalWaveSpawner.WaveCompleted();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(1);
        }
    }

    void OnEnable()
    {
        Egg.onEggHitPlayer += TakeDamage;
        RestartGameMode.onRestartMode += ResetPlayerHealth;
    }

    void OnDisable()
    {
        Egg.onEggHitPlayer -= TakeDamage;
        RestartGameMode.onRestartMode -= ResetPlayerHealth;
    }

    void TakeDamage(int damage)
    {
        player.currenthealth -= damage;
        healthBar.SetHealth(player.currenthealth);
        onPlayerTookDamage?.Invoke();
    }

    void ResetPlayerHealth()
    {
        player.currenthealth = player.maxHealth;
        player.isDead = false;
    }
}