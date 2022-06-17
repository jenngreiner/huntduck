using UnityEngine;
using System.Collections.Generic;
using Oculus.Platform;
using Oculus.Platform.Models;

// Coordinates updating leaderboard scores and polling for leaderboard updates.
public class LeaderboardManager : MonoBehaviour
{
	// API NAME for the leaderboard where we store the user's match score
	private const string HIGHEST_MATCH_SCORE = "Leaderboard";

	// the top number of entries to query
	private const int TOP_N_COUNT = 10;

	// how often to poll the service for leaderboard updates
	private const float LEADERBOARD_POLL_FREQ = 30.0f;

	// the next time to check for leaderboard updates
	private float m_nextCheckTime;

	// cache to hold high-score leaderboard entries as they come in
	private volatile SortedDictionary<int, LeaderboardEntry> m_highScores;

	// whether we've found the local user's entry yet
	private bool m_foundLocalUserHighScore;

	// callback to deliver the high-scores leaderboard entries
	private OnHighScoreLeaderboardUpdated m_highScoreCallback;


	public void CheckForUpdates()
	{
		if (Time.time >= m_nextCheckTime)
		{
			m_nextCheckTime = Time.time + LEADERBOARD_POLL_FREQ;

			QueryHighScoreLeaderboard();
		}
	}


	#region Highest Score Board

	public delegate void OnHighScoreLeaderboardUpdated(SortedDictionary<int, LeaderboardEntry> entries);

	public OnHighScoreLeaderboardUpdated HighScoreLeaderboardUpdatedCallback
	{
		set { m_highScoreCallback = value; }
	}

	// made this public
	public void QueryHighScoreLeaderboard()
	{
		// if a query is already in progress, don't start a new one.
		if (m_highScores != null)
			return;

		m_highScores = new SortedDictionary<int, LeaderboardEntry>();
		m_foundLocalUserHighScore = false;

		Leaderboards.GetEntries(HIGHEST_MATCH_SCORE, TOP_N_COUNT, LeaderboardFilterType.None,
			LeaderboardStartAt.Top).OnComplete(HighestScoreGetEntriesCallback);
	}

	void HighestScoreGetEntriesCallback(Message<LeaderboardEntryList> msg)
	{
		if (!msg.IsError)
		{
			foreach (LeaderboardEntry entry in msg.Data)
			{
				m_highScores[entry.Rank] = entry;

                if (entry.User.ID == huntduck.PlatformManager.MyID)
                {
                    m_foundLocalUserHighScore = true;
                }
                Debug.Log("m_highScores: " + m_highScores);
            }

			// results might be paged for large requests
			if (msg.Data.HasNextPage)
			{
				Leaderboards.GetNextEntries(msg.Data).OnComplete(HighestScoreGetEntriesCallback); ;
				return;
			}

			// if local user not in the top, get their position specifically
			if (!m_foundLocalUserHighScore)
			{
				Leaderboards.GetEntries(HIGHEST_MATCH_SCORE, 1, LeaderboardFilterType.None,
					LeaderboardStartAt.CenteredOnViewer).OnComplete(HighestScoreGetEntriesCallback);
				return;
			}
		}
		// else an error is returned if the local player isn't ranked - we can ignore that

		if (m_highScoreCallback != null)
		{
			m_highScoreCallback(m_highScores);
		}
		m_highScores = null;
	}
	#endregion

	// submit the local player's match score to the leaderboard service
	//public void SubmitMatchScores(bool wonMatch, uint score)
	//{
	//	if (score > 0)
	//	{
	//		Leaderboards.WriteEntry(HIGHEST_MATCH_SCORE, score);
	//	}
	//}

	public void SubmitMatchScores(uint score)
	{
		if (score > 0)
		{
			Leaderboards.WriteEntry(HIGHEST_MATCH_SCORE, score);
		}
	}
}