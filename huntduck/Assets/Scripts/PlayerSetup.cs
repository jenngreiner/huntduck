//using UnityEngine;


//public class PlayerSetup : MonoBehaviour
//{
//    [SerializeField]
//    Behaviour[] componentsToDisable;

//    [SerializeField]
//    string remoteLayerName = "RemotePlayer";

//    private GameObject player;

//    // Use this for initialization
//    void Start()
//    {
//        // Disable components that should only be active on the player that we control
//        if (player.layer.ToString() != "Player")
//        {
//            DisableComponents();
//            AssignRemoteLayer();
//        }


//    }

//    void RegisterPlayer()
//    {
//        string _ID = "Player "

//    }

//    void AssignRemoteLayer()
//    {
//        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
//    }

//    void DisableComponents()
//    {
//        for (int i = 0; i < componentsToDisable.Length; i++)
//        {
//            componentsToDisable[i].enabled = false;
//        }
//    }
//}
