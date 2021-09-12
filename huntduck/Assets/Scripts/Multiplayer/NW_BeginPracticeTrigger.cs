using UnityEngine;
using Photon.Pun;
using System;

public class NW_BeginPracticeTrigger : MonoBehaviourPun
{
    public static bool isGameStarted = false;

    public NW_PracticeRangeManager nw_practiceRangeManager;

    public void Awake()
    {
        RPC_setGameNotStarted();
    }

    void OnTriggerEnter(Collider other)
    {
        this.photonView.RPC("RPC_BeginPractice", RpcTarget.All, "other");
        Debug.Log(string.Format("RPC_BeginPractice {0},", other));
    }

    [PunRPC]
    void RPC_setGameNotStarted()
    {
        isGameStarted = false;
        Debug.Log(string.Format("RPC_BeginPractice"));
    }

    [PunRPC]
    void RPC_BeginPractice(Collider other)
    {
        // begin practice when player touches trigger, if practice hasn't already started
        if (other.tag == "Player" && !isGameStarted)
        {
            // Begin the game
            nw_practiceRangeManager.RPC_BeginGame();
            isGameStarted = true;
        }
    }
}