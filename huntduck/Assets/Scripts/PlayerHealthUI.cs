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

    private PlayerData playerData;

    void Start()
    {
        playerData = ObjectManager.instance.player.GetComponent<PlayerData>();
    }

    void OnEnable()
    {
        PlayerHealth.onPlayerTookDamage += UpdateHealthUI;
        PlayerHealth.onPlayerDied += ShowPlayerDeadImage;

        // SINGLESCENE: reset health UI on restart or quit
        RestartGameMode.onRestartMode += ShowHealthBar;
        ExitGameMode.onExitMode += ShowHealthBar;
    }

    void OnDisable()
    {
        PlayerHealth.onPlayerTookDamage -= UpdateHealthUI;
        PlayerHealth.onPlayerDied -= ShowPlayerDeadImage;

        // SINGLESCENE: reset health UI on restart or quit
        RestartGameMode.onRestartMode -= ShowHealthBar;
        ExitGameMode.onExitMode -= ShowHealthBar;
    }

    void UpdateHealthUI()
    {
        healthCountText.text = playerData.currenthealth.ToString();
    }

    void ShowPlayerDeadImage()
    {
        healthLabelObj.SetActive(false);
        healthBarObj.SetActive(false);
        playerDeadImage.SetActive(true);
    }

    void ShowHealthBar()
    {
        healthLabelObj.SetActive(true);
        healthBarObj.SetActive(true);
        playerDeadImage.SetActive(false);
    }
}