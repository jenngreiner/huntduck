using UnityEngine;

// show and hide weapons wall off key events
public class WeaponsManager
{
    public delegate void WeaponSelected();
    public static event WeaponSelected onWeaponSelected;

    // weapon selection occurs in SnapZone.cs
    public static void SelectWeapon()
    {
        onWeaponSelected?.Invoke();
    }
}