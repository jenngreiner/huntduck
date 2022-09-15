using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;


namespace Com.HuntDuck
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields
        [Tooltip("The maximum number of players per room. Whena room is full, it can't be joined by new players, and so new room will be created.")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;

        // Used in original script, not using now
        //[Tooltip("The Ui panel to let the user enter name, connect, and play")]
        //[SerializeField]
        //private GameObject controlPanel;
        //[Tooltip("The UI Label to inform the user that the connection is in progress")]
        //[SerializeField]
        //private GameObject progressLabel;

        #endregion

        #region Private Fields
        /// <summary>
        /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
        /// </summary>
        string gameVersion = "1";

        /// Keep track of the current process. Connectino is asynh and based on several callbacks from Photon. Need to keep track of this to properly adjust the behavior when we receive call back by Photon.
        /// Typically this is used for the OnConnectedToMaster() callback.
        bool isConnecting;

        #endregion

        #region Monobehaviour CallBacks

        private void Awake()
        {
            /// <summary>
            /// #Critical
            /// this makes sure we can use PhtonNetwork.LoadLevel() on the master client and all clients in the same room synctheir level automatically.
            /// </summary>
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        //void Start()
        //{
        //    progressLabel.SetActive(false);
        //    controlPanel.SetActive(true);
        //}

        #endregion

        #region Public Methods

        /// <summary>
        ///Start the connection process.
        /// - If already connected, we attempt joining a random room
        /// - If not yet connected, Connect this application instance to Photon Cloud Network
        /// </summary>
        public void Connect()
        {
            //progressLabel.SetActive(true);
            //controlPanel.SetActive(false);
            
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }

        #endregion

        #region MonobehaviourPunCallbacks Callbacks

        public override void OnConnectedToMaster()
        {
            if (isConnecting)
            {
                Debug.Log("Assets/Launcher: OnConnectedToMaster() was called by PUN");

                // #Critical: The first we try to do is to join a potential existingroom. If there is, good, else we'll be caled back with OnJoinRandomFailed()
                PhotonNetwork.JoinRandomRoom();
                isConnecting = false;
            }
        }


        public override void OnDisconnected(DisconnectCause cause)
        {
            //progressLabel.SetActive(false);
            //controlPanel.SetActive(true);
            isConnecting = false;

            Debug.LogWarningFormat("Assets/Launcher: OnDisconnected() was called by PUN with the reason {0}", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("Assets/Launcher: OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

            // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Assets/ Launcher: OnJoinedRoom() was called by PUN. Now this client was in a room.");
            string sceneName = this.gameObject.name;
            // only load if first player, else use 'PhotonNetwork.AutomaticallySyncScene
            Debug.Log("We load the " + sceneName + " scene");

            // Load the Room Level
            PhotonNetwork.LoadLevel(sceneName);
        }
        #endregion

    }
}
