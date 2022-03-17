using UnityEngine;

namespace huntduck
{
    // store specialized player scripts to make their properties accessible to other scripts
    // since these values are simply script references, store as read-only (don't want other scripts changing)
    public class Player : MonoBehaviour
    {
        public int score = 0;
        public int duckKills = 0;
        public float health = 100f;
        public bool isDead = false;
        public Transform controller;

        //public PlayerHealth playerHealth;


        //// since the scripts are reference
        //public PlayerHealth PlayerHealthScript { get { return playerHealth; } private set { playerHealth = value; } }

        //public PlayerScore PlayerScoreScript { get { return playerScore; } private set { playerScore = value; } }

        //public Transform PlayerControllerScript {  get { return playerController; } private set { playerController = value; } }

        //[SerializeField]
        //private PlayerHealth playerHealth;
        //[SerializeField]
        //private PlayerScore playerScore;
        //[SerializeField]
        //private Transform playerController;
    }
}
