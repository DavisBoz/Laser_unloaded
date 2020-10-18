using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;

public class SampleManager1 : MonoBehaviour
{ 

	private Dictionary<string, int> GameStats = new Dictionary<string, int>();

	private void Awake()
	{
		RefreshLeaderboards();
	}

	private void Start()
	{
		RefreshLeaderboards();
	}

	private void OnGotLeaderboards()
	{
		GoToLeaderboards();
	}
	private void OnGotLeaderboardsFailed()
	{
		Debug.LogError("ERROR: Failed to retrieve Leaderboards. Verify configuration on PlayFab GameManager matches the supplied documentation.");
	}

	// Selected leaderboard
	private int CurrentLeaderboard = 0;

	// UX strings for the leaderboards
	private string[,] LeaderboardInfo = new string[,]
	{
		{ "highscore2", "Level 2 High Scores", "Points" },
		{ "time2", "Level 2 Best Times", "Seconds" },
		{ "kills2", "Level 2 Most Kills", "Kills" },
		{ "deaths2", "Level 2 Least Deaths", "Deaths"}
	};

	public void RefreshLeaderboards()
	{
		PlayfabManager1.LoadLeaderboards(OnGotLeaderboards, OnGotLeaderboardsFailed);
	}

	public void GoToLeaderboards()
	{
		ShowLeaderboard(CurrentLeaderboard);
	}

	public void NextLeaderboard()
	{
		CurrentLeaderboard++;

		if (CurrentLeaderboard == LeaderboardInfo.GetLength(0))
		{
			CurrentLeaderboard = 0;
		}

		RefreshLeaderboards();

        //ShowLeaderboard(CurrentLeaderboard);
    }

	public void PrevLeaderboard()
	{
		CurrentLeaderboard--;

		if (CurrentLeaderboard < 0)
		{
			CurrentLeaderboard = LeaderboardInfo.GetLength(0) - 1;
		}

		RefreshLeaderboards();
	}

	private void ShowLeaderboard(int boardIndex)
	{
		var list = GameObject.Find("Score List");
		var script = list.GetComponent<ScoreList1>();

		script.ShowLeaderboard(
			LeaderboardInfo[CurrentLeaderboard, 0],
			LeaderboardInfo[CurrentLeaderboard, 1],
			LeaderboardInfo[CurrentLeaderboard, 2]
			);
	}

}
