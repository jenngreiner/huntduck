using UnityEngine;
using UnityEngine.UI;
using huntduck;

public class PlayerHealthUI : MonoBehaviour
{
    public GameObject healthLabelObj;
    public GameObject healthCountObj;
    public GameObject healthBarObj;
    public Text healthCountText;
    public GameObject playerDeadImage;

    private PlayerData player;

    void Start()
    {
        player = ObjectManager.instance.player;
    }

    void OnEnable()
    {
        //SurvivalWaveSpawner.onFirstWaveStart += TurnTVOn;
        PlayerHealth.onPlayerTookDamage += UpdateHealthUI;
        PlayerHealth.onPlayerDied += ShowPlayerDeadImage;

        // SINGLESCENE: reset health UI on restart or quit
        RestartGameMode.onRestartMode += ShowHealthUI;
        ExitGameMode.onExitMode += ShowHealthUI;
    }

    void OnDisable()
    {
        //SurvivalWaveSpawner.onFirstWaveStart -= TurnTVOn;
        PlayerHealth.onPlayerTookDamage -= UpdateHealthUI;
        PlayerHealth.onPlayerDied -= ShowPlayerDeadImage;

        // SINGLESCENE: reset health UI on restart or quit
        RestartGameMode.onRestartMode += ShowHealthUI;
        ExitGameMode.onExitMode -= ShowHealthUI;
    }

    void UpdateHealthUI()
    {
        healthCountText.text = player.currenthealth.ToString();
    }

    void ShowPlayerDeadImage()
    {
        playerDeadImage.SetActive(true);
        HideHealthUI();
    }

    void HideHealthUI()
    {
        // hide label and bar by shrinking, while keeping active
        healthLabelObj.transform.localScale = new Vector3(0, 0, 0);
        healthBarObj.transform.localScale = new Vector3(0, 0, 0);
    }

    void ShowHealthUI()
    {
        // show label and bar by scaling up
        healthLabelObj.transform.localScale = new Vector3(1, 1, 1);
        healthBarObj.transform.localScale = new Vector3(1, 1, 1);
    }
}