using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideMe : MonoBehaviour
{
    private GameObject me;

    void Start()
    {
        me = this.gameObject;
        me.SetActive(false);
        Debug.Log(this.gameObject.name + " HideMe!");
    }
}
