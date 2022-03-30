using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFly : MonoBehaviour
{
    public List<GameObject> kids;
    public float speed;

    public delegate void FlyingVHit();
    public static event FlyingVHit onFlyingVHit;

    void Start()
    {
        kids = new List<GameObject>();

        foreach (Transform child in transform)
        {
            kids.Add(child.gameObject);
        }
    }

    void Update()
    {
        if (kids.Count <= 0)
        {
            onFlyingVHit.Invoke();
            Destroy(gameObject);
        }
        else
        {
            RemoveEmptiesFromKids();
        }

        transform.Translate(Vector3.forward * speed);
    }

    void RemoveEmptiesFromKids()
    {
        for (int i = kids.Count -1; i > -1; i--)
        {
            if(kids[i] == null)
            {
                kids.RemoveAt(i);
            }
        }
    }
}