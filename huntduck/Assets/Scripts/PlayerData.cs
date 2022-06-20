using UnityEngine;

namespace huntduck
{
    // store specialized player scripts to make their properties accessible to other scripts
    // TODO: since these values are simply script references, store as read-only (don't want other scripts changing)
    public class PlayerData : MonoBehaviour
    {
        public int score = 0;
        public int duckKills = 0;
        public int maxHealth = 3;
        public int currenthealth; 
        public bool isDead = false;
        public Transform controller;
    }
}