using UnityEngine;

// show and hide weapons wall off key events
public class WeaponsManager : MonoBehaviour
{
    public delegate void WeaponSelected();
    public static event WeaponSelected onWeaponSelected;

    public GameObject shotgun;
    public GameObject shotgunRing;


    void OnEnable()
    {
        SurvivalWaveSpawner.onWeaponUnlocked += ShowWeapon;
    }

    void Oisable()
    {
        SurvivalWaveSpawner.onWeaponUnlocked -= ShowWeapon;
    }

    public void ShowWeapon(int waveNumber)
    {
        switch(waveNumber)
        {
            case 5:
                // shotgun appears
                shotgun.SetActive(true);
                shotgunRing.SetActive(true);
                break;
            case 10:
                // bow appears
                break;
            default:
                break;
        }
    }

    // weapon selection occurs in SnapZone.cs
    public static void SelectWeapon()
    {
        onWeaponSelected?.Invoke();
    }
}