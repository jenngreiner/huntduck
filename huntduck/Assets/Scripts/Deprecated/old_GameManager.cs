//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class GameManager : MonoBehaviour
//{

//    private const string PLAYER_ID_PREFIX = "Player ";

//    private static Dictionary<string, Duck> players = new Dictionary<string, Duck>();

//    public void RegisterPlayer(string _ID, Duck _player)
//    {
//        string _playerID = PLAYER_ID_PREFIX + _ID;
//        players.Add(_playerID, _player);
//        _player.transform.name = _playerID;
//    }

//    public static void UnRegisterPlayer(string _playerID)
//    {
//        players.Remove(_playerID);
//    }

//    public static Duck GetPlayer(string _playerID)
//    {
//        return players[_playerID];
//    }

//}
