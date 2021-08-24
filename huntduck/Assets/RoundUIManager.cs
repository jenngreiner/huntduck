using UnityEngine;
using UnityEngine.UI;

public class RoundUIManager : MonoBehaviour
{
    public Text roundNumber;

    //public AudioSource roundStartSound;

    void Start()
    {
        this.gameObject.SetActive(false);
    }
}
