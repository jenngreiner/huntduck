using UnityEngine;
using Photon.Pun;

public class PlayerSpawnPoint : MonoBehaviourPun
{
    private bool isAvailable = true; // whether spawn point currently available
    public int playerID; // the ID of the player who is currently occupying this spawn point, if any

    [PunRPC]
    public void SetAvailability(bool availability)
    {
        isAvailable = availability;
    }

    public bool GetAvailability()
    {
        return isAvailable;
    }

}