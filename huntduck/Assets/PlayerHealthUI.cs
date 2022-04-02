using UnityEngine;
using UnityEngine.UI;
using huntduck;

public class PlayerHealthUI : MonoBehaviour
{
    private PlayerData playerData;

    public GameObject healthLabelObj;
    public GameObject healthCountObj;
    public Text healthCountText;

    public GameObject tvScreen;
    public GameObject healthZeroImage;

    void Start()
    {
        playerData = ObjectManager.instance.player;

        TurnTVOff();
    }

    void OnEnable()
    {
        SurvivalWaveSpawner.onFirstWaveStart += TurnTVOn;
        Egg.onEggHitPlayer += UpdateHealthUI;
        PlayerHealth.onPlayerDied += ShowHealthZeroImage;

        // SINGLESCENE: reset health UI on restart or quit
        RestartGameMode.onRestartMode += TurnTVOff;
        ExitGameMode.onExitMode += TurnTVOff;
    }

    void OnDisable()
    {
        SurvivalWaveSpawner.onFirstWaveStart -= TurnTVOn;
        Egg.onEggHitPlayer -= UpdateHealthUI;
        PlayerHealth.onPlayerDied -= ShowHealthZeroImage;

        // SINGLESCENE: reset health UI on restart or quit
        RestartGameMode.onRestartMode += TurnTVOff;
        ExitGameMode.onExitMode -= TurnTVOff;
    }

    void HideHealthUI()
    {
        // hide label and health UI (by shrinking), while keeping active
        healthLabelObj.transform.localScale = new Vector3(0, 0, 0);
        healthCountObj.transform.localScale = new Vector3(0, 0, 0);
    }

    void ShowHealthUI()
    {
        healthLabelObj.transform.localScale = new Vector3(1, 1, 1);
        healthCountObj.transform.localScale = new Vector3(1, 1, 1);
    }

    void TurnTVOff()
    {
        HideHealthUI();
        tvScreen.SetActive(false);
        healthZeroImage.SetActive(false);
    }

    void TurnTVOn()
    {
        tvScreen.SetActive(true);
        ShowHealthUI();
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        healthCountText.text = playerData.health.ToString();
    }

    void ShowHealthZeroImage()
    {
        healthZeroImage.SetActive(true);
        HideHealthUI();
    }
}