using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    public bool isAvailable = true; // whether spawn point currently available
    public int playerID; // the ID of the player who is currently occupying this spawn point, if any
}
