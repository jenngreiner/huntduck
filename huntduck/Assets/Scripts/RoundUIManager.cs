using UnityEngine;
using UnityEngine.UI;

// the RoundUI is altered via waves in WaveSpawner.cs
public class RoundUIManager : MonoBehaviour
{
    public Text roundNumber;

    void Start()
    {
        this.gameObject.SetActive(false);
    }
}
