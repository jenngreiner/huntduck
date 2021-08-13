using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


namespace Com.HuntDuck
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields
        [Tooltip("The maximum number of players per room. Whena room is full, it can't be joined by new players, and so new room will be created.")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;


        [Tooltip("The Ui panel to let the user enter name, connect, and play")]
        [SerializeField]
        private GameObject controlPanel;
        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        private GameObject progressLabel;

        #endregion

        #region Private Fields


        /// <summary>
        /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
        /// </summary>
        string gameVersion = "1";

        #endregion

        #region Monobehaviour CallBacks

        /// <summary>
        /// Monobehavior method called on GameObject by Unity during initialization phase.
        /// </summary>
        ///
        private void Awake()
        {
            /// #Critical
            /// this makes sure we can use PhtonNetwork.LoadLevel() on the master client and all clients in the same room synctheir level automatically.
            ///
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        /// <summary>
        /// Monobehavior method called on GameObject by Unity during initialization phase.
        /// </summary>
        ///
        void Start()
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }



        #endregion

        #region Public Methods

        ///<summary>
        ///Start the connection process.
        /// - If already connected, we attempt joining a random room
        /// - If not yet connected, Connect this application instance to Photon Cloud Network
        /// </summary>

        public void Connect()
        {
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);
            // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
            if (PhotonNetwork.IsConnected)
            {
                // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                // #Critical, we must first and foremost connect to Photon Online Server.
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }

        #endregion


        #region MonobehaviourPunCallbacks Callbacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN Basics Tutorials/Launcher: OnConnectedToMaster() was called by PUN");

            // #Critical: The first we try to do is to join a potential existingroom. If there is, good, else we'll be caled back with OnJoinRandomFailed()
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            progressLabel.SetActive(false);
            progressLabel.SetActive(true);
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with the reason {0}", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
            // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.

            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("PUN Basics Tutorial / Launcher: OnJoinedRoom() was called by PUN. Now this client was in a room.");

            // only load if first player, else use 'PhotonNetwork.AutomaticallySyncScene
            Debug.Log("We load the 'Room for 1'");

            // Load the Room Level
            PhotonNetwork.LoadLevel("Room for 1");
        }
        #endregion

    }
}
