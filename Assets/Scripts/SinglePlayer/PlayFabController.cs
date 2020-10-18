using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PlayFab.Json;


public class PlayFabController : MonoBehaviour
{
    public static PlayFabController PFC;
    public static bool IsLoggedIn = false;
    private string user_email;
    private string user_password;
    private string username;

    public Button login_button;
    public Button iOS_login_button;

    private void OnEnable()
    {
        if (PlayFabController.PFC == null)
        {
            PlayFabController.PFC = this;
        }
        else
        {
            if (PlayFabController.PFC != this)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void Start()
    {
        //PlayerPrefs.DeleteAll();
        //Note: Setting title Id here can be skipped if you have set the value in Editor Extensions already.
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
            PlayFabSettings.TitleId = "E5D52"; // Please change this value to your own titleId from PlayFab Game Manager
        }

        if (PlayerPrefs.HasKey("IOS"))
        {
            username = PlayerPrefs.GetString("IOSUSERNAME");
            var requestiOS = new LoginWithIOSDeviceIDRequest { DeviceId = ReturnMobileID(), CreateAccount = false };
            PlayFabClientAPI.LoginWithIOSDeviceID(requestiOS, OnLoginiOSSuccess, OnLoginiOSFailure);

        }

        else if (PlayerPrefs.HasKey("EMAIL"))
        {
            user_email = PlayerPrefs.GetString("EMAIL");
            user_password = PlayerPrefs.GetString("PASSWORD");
            var request = new LoginWithEmailAddressRequest { Email = user_email, Password = user_password };
            PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
        }
    }

    #region Login
    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        PlayerPrefs.SetString("EMAIL", user_email);
        PlayerPrefs.SetString("PASSWORD", user_password);
        PlayerPrefs.SetString("USERNAME", username);
        IsLoggedIn = true;
        PlayFabController.PFC.GetStats();
        SceneManager.LoadScene("MainMenu");
    }

