using UnityEngine;

public class WeaponsManager : MonoBehaviour
{
    public GameObject WeaponsWall;

    public static bool isWeaponSelected = false;

    public delegate void WeaponSelected();
    public static event WeaponSelected onWeaponSelected;

    void Start()
    {
        WeaponsWall.SetActive(false);
    }

    void OnEnable()
    {
        onWeaponSelected += HideWeaponsWall;
    }

    void OnDisable()
    {
        onWeaponSelected -= HideWeaponsWall;
    }

    // weapon selection occurs in SnapZone.cs
    // static method allow access without wiring up elsewhere
    public static void SelectWeapon()
    {
        onWeaponSelected();
    }

    private void HideWeaponsWall()
    {
        WeaponsWall.SetActive(false);
    }

    public void ShowWeaponsWall()
    {
        //isWeaponSelected = true;
        WeaponsWall.SetActive(true);
        Debug.Log("Show that weapons wall!");
    }
}
