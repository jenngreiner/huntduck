using UnityEngine;

public class OnlyOneMe : MonoBehaviour
{
    public static OnlyOneMe instance { get; private set; }

    void Awake()
    {
        // if there is an instance, and its not me, delete me
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
}