    public void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        PlayerPrefs.SetString("EMAIL", user_email);
        PlayerPrefs.SetString("PASSWORD", user_password);
        PlayerPrefs.SetString("USERNAME", username);
        IsLoggedIn = true;
        var request_name = new UpdateUserTitleDisplayNameRequest { DisplayName = username };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request_name, resultCallback =>
        {
            SceneManager.LoadScene("MainMenu");
        }, errorCallback =>
        {
            Debug.Log(errorCallback.GenerateErrorReport());
        });
    }

    private void OnLoginFailure(PlayFabError error)
    {
        var register_request = new RegisterPlayFabUserRequest { Email = user_email, Password = user_password, Username = username };
        PlayFabClientAPI.RegisterPlayFabUser(register_request, OnRegisterSuccess, OnRegisterFailure);
    }

    private void OnRegisterFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

    private void OnLoginiOSSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        PlayerPrefs.SetString("IOSUSERNAME", username);
        PlayerPrefs.SetString("IOS", "yes");
        IsLoggedIn = true;
        PlayFabController.PFC.GetStats();
        var request_name = new UpdateUserTitleDisplayNameRequest { DisplayName = username };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request_name, resultCallback =>
        {
            SceneManager.LoadScene("MainMenu");
        }, errorCallback =>
        {
            Debug.Log(errorCallback.GenerateErrorReport());
        });
    }

    private void OnLoginiOSFailure(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    public void GetUserEmail(string email_in)
    {
        user_email = email_in;
    }

    public void GetUserPassword(string password_in)
    {
        user_password = password_in;
    }

    public void GetUsername(string username_in)
    {
        username = username_in;
    }

    public void OnClickLogin()
    {
        var request = new LoginWithEmailAddressRequest { Email = user_email, Password = user_password };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }

    public void OnClickLoginiOS()
    {
        var requestiOS = new LoginWithIOSDeviceIDRequest { DeviceId = ReturnMobileID(), CreateAccount = true };
        PlayFabClientAPI.LoginWithIOSDeviceID(requestiOS, OnLoginiOSSuccess, OnLoginiOSFailure);
    }

    public static string ReturnMobileID()
    {
        string device_id = SystemInfo.deviceUniqueIdentifier;
        return device_id;
    }
    #endregion Login




    public int player_highscore1;
    public int best_time1;
    public int kill1;
    public int death1;
    public int player_highscore2;
    public int best_time2 = -1000;
    public int kill2;
    public int death2 = -1000;
    public int player_highscore3;
    public int best_time3 = -1000;
    public int kill3;
    public int death3 = -1000;
    public int total_kills;
    public int total_score;
    public int total_deaths;
    public int total_time;
    public static string stat_name;
    public static string highscore1;
    public static string highscore2;
    public static string highscore3;

    #region PlayerStats

    public void GetStats()
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            OnGetStats,
            error => Debug.LogError(error.GenerateErrorReport())
        );
    }

    void OnGetStats(GetPlayerStatisticsResult result)
    {
        Debug.Log("Received the following Statistics:");
        foreach (var eachStat in result.Statistics)
        {
            if (eachStat.StatisticName == "highscore1")
            {
                Debug.Log(eachStat.StatisticName + ":" + eachStat.Value);
                highscore1 = eachStat.Value.ToString();
            }

            if (eachStat.StatisticName == "highscore2")
            {
                Debug.Log(eachStat.StatisticName + ":" + eachStat.Value);
                highscore2 = eachStat.Value.ToString();
            }

            if (eachStat.StatisticName == "highscore3")
            {
                Debug.Log(eachStat.StatisticName + ":" + eachStat.Value);
                highscore3 = eachStat.Value.ToString();
            }
        }

    }

    public void StartCloudUpdatePlayer()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "Lvl1")
        {
            player_highscore1 = Preserve.player_highscore1;
            best_time1 = Preserve.time1;
            best_time2 = -10000;
            best_time3 = -10000;
            total_time = -100000;
            death1 = Preserve.deaths1;
            death2 = -100;
            death3 = -100;
            total_deaths = -1000;
            kill1 = Preserve.kills1;
            PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
            {
                FunctionName = "UpdatePlayerStats", // Arbitrary function name (must exist in your uploaded cloud.js file)
                FunctionParameter = new { time1 = best_time1, highscore1 = player_highscore1, kills1 = kill1, deaths1 = death1, time2 = best_time2, deaths2 = death2, time3 = best_time3, deaths3 = death3, deaths = total_deaths, tot_time = total_time }, // The parameter provided to your function
                                                                                                                                  //GeneratePlayStreamEvent = true, // Optional - Shows this event in PlayStream
            }, OnCloudUpdateStat, OnErrorShared);
        }
        if (scene.name == "Lvl2")
        {
            player_highscore2 = Preserve.player_highscore2;
            best_time2 = Preserve.time2;
            best_time1 = -10000;
            best_time3 = -10000;
            total_time = -100000;
            death2 = Preserve.deaths2;
            death1 = -100;
            death3 = -100;
            total_deaths = -1000;
            kill2 = Preserve.kills2;
            PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
            {
                FunctionName = "UpdatePlayerStats", // Arbitrary function name (must exist in your uploaded cloud.js file)
                FunctionParameter = new { time2 = best_time2, highscore2 = player_highscore2, kills2 = kill2, deaths2 = death2, time1 = best_time1, deaths1 = death1, time3 = best_time3, deaths3 = death3, deaths = total_deaths, tot_time = total_time }, // The parameter provided to your function
                                                                                                                                  //GeneratePlayStreamEvent = true, // Optional - Shows this event in PlayStream
            }, OnCloudUpdateStat, OnErrorShared);
        }

        if (scene.name == "Lvl3")
        {
            player_highscore3 = Preserve.player_highscore3;
            best_time3 = Preserve.time3;
            best_time1 = -10000;
            best_time2 = -10000;
            death3 = Preserve.deaths1;
            death1 = -100;
            death2 = -100;
            kill3 = Preserve.kills3;
            total_kills = Preserve.total_kills;
            total_score = Preserve.total_score;
            total_deaths = Preserve.total_deaths;
            total_time = Preserve.time1 + Preserve.time2 + Preserve.time3;
            PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
            {
                FunctionName = "UpdatePlayerStats", // Arbitrary function name (must exist in your uploaded cloud.js file)
                FunctionParameter = new { time3 = best_time3, highscore3 = player_highscore3, kills3 = kill3, deaths3 = death3, time1 = best_time1, deaths1 = death1, time2 = best_time2, deaths2 = death2, tot_kills = total_kills, tot_score = total_score, deaths = total_deaths, tot_time = total_time }, // The parameter provided to your function
                                                                                                                                                                                                                                      //GeneratePlayStreamEvent = true, // Optional - Shows this event in PlayStream
            }, OnCloudUpdateStat, OnErrorShared);
        }
    }


    private static void OnCloudUpdateStat(ExecuteCloudScriptResult result)
    {
        // CloudScript returns arbitrary results, so you have to evaluate them one step and one parameter at a time
        Debug.Log(PlayFab.PluginManager.GetPlugin<ISerializerPlugin>(PluginContract.PlayFab_Serializer));
        JsonObject jsonResult = (JsonObject)result.FunctionResult;
        object messageValue;
        jsonResult.TryGetValue("messageValue", out messageValue); // note how "messageValue" directly corresponds to the JSON values set in CloudScript
        Debug.Log((string)messageValue);
    }

    private static void OnErrorShared(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    #endregion PlayerStats

}
