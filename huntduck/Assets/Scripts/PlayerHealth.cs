using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float playerHealth = 100f;
    public bool isPlayerDead = false;

    void Update()
    {
        if (playerHealth <= 0f && !isPlayerDead)
        {
            isPlayerDead = true;
            Debug.Log("GAMEOVER: Player is dead!");
        }
    }
}
