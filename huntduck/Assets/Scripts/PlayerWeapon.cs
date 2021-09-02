// using UnityEngine.Networking
// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

// Unity will know how to save & load this class so we can update the range and damage in the inspector
[System.Serializable]
public class PlayerWeapon
{
    public string name = "Gun";

    public int damage = 10;
    public float range = 100f;
}
