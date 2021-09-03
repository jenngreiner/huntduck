using UnityEngine;

public class HideWeapons : MonoBehaviour
{
    private void Update()
    {
        // if weapon is picked up, hide the whole weapon wall
        OnWeaponPickedUp += hideWeaponWall();
    }

    private void hideWeaponWall()
    {
        this.gameObject.SetActive(false);
    }
}
