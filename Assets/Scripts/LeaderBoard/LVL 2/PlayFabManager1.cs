using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public static class PlayfabManager1
{

    // List of server configured leaderboards
    public static string[] LeaderboardList = 
    { 
        "highscore2",
        "time2",
        "deaths2",
        "kills2"
    };

	private static int m_NumLeaderboardSuccess = 0;
	private static  int m_NumLeaderboardError = 0;
	private static void OnLeaderboardResult(bool bSuccess, System.Action onSuccess, System.Action onFailed)
	{
		if (bSuccess)
		{
			++m_NumLeaderboardSuccess;
		}
		else
		{
			++m_NumLeaderboardError;
		}

		// Are we done?
		if (m_NumLeaderboardSuccess + m_NumLeaderboardError == LeaderboardList.Length)
		{
			if (m_NumLeaderboardError == 0)
			{
				onSuccess();
			}
			else
			{
				onFailed();
			}
		}
	}

    ////////////////////////////////////////////////////////////////
    /// Load the leaderboard data
    /// 
    public static void LoadLeaderboards(System.Action onSuccess, System.Action onFailed)
    {
		m_NumLeaderboardSuccess = 0;
		m_NumLeaderboardError = 0;

		LeaderboardData = new Dictionary<string, List<PlayerLeaderboardEntry>>();

        foreach (string board in LeaderboardList)
        {
            PlayFabClientAPI.GetLeaderboard(
                // Request
                new GetLeaderboardRequest
                {
                    StatisticName = board,
                    StartPosition = 0,
                    MaxResultsCount = MaxLeaderboardEntries
                },
                // Success
                (GetLeaderboardResult result) =>
                {
                    var boardName = (result.Request as GetLeaderboardRequest).StatisticName;
                    LeaderboardData[boardName] = result.Leaderboard;
                    AreLeaderboardsLoaded = true;
                    Debug.Log(string.Format("GetLeaderboard completed: {0}", boardName));
					OnLeaderboardResult(true, onSuccess, onFailed);
				},
                // Failure
                (PlayFabError error) =>
                {
                    Debug.LogError("GetLeaderboard failed.");
                    Debug.LogError(error.GenerateErrorReport());
					OnLeaderboardResult(false, onSuccess, onFailed);
				}
                );
        }
	}

    // Max entries to retrieve; based on UI space
    public static int MaxLeaderboardEntries = 9;

    // Flag set when leaderboards have been loaded
    public static bool AreLeaderboardsLoaded = false;

    // Cache for leaderboard data
    private static Dictionary<string, List<PlayerLeaderboardEntry>> LeaderboardData;

    // Access for leaderboards
    public static List<PlayerLeaderboardEntry> GetLeaderboard(string board)
    {
        return LeaderboardData[board];
    }
}
