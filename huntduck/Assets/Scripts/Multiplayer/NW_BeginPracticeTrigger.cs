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
        ////Debug.Log("NW_BeginPracticeTrigger.cs: trigger was entered");
        ////if (PhotonNetwork.InRoom)
        ////{
        //    Debug.Log("NW_BeginPracticeTrigger.cs: we are within PN room");
        //    this.photonView.RPC("RPC_BeginPractice", RpcTarget.All, "other");
        //    Debug.Log(string.Format("RPC_BeginPractice {0},", other));
        ////}
        ////
        Debug.Log("NW_BeginPracticeTrigger.cs: trigger was entered");
        if (other.tag == "Player" && !isGameStarted)
        {
            Debug.Log("NW_BeginPracticeTrigger.cs: We are player, can we start the game?");

            if (PhotonNetwork.InRoom)
            {
                Debug.Log("NW_BeginPracticeTrigger.cs: You in the room dog");
                // Begin the game
                nw_practiceRangeManager.BeginGame();
                Debug.Log("NW_BeginPracticeTrigger.cs: Practice round just began yall");
            }
            else
            {
                Debug.Log("NW_BeginPracticeTrigger.cs: You ain't in the room dog");
            }

        }
    }

    [PunRPC]
    void RPC_setGameNotStarted()
    {
        isGameStarted = false;
        Debug.Log(string.Format("RPC_setGameNotStarted"));
    }

    [PunRPC]
    void RPC_BeginPractice(Collider other)
    {
        Debug.Log("NW_BeginPracticeTrigger.cs: RPC_BeginPractice was called");
        // begin practice when player touches trigger, if practice hasn't already started
        if (other.tag == "Player" && !isGameStarted)
        {
            // Begin the game
            nw_practiceRangeManager.photonView.RPC("RPC_BeginGame", RpcTarget.All);
            Debug.Log("NW_BeginPracticeTrigger.cs: Practice round just began yall");
            isGameStarted = true;
        }
    }
}