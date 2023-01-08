using UnityEngine;

public class SetPlayerPosition : MonoBehaviour
{
    private GameObject playerController;

    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag(TagManager.PLAYER_CONTROLLER_TAG);
        playerController.transform.position = transform.position;

        Debug.Log("SetPlayerPosition via playercontroller to " + transform.position);
    }
}
