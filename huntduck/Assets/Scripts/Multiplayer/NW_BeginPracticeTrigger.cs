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
        ////Debug.Log("NW_BeginPracticeTrigger trigger was entered");
        ////if (PhotonNetwork.InRoom)
        ////{
        //    Debug.Log("NW_BeginPracticeTrigger we are within PN room");
        //    this.photonView.RPC("RPC_BeginPractice", RpcTarget.All, "other");
        //    Debug.Log(string.Format("RPC_BeginPractice {0},", other));
        ////}
        ////
        Debug.Log("trigger was entered");
        if (other.tag == "Player" && !isGameStarted)
        {
            Debug.Log("We are player, can we start the game?");

            if (PhotonNetwork.InRoom)
            {
                Debug.Log("You in the room dog");
                // Begin the game
                nw_practiceRangeManager.BeginGame();
                Debug.Log("Practice round just began yall");
            }
            else
            {
                Debug.Log("You ain't in the room dog");
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
        Debug.Log("RPC_BeginPractice was called");
        // begin practice when player touches trigger, if practice hasn't already started
        if (other.tag == "Player" && !isGameStarted)
        {
            // Begin the game
            nw_practiceRangeManager.photonView.RPC("RPC_BeginGame", RpcTarget.All);
            Debug.Log("Practice round just began yall");
            isGameStarted = true;
        }
    }
}