using System;
using System.Linq;
using BNG;
using UnityEngine;

public class WeaponWheelSelect : MonoBehaviour
{
    [Serializable]
    public struct Weapon
    {
        public Grabbable equipGrabbableWeapon;
        public GameObject weaponObject;
        public GameObject weaponCubeOrigin;
    }

    [Tooltip("The Grabber of hand you want this on, right or left")]
    public Grabber handGrabber;

    [Tooltip("The Weapons in the Hierarchy")]
    public Weapon[] weapons;

    public void WeaponSelect(Weapon weapon)
    {
        if (handGrabber.HeldGrabbable == weapon.equipGrabbableWeapon) return;

        if (handGrabber.HeldGrabbable != null)
            DropWeapon(weapons.First(w => w.equipGrabbableWeapon == handGrabber.HeldGrabbable));
        if (handGrabber.HeldGrabbable != null) return;

        ActivateWeapon(weapon);
    }

    private void DropWeapon(Weapon weapon)
    {
        handGrabber.HeldGrabbable.DropItem(handGrabber);
        weapon.equipGrabbableWeapon.transform.SetPositionAndRotation(weapon.weaponCubeOrigin.transform.position,
            weapon.weaponCubeOrigin.transform.rotation);
        weapon.equipGrabbableWeapon.transform.parent = transform;
        weapon.equipGrabbableWeapon.GetComponent<Rigidbody>().isKinematic = true;
        weapon.weaponObject.SetActive(false);
    }

    private void ActivateWeapon(Weapon weapon)
    {
        weapon.weaponObject.SetActive(true);
        weapon.weaponObject.transform.position = handGrabber.transform.position;
        handGrabber.GrabGrabbable(weapon.equipGrabbableWeapon);
        weapon.equipGrabbableWeapon.GetComponent<Rigidbody>().isKinematic = false;
        weapon.equipGrabbableWeapon.transform.SetParent(null);
    }

    public void EmptyHandSelect()
    {
        if (handGrabber.HeldGrabbable == null) return;
        DropWeapon(weapons.First(w => w.equipGrabbableWeapon == handGrabber.HeldGrabbable));
        handGrabber.HeldGrabbable?.DropItem(handGrabber);
    }
}