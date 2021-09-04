using UnityEngine;

public class HideWeapons : MonoBehaviour
{
    public static bool isWeaponSelected = false;

    void Update()
    {
        if (isWeaponSelected)
        {
            this.gameObject.SetActive(false);
        }
    }

    // static method for static boolean, can be called from anywhere
    // we will call from SnapZone.cs to trigger when weapon is picked up
    public static void hideWeaponWall()
    {
        isWeaponSelected = true;
    }
}
