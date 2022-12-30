using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        if (!playerExists())
        {
            GameObject _player = Instantiate(player, transform.position, transform.rotation);
            _player.name = player.name;
        }
    }

    private bool playerExists()
    {
        GameObject player = GameObject.FindWithTag(TagManager.PLAYER_TAG);

        if (player != null)
        {
            return true;
        }

        return false;
    }
}
