using UnityEngine;

public class BeginGameTrigger : MonoBehaviour
{
    public WaveSpawner waveSpawner;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // start the round
            waveSpawner.enabled = true;
        }
    }
}
