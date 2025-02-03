using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

//TODO: Currently an error when leaving and rejoining the room, scoreboard goes up for both payers. test again, if still an issue, consider adding back in OnCreatedRoom and OnJoinedRoom callbacks 
public class NetworkScoreManager : MonoBehaviourPunCallbacks
{
    // for multiplayer define points for each object
    private const int pointsForClay = 1;
    private const int pointsForDuck = 5;

    void Start()
    {
        InitializePlayerScores();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        InitializePlayerScores();
    }

    void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEventReceived;
    }

    void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEventReceived;
    }


    //GPT INSPIRED METHODS
    private void InitializePlayerScores()
    {
        if (!PhotonNetwork.IsMasterClient) return; // Only the master client should initialize scores

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (!player.CustomProperties.ContainsKey("score"))
            {
                SetPlayerScore(player, 0); // Master client sets initial score
            }
        }
    }

    private void SetPlayerScore(Player player, int score)
    {
        Hashtable scoreProperty = new Hashtable { { "score", score } };
        player.SetCustomProperties(scoreProperty); // Master client sets scores when initializing
    }

    public void UpdatePlayerScore(int score)
    {
        if (PhotonNetwork.LocalPlayer != null) // Ensure we are only updating our own score
        {
            Hashtable scoreProperty = new Hashtable { { "score", score } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(scoreProperty);
        }
    }

    // Revised event receiver
    private void OnEventReceived(EventData photonEvent)
    {
        // Listen for event code 1 (the hit event from RaycastWeapon).
        if (photonEvent.Code == 1)
        {
            object[] data = (object[])photonEvent.CustomData;
            string shooterUserId = (string)data[0];
            string hitObjectTag = (string)data[1];

            // Only process the event if it’s for the local player.
            if (PhotonNetwork.LocalPlayer.UserId == shooterUserId)
            {
                int scoreIncrement = 0;

                // Determine score increment based on the hit object's tag.
                switch (hitObjectTag)
                {
                    case TagManager.PRACTICECLAY_TAG:
                        scoreIncrement = pointsForClay;
                        break;
                    case TagManager.INFINITEDUCK_TAG:
                        scoreIncrement = pointsForDuck;
                        break;
                }

                // Update the local player's score.
                int currentScore = PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("score", out object scoreObj)
                    ? (int)scoreObj
                    : 0;
                UpdatePlayerScore(currentScore + scoreIncrement);
            }
        }


        //ORIGINAL PRE-GPT METHODS BELOW

        //private void InitializePlayerScores()
        //{
        //    foreach (Player player in PhotonNetwork.PlayerList)
        //    {
        //        if (!player.CustomProperties.ContainsKey("score"))
        //        {
        //            UpdatePlayerScore(player, 0); // set initial score to 0
        //        }
        //    }
        //}

        //public void UpdatePlayerScore(Player player, int score)
        //{

        //    Hashtable scoreProperty = new Hashtable();
        //    scoreProperty["score"] = score;
        //    player.SetCustomProperties(scoreProperty);
        //}

        // receive event from RaycastWeapon.cs
        //private void OnEventReceived(EventData photonEvent)
        //{
        //    if (photonEvent.Code == 0)
        //    {
        //        //unpack data from RaiseEvent
        //        object[] data = (object[])photonEvent.CustomData;
        //        string userID = (string)data[0];
        //        string hitObjectTag  = (string)data[1];
        //        int scoreIncrement = 0;

        //        // set correct points to add to score
        //        switch (hitObjectTag)
        //        {
        //            case TagManager.PRACTICECLAY_TAG:
        //                scoreIncrement = pointsForClay;
        //                break;
        //            case TagManager.INFINITEDUCK_TAG:
        //                scoreIncrement = pointsForDuck;
        //                break;
        //        }

        //        //GPT INSPIRED METHOD
        //        foreach (Player player in PhotonNetwork.PlayerList)
        //        {
        //            if (player.UserId == userID)
        //            {
        //                object[] eventData = new object[] { scoreIncrement };
        //                RaiseEventOptions options = new RaiseEventOptions { TargetActors = new int[] { player.ActorNumber } };
        //                SendOptions sendOptions = new SendOptions { Reliability = true };

        //                PhotonNetwork.RaiseEvent(1, eventData, options, sendOptions); // Event code 1 for score update
        //                break;
        //            }
        //        }

        //ORIGINAL PRE-GPT METHOD
        //Find the player and update the score
        //foreach (Player player in PhotonNetwork.PlayerList)
        //{
        //    if (player.UserId == userID)
        //    {
        //        int currentScore = player.CustomProperties.TryGetValue("score", out object scoreObj) ? (int)scoreObj : 0;
        //        UpdatePlayerScore(player, currentScore + scoreIncrement);
        //        break;
        //    }
        //}
    }
}