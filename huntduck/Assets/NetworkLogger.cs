using System.Linq;  // Required for the Skip() method
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;

public class NetworkLogger : MonoBehaviourPunCallbacks
{
    public Text networkLogs;
    public Text playerListText;

    public static NetworkLogger instance { get; private set; }

    // Maximum number of log lines to keep in the UI text
    private const int maxLogLines = 100;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public static void LogNetworkMessage(string message)
    {
        if (instance != null && instance.photonView != null)
        {
            // instance.photonView.RPC("LogText", RpcTarget.AllBufferedViaServer, message);
        }
        else
        {
            Debug.LogWarning("[NetworkLogger] Instance is null! Cannot send network log.");
        }
    }

    void Start()
    {
        Debug.Log("NetworkLogger is now operational");

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("LogText", RpcTarget.AllBufferedViaServer, "Current Room is: <color=aqua>" + PhotonNetwork.CurrentRoom.Name + "</color>");
            photonView.RPC("LogText", RpcTarget.AllBufferedViaServer, "The allmighty room creator is: <color=orange>" + PhotonNetwork.MasterClient.NickName + "</color>");
            photonView.RPC("UpdatePlayerListUI", RpcTarget.All);
        }

        Application.logMessageReceived += HandleLog;
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        Application.logMessageReceived -= HandleLog;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        string formattedLog = "";

        switch (type)
        {
            case LogType.Error:
                formattedLog = $"<color=red>[ERROR] </color> {logString} \n {stackTrace}";
                break;
            case LogType.Warning:
                formattedLog = $"<color=yellow>[WARNING] </color> {logString}";
                break;
            case LogType.Exception:
                formattedLog = $"<color=red>[EXCEPTION] </color> {logString} \n {stackTrace}";
                break;
            case LogType.Log:
                formattedLog = $"<color=green>[INFO] </color> {logString}";
                break;
            default:
                formattedLog = $"<color=orange>[UNKNOWN] </color> {logString}";
                break;
        }

        if (logString.Length > 0) // Avoid blank log lines
        {
            // Filter out internal Photon RPC logs if needed
            if (logString.StartsWith("Sending RPC") || logString.StartsWith("Received RPC"))
                return;

            photonView.RPC("LogText", RpcTarget.AllBufferedViaServer, formattedLog);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        // This callback fires on all clients.
        photonView.RPC("LogText", RpcTarget.AllBufferedViaServer, "Welcome new player: <color=orange>" + newPlayer.NickName + "</color>");
        photonView.RPC("UpdatePlayerListUI", RpcTarget.All);
    }

    public override void OnPlayerLeftRoom(Player leavingPlayer)
    {
        base.OnPlayerLeftRoom(leavingPlayer);

        photonView.RPC("LogText", RpcTarget.AllBufferedViaServer, "Goodbye old friend: <color=red>" + leavingPlayer.NickName + "</color>");
        photonView.RPC("UpdatePlayerListUI", RpcTarget.All);
    }

    [PunRPC]
    void UpdatePlayerListUI()
    {
        // Clear the current player list
        playerListText.text = "";

        if (PhotonNetwork.IsConnectedAndReady)
        {
            // Add the current player count to the player list
            playerListText.text += "Total Players: <color=orange>" + PhotonNetwork.CurrentRoom.PlayerCount + "</color>\n";

            // Add each player's nickname to the player list
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                playerListText.text += "<color=orange>" + player.NickName + "</color>\n";
            }
        }
    }

    [PunRPC]
    void LogText(string message)
    {
        if (networkLogs)
        {
            // Append the new log message
            networkLogs.text += "\n" + message;

            // Split the text into individual lines
            string[] lines = networkLogs.text.Split('\n');
            if (lines.Length > maxLogLines)
            {
                // Keep only the last 'maxLogLines' lines
                networkLogs.text = string.Join("\n", lines.Skip(lines.Length - maxLogLines).ToArray());
            }
        }
    }
}


