using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

// relies on Weapon being held by SnapZone
public class WeaponPickedUp : MonoBehaviour {
    public delegate void WeaponPickedUpD();
    public static event WeaponPickedUpD onWeaponPickedUp;

    // subscribe to events
    void OnEnable()
    {
        //BNG.SnapZone.On
        //onWeaponPickedUp
    }

    // unsubscribe to events
    void OnDisable()
    {
        
    }
}
