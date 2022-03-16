using UnityEngine;

namespace huntduck
{
    // store specialized player scripts to make their properties accessible to other scripts
    // since these values are simply script references, store as read-only (don't want other scripts changing)
    public class Player : MonoBehaviour
    {
        // since the scripts are reference
        public PlayerHealth PlayerHealth { get { return playerHealth; } private set { playerHealth = value; } }

        public PlayerScore PlayerScore { get { return playerScore; } private set { playerScore = value; } }

        public Transform PlayerController {  get { return playerController; } private set { playerController = value; } }

        [SerializeField]
        private PlayerHealth playerHealth;
        [SerializeField]
        private PlayerScore playerScore;
        [SerializeField]
        private Transform playerController;
    }
}