//using UnityEngine;
//using UnityEngine.UI;
//using Photon.Pun;
//using Photon.Realtime;
//using System.Collections;

//public class NetworkLogger : MonoBehaviourPunCallbacks
//{
//    public Text networkLogs;
//    public Text playerListText;

//    //bool hasLoadedScene;

//    public static NetworkLogger instance { get; private set; }


//    private void Awake()
//    {
//        if (instance == null)
//        {
//            instance = this;
//        }
//    }

//    public static void LogNetworkMessage(string message)
//    {
//        if (instance != null && instance.photonView != null)
//        {
//            // instance.photonView.RPC("LogText", RpcTarget.AllBufferedViaServer, message);
//        }
//        else
//        {
//            Debug.LogWarning("[NetworkLogger] Instance is null! Cannot send network log.");
//        }
//    }


//    void Start()
//    {
//        // when scene (and thus this script) loads on network, update the player list for everyone
//        Debug.Log("NetworkLogger is now operational");

//        if (PhotonNetwork.IsMasterClient)
//        {
//            photonView.RPC("LogText", RpcTarget.AllBufferedViaServer, "Current Room is: <color=aqua>" + PhotonNetwork.CurrentRoom.Name + "</color>");
//            photonView.RPC("LogText", RpcTarget.AllBufferedViaServer, "The allmighty room creator is: <color=orange>" + PhotonNetwork.MasterClient.NickName + "</color>");
//            photonView.RPC("UpdatePlayerListUI", RpcTarget.All);
//        }

//        Application.logMessageReceived += HandleLog;
//    }

//    private void OnDestroy()
//    {
//        // Unsubscribe to prevent memory leaks
//        Application.logMessageReceived -= HandleLog;
//    }

//    private void HandleLog(string logString, string stackTrace, LogType type)
//    {
//        string formattedLog = "";


//        switch (type)
//        {
//            case LogType.Error:
//                formattedLog = $"<color=red>[ERROR] </color> {logString} \n {stackTrace}";
//                break;
//            case LogType.Warning:
//                formattedLog = $"<color=yellow>[WARNING] </color> {logString}";
//                break;
//            case LogType.Exception:
//                formattedLog = $"<color=red>[EXCEPTION] </color> {logString} \n {stackTrace}";
//                break;
//            case LogType.Log:
//                formattedLog = $"<color=green>[INFO] </color> {logString}";
//                break;
//            default:
//                formattedLog = $"<color=orange>[UNKNOWN] </color> {logString}";
//                break;
//        }
//        if (logString.Length > 0) // Solution to avoid blank log lines
//        {
//            photonView.RPC("LogText", RpcTarget.AllBufferedViaServer, formattedLog);
//        }

//    }


//    // TODO: try switching logtext functions to these callbacks
//    //public override void OnJoinedRoom()
//    //{
//    //    base.OnJoinedRoom();
//    //}

//    //public override void OnLeftRoom()
//    //{
//    //    base.OnLeftRoom();
//    //}

//    public override void OnPlayerEnteredRoom(Player newPlayer)
//    {
//        base.OnPlayerEnteredRoom(newPlayer);

//        //will only fire on non-master clients because of this callback fires before PhotonNetwork.LoadLevel is completed by master
//        photonView.RPC("LogText", RpcTarget.AllBufferedViaServer, "Welcome new player: <color=orange>" + newPlayer.NickName + "</color>");
//        photonView.RPC("UpdatePlayerListUI", RpcTarget.All);
//    }

//    public override void OnPlayerLeftRoom(Player leavingPlayer)
//    {
//        base.OnPlayerLeftRoom(leavingPlayer);

//        photonView.RPC("LogText", RpcTarget.AllBufferedViaServer, "Goodbye old friend: <color=red>" + leavingPlayer.NickName + "</color>");
//        photonView.RPC("UpdatePlayerListUI", RpcTarget.All);
//    }

