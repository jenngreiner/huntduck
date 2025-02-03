using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HD_PlayerManager : MonoBehaviour
{
    //[Tooltip("The player object that is part of the scene (not persistent).")]
    //public GameObject scenePlayer;

    //void Awake()
    //{
    //    // Check if a persistent player already exists via OnlyOneMe.
    //    if (OnlyOneMe.instance != null)
    //    {
    //        Debug.Log("Persistent player exists. Destroying scene player: " + scenePlayer.GetInstanceID());
    //        Destroy(scenePlayer);
    //    }
    //}

    // Optional: specify a tag that all player objects share.

    void Start()
    {
        // Find all players in the scene.
        GameObject[] players = GameObject.FindGameObjectsWithTag(TagManager.PLAYER_TAG);

        // Assume your persistent player uses the OnlyOneMe singleton.
        GameObject persistentPlayer = (OnlyOneMe.instance != null) ? OnlyOneMe.instance.gameObject : null;

        foreach (GameObject player in players)
        {
            // If a persistent player exists and this is not it...
            if (persistentPlayer != null && player != persistentPlayer)
            {
                Debug.Log("Destroying duplicate scene player: " + player.GetInstanceID());
                Destroy(player);
            }
        }
    }
}
