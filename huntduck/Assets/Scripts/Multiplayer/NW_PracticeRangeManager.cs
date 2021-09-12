using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NW_PracticeRangeManager : MonoBehaviourPun
{
    public GameObject beginGameUI;
    public Text beginGameText;
    public NW_WaveSpawner nw_waveSpawner;
    public WeaponsManager weaponsManager;
    public Canvas walletCanvas;
    public GameObject carniDucks;

    void Start()
    {
        if (PhotonNetwork.InRoom)
        {
            this.photonView.RPC("RPC_SetupRound", RpcTarget.All);
        }
    }

    void OnEnable()
    {
        if (PhotonNetwork.InRoom)
        {
            NW_WeaponsManager.onWeaponSelected += RPC_StartRound;
        }
    }

    void OnDisable()
    {
        if (PhotonNetwork.InRoom)
        {
            NW_WeaponsManager.onWeaponSelected -= RPC_StartRound;
        }
    }

    [PunRPC]
    void RPC_SetupRound()
    {
        walletCanvas.gameObject.SetActive(false);
        carniDucks.gameObject.SetActive(false);
    }

    [PunRPC]
    void RPC_StartRound()
    {
        beginGameUI.SetActive(false);
        walletCanvas.enabled = true;
        //carniDucks.SetActive(true);
        nw_waveSpawner.enabled = true;
        Debug.Log(string.Format("RPC_StartRound"));
    }


    // this is called in BeginPracticeTrigger.cs on StartingBlock GameObject
    [PunRPC]
    public void RPC_BeginGame()
    {
        Debug.Log("LET THE GAMES BEGIN!!");
        StartCoroutine(PracticeRangeIntro());
    }

    IEnumerator PracticeRangeIntro()
    {
        beginGameText.text = "Welcome to the Practice Range!";
        yield return new WaitForSeconds(3);
        beginGameText.text = "Select your weapon behind you";
        //weaponsManager.ShowWeaponsWall();
        Debug.Log("running practicerangeintro coroutine");
    }
}
