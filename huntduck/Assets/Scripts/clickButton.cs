using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;


public class clickButton : MonoBehaviour
{
    public DemoScript _demoScript;

    // Start is called before the first frame update
    void Start()
    {
        


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("k"))
        {
            _demoScript.ShootLauncher();
        }
    }
}