//    [PunRPC]
//    void UpdatePlayerListUI()
//    {
//        // Clear the current player list
//        playerListText.text = "";

//        if (PhotonNetwork.IsConnectedAndReady)
//        {
//            // Add the current player count to the player list
//            playerListText.text += "Total Players: <color=orange>" + PhotonNetwork.CurrentRoom.PlayerCount + "</color>\n";

//            // Add each player's nickname to the player list
//            foreach (Player player in PhotonNetwork.PlayerList)
//            {
//                playerListText.text += "<color=orange>" + player.NickName + "</color>\n";
//            }
//        }
//    }

//    [PunRPC]
//    void LogText(string message)
//    {
//        // Output to worldspace to help with debugging.
//        if (networkLogs)
//        {
//            networkLogs.text += "\n" + message;
//        }

//        // Debug.Log(message);
//    }
//}

//using UnityEngine;
//using UnityEngine.UI;
//using Photon.Pun;
//using Photon.Realtime;

//public class NetworkLogger : MonoBehaviourPunCallbacks
//{
//    public Text networkLogs;
//    public Text playerListText;

//    bool hasLoadedScene;

//    public static NetworkLogger instance { get; private set; }

//    void Start()
//    {
//        // when scene (and thus this script) loads on network, update the player list for everyone
//        Debug.Log("NetworkLogger is now operational");

//        if (PhotonNetwork.IsMasterClient)
//        {
//            photonView.RPC("LogText", RpcTarget.AllBufferedViaServer, "Current Room is: <color=aqua>" + PhotonNetwork.CurrentRoom.Name + "</color>");
//            photonView.RPC("LogText", RpcTarget.AllBufferedViaServer, "The allmighty room creator is: <color=orange>" + PhotonNetwork.MasterClient.NickName + "</color>");
//            photonView.RPC("UpdatePlayerListUI", RpcTarget.All);
//        }

//    }

//    // TODO: try switching logtext functions to these callbacks
//    //public override void OnJoinedRoom()
//    //{
//    //    base.OnJoinedRoom();
//    //}

//    //public override void OnLeftRoom()
//    //{
//    //    base.OnLeftRoom();
//    //}

//    public override void OnPlayerEnteredRoom(Player newPlayer)
//    {
//        base.OnPlayerEnteredRoom(newPlayer);

//        //will only fire on non-master clients because of this callback fires before PhotonNetwork.LoadLevel is completed by master
//        photonView.RPC("LogText", RpcTarget.AllBufferedViaServer, "Welcome new player: <color=orange>" + newPlayer.NickName + "</color>");
//        photonView.RPC("UpdatePlayerListUI", RpcTarget.All);
//    }

//    public override void OnPlayerLeftRoom(Player leavingPlayer)
//    {
//        base.OnPlayerLeftRoom(leavingPlayer);

//        photonView.RPC("LogText", RpcTarget.AllBufferedViaServer, "Goodbye old friend: <color=red>" + leavingPlayer.NickName + "</color>");
//        photonView.RPC("UpdatePlayerListUI", RpcTarget.All);
//    }

//    [PunRPC]
//    void UpdatePlayerListUI()
//    {
//        // Clear the current player list
//        playerListText.text = "";

//        if (PhotonNetwork.IsConnectedAndReady)
//        {
//            // Add the current player count to the player list
//            playerListText.text += "Total Players: <color=orange>" + PhotonNetwork.CurrentRoom.PlayerCount + "</color>\n";

//            // Add each player's nickname to the player list
//            foreach (Player player in PhotonNetwork.PlayerList)
//            {
//                playerListText.text += "<color=orange>" + player.NickName + "</color>\n";
//            }
//        }
//    }

//    [PunRPC]
//    void LogText(string message)
//    {
//        // Output to worldspace to help with debugging.
//        if (networkLogs)
//        {
//            networkLogs.text += "\n" + message;
//        }

//        Debug.Log(message);
//    }
//}