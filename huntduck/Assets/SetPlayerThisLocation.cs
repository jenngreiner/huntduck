using UnityEngine;

public class SetPlayerThisLocation : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player.transform.position = transform.position;
        player.transform.rotation = transform.rotation;
    }
}