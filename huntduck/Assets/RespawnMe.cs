using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BNG.Damageable))]
public class RespawnMe : MonoBehaviour
{
    private BNG.Damageable damageableScript;

    private void Start()
    {
        damageableScript = transform.GetComponent<BNG.Damageable>();
    }

    private void OnEnable()
    {
        BNG.Damageable.onDestroyedDelegate += RespawnThisObject;
    }

    private void OnDisable()
    {
        BNG.Damageable.onDestroyedDelegate -= RespawnThisObject;
    }

    void RespawnThisObject()
    {
        damageableScript.InstantRespawn();
    }
}
