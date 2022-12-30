using UnityEngine;

public class SetPlayerName : MonoBehaviour
{
    public string yourName;

    void Start()
    {
        // set the name on this object to yourName
        transform.name = yourName;
    }
}
