using UnityEngine;
using UnityEngine.UI;

public class RoundUIManager : MonoBehaviour
{
    public Text roundNumber;

    void Start()
    {
        this.gameObject.SetActive(false);
    }
}
