using UnityEngine;

// show and hide weapons wall off key events
public class WeaponsManager : MonoBehaviour
{
    public delegate void WeaponSelected();
    public static event WeaponSelected onWeaponSelected;

    // weapon selection occurs in SnapZone.cs
    // static method allow access without wiring up elsewhere
    public static void SelectWeapon()
    {
        onWeaponSelected();
    }
}
