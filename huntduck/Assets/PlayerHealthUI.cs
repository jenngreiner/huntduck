//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using huntduck;
//using UnityEngine.UI;

//public class PlayerHealthUI : MonoBehaviour
//{
//    private PlayerData player;

//    public Text dollarsText;
//    public static string walletScore;

//    void Start()
//    {
//        player = ObjectManager.instance.player;
//        // reset score when game starts
//        UpdateScoreUI();
//    }

//    void OnEnable()
//    {
//        // update score ui when the player's score changes
//        PlayerScore.onScoreUpdate += UpdateScoreUI;
//        SurvivalWaveSpawner.onDuckHit += UpdateScoreUI;
//    }

//    void OnDisable()
//    {
//        PlayerScore.onScoreUpdate -= UpdateScoreUI;
//        SurvivalWaveSpawner.onDuckHit -= UpdateScoreUI;
//    }
//}
