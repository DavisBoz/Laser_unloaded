using UnityEngine;

public class UserManager1 : MonoBehaviour
{ 

	//private Dictionary<string, int> GameStats = new Dictionary<string, int>();

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
		{ "highscore2"},
		{ "time2"},
		{ "kills2"},
		{ "deaths2"}
	};

	public void RefreshLeaderboards()
	{
		UserLeaderboard1.LoadLeaderboards(OnGotLeaderboards, OnGotLeaderboardsFailed);
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
		var list = GameObject.Find("User List");
		var script = list.GetComponent<UserList1>();

		script.ShowLeaderboard(
			LeaderboardInfo[CurrentLeaderboard, 0]
			);
	}

}
