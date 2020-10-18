using UnityEngine;
using System;
using TMPro;

public class UserList1 : MonoBehaviour
{
    public GameObject EntryPrefab;

    // Use this for initialization
    public void ShowLeaderboard (string boardName)
    {
		// Remove any existing entries
		foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Get the leaderboard data
        var leaderboards = UserLeaderboard1.GetLeaderboardAroundPlayer(boardName);

		if (leaderboards.Count > 0)
		{
			if (boardName == "time2")
			{
				foreach (var leaderboard in leaderboards)
				{
					var entry = Instantiate(EntryPrefab);

					entry.transform.SetParent(this.transform, false);
					entry.transform.Find("Player").GetComponent<TextMeshProUGUI>().text = leaderboard.DisplayName ?? leaderboard.PlayFabId;
					entry.transform.Find("Rank").GetComponent<TextMeshProUGUI>().text = (leaderboard.Position + 1).ToString();
					float t = (((float)leaderboard.StatValue) / 100) * -1;
					string t2 = t.ToString();
					entry.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = t2;
				}
			}
			else if (boardName == "deaths2")
			{
				// Create UI entries for each
				foreach (var leaderboard in leaderboards)
				{
					var entry = Instantiate(EntryPrefab);

					entry.transform.SetParent(this.transform, false);
					entry.transform.Find("Player").GetComponent<TextMeshProUGUI>().text = leaderboard.DisplayName ?? leaderboard.PlayFabId;
					entry.transform.Find("Rank").GetComponent<TextMeshProUGUI>().text = (leaderboard.Position + 1).ToString();
					entry.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = (leaderboard.StatValue * -1).ToString();
				}
			}
			else
			{
				// Create UI entries for each
				foreach (var leaderboard in leaderboards)
				{
					var entry = Instantiate(EntryPrefab);

					entry.transform.SetParent(this.transform, false);
					entry.transform.Find("Player").GetComponent<TextMeshProUGUI>().text = leaderboard.DisplayName ?? leaderboard.PlayFabId;
					entry.transform.Find("Rank").GetComponent<TextMeshProUGUI>().text = (leaderboard.Position + 1).ToString();
					entry.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = leaderboard.StatValue.ToString();
				}
            }
		}
		else
		{
			var entry = Instantiate(EntryPrefab);
			entry.transform.SetParent(this.transform, false);
			entry.transform.Find("Player").GetComponent<TextMeshProUGUI>().text = "Leaderboard is empty";
			entry.transform.Find("Rank").GetComponent<TextMeshProUGUI>().text = String.Empty;
			entry.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = String.Empty;
		}
    }
}
