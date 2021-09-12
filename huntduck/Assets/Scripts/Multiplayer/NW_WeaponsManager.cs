using UnityEngine;
using Photon.Pun;

// show and hide weapons wall off key events
public class NW_WeaponsManager : MonoBehaviour
{
    public delegate void WeaponSelected();
    public static event WeaponSelected onWeaponSelected;

    //public GameObject WeaponsWall;
    //public AudioSource levelupSound;

    // weapon selection occurs in SnapZone.cs
    // static method allow access without wiring up elsewhere
    [PunRPC]
    public static void RPC_SelectWeapon()
    {
        onWeaponSelected();
    }

    //public void ShowWeaponsWall()
    //{
    //    WeaponsWall.SetActive(true);
    //    Debug.Log("Show that weapons wall!");
    //    levelupSound.Play();
    //    Debug.Log("Playing levelupsound");
    //}
}