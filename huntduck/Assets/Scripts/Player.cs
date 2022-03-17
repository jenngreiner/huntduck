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
    }
}
