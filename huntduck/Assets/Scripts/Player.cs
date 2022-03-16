using UnityEngine;

namespace huntduck
{
    // store player properties to make accessible to other scripts
    public class Player : MonoBehaviour
    {
        // TODO: replicate this readonly pattern for variables above OR replace values with scriptable objects throughout game
        public PlayerHealth PlayerHealth { get { return playerHealth; } private set { playerHealth = value; } }
        [SerializeField]
        private PlayerHealth playerHealth;

        public PlayerScore playerScore;
        public Transform playerController;
    }
}
