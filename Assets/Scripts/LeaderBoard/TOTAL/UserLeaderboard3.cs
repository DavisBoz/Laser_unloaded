using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public static class UserLeaderboard3
{

    // List of server configured leaderboards
    public static string[] LeaderboardList = 
    {
        "tot_score",
        "tot_time",
        "deaths",
        "tot_kills"
    };

    public static string MyPlayfabID;

    public static void Awake()
    {
        GetAccountInfo();
    }

    public static void Start()
    {
        GetAccountInfo();
    }

    public static void GetAccountInfo()
    {
        GetAccountInfoRequest request = new GetAccountInfoRequest();
        PlayFabClientAPI.GetAccountInfo(request, Successs, fail);
    }


    public static void Successs(GetAccountInfoResult result)
    {

        MyPlayfabID = result.AccountInfo.PlayFabId;

    }


    public static void fail(PlayFabError error)
    {

        Debug.LogError(error.GenerateErrorReport());
    }
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
            PlayFabClientAPI.GetLeaderboardAroundPlayer(
                // Request
                new GetLeaderboardAroundPlayerRequest
                {
                    StatisticName = board,
                    PlayFabId= MyPlayfabID,
                    MaxResultsCount = MaxLeaderboardEntries,
                },
                // Success
                (GetLeaderboardAroundPlayerResult result) =>
                {
                    var boardName = (result.Request as GetLeaderboardAroundPlayerRequest).StatisticName;
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
    public static int MaxLeaderboardEntries = 1;

    // Flag set when leaderboards have been loaded
    public static bool AreLeaderboardsLoaded = false;

    // Cache for leaderboard data
    private static Dictionary<string, List<PlayerLeaderboardEntry>> LeaderboardData;

    // Access for leaderboards
    public static List<PlayerLeaderboardEntry> GetLeaderboardAroundPlayer(string board)
    {
        return LeaderboardData[board];
    }
}
