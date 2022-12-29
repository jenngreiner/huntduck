//using UnityEngine;
//using UnityEngine.SceneManagement;
//using Photon.Pun;

//public class MultiplayerJoinRoom : MonoBehaviourPunCallbacks
//{
//    public Scene multiplayerScene; // the multiplayer scene we want to load

//    public Transform[] spawnPointsArray; //array of spawnpoints

//    public GameObject playerPrefab; // prefab of the player object

//    private AsyncOperation asyncLoad; // reference to async load operation, so we know if scene is loaded

//    private void Start()
//    {
//        asyncLoad = SceneManager.LoadSceneAsync(multiplayerScene.name);
//        asyncLoad.allowSceneActivation = false; // set the allowSceneActivation property to false to prevent scene from being activated automatically

//        // Subscribe to the sceneLoaded event
//        SceneManager.sceneLoaded += OnSceneLoaded;
//    }

//    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//    {
//        // check if the scene that was just loaded is the multiplayerscene
//        if(scene.name == multiplayerScene.name)
//        {
//            // find all the GameObjects with "SpawnPoint" tag in scene and stick them in spawnPointObjects array
//            GameObject[] spawnPointObjects = GameObject.FindGameObjectsWithTag(TagManager.PLAYER_SPAWNPOINT_TAG);
//            spawnPointsArray = new Transform[spawnPointObjects.Length]; // create the spawn points array at the length of the number of spawnpoints in scene

//            // fill the array we just made with the actual spawn point GameObject transforms (position, rotation)
//            for (int i = 0; i < spawnPointObjects.Length; i++)
//            {
//                Debug.Log("OnSceneLoaded i BEFORE running function = " + i);
//                spawnPointsArray[i] = spawnPointObjects[i].transform;
//            }
//        }
//    }

//    public override void OnJoinedRoom()
//    {
//        //check if the room is full
//        if(PhotonNetwork.CurrentRoom.PlayerCount >= 3)
//        {
//            //create new room if room is full
//            PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 3 });
//        }

//        //check if scene containing the spawn points is already loaded
//        if (SceneManager.GetSceneByName(multiplayerScene.name).isLoaded)
//        {
//            int spawnPointIndex = -1;
//            for (int i = 0; i < spawnPoints.Length; i++)
//            {
//                if (spawnPoints[i].GetComponent<SpawnPoint>().isAvailable)
//                {
//                    spawnPointIndex = i;
//                    break;
//                }
//            }

//            // if an available spawn point was found, use it to spawn the player
//            if (spawnPointIndex != -1)
//            {
//                PhotonNetwork.Instantiate("PlayerPrefab", spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
//                spawnPoints[spawnPointIndex].GetComponent<SpawnPoint>().isAvailable = false;  // mark the spawn point as unavailable
//            }
//            else
//            {
//                Debug.LogError("No available spawn points!");
//            }
//            //playerPrefab.transform.position = spawnPoint.position;
//            //playerPrefab.transform.rotation = spawnPoint.rotation;

//            DontDestroyOnLoad(playerPrefab);
//        }
//        else
//        {
//            asyncLoad.allowSceneActivation = true;
//        }

//    }
//}
