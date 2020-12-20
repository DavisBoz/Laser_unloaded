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
    public string username;

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

        if (PlayerPrefs.HasKey("USERNAME"))
        {
            username = PlayerPrefs.GetString("USERNAME");
            var requestiOS = new LoginWithIOSDeviceIDRequest { DeviceId = ReturnMobileID(), CreateAccount = false };
            PlayFabClientAPI.LoginWithIOSDeviceID(requestiOS, OnLoginiOSSuccess, OnLoginiOSFailure);

        }
    }

    #region Login
    
    private void OnLoginiOSSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        PlayerPrefs.SetString("USERNAME", username);
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



    //Singleplayer
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

    //Online
    public int mult_kills;
    public int mult_deaths;
    public int kd;
    public int x_p;

    public int o_kills;
    public int o_deaths;
    public int killdeath;
    public int exp;

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

            else if (eachStat.StatisticName == "highscore2")
            {
                Debug.Log(eachStat.StatisticName + ":" + eachStat.Value);
                highscore2 = eachStat.Value.ToString();
            }

            else if (eachStat.StatisticName == "highscore3")
            {
                Debug.Log(eachStat.StatisticName + ":" + eachStat.Value);
                highscore3 = eachStat.Value.ToString();
            }

            else if (eachStat.StatisticName == "online_kills")
            {
                Debug.Log(eachStat.StatisticName + ":" + eachStat.Value);
                o_kills = eachStat.Value;
            }

            else if (eachStat.StatisticName == "online_deaths")
            {
                Debug.Log(eachStat.StatisticName + ":" + eachStat.Value);
                o_deaths = eachStat.Value;
            }

            else if (eachStat.StatisticName == "xp")
            {
                Debug.Log(eachStat.StatisticName + ":" + eachStat.Value);
                exp = eachStat.Value;
            }

            else if (eachStat.StatisticName == "kdr")
            {
                Debug.Log(eachStat.StatisticName + ":" + eachStat.Value);
                killdeath = eachStat.Value;
            }
        }

    }

    public void StartCloudUpdatePlayer()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "Lvl1")
        {
            //singleplayer
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
            //online
            mult_deaths = 0;
            mult_kills = 0;
            kd = 0;
            x_p = 0;
            PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
            {
                FunctionName = "UpdatePlayerStats", // Arbitrary function name (must exist in your uploaded cloud.js file)
                FunctionParameter = new { time1 = best_time1, highscore1 = player_highscore1, kills1 = kill1, deaths1 = death1, time2 = best_time2, deaths2 = death2, time3 = best_time3, deaths3 = death3, deaths = total_deaths, tot_time = total_time, online_kills = mult_kills, online_deaths = mult_deaths,  kdr = kd, xp = x_p}, // The parameter provided to your function
                                                                                                                                  //GeneratePlayStreamEvent = true, // Optional - Shows this event in PlayStream
            }, OnCloudUpdateStat, OnErrorShared);
        }
        else if (scene.name == "Lvl2")
        {
            //singleplayer
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
            //online
            mult_deaths = 0;
            mult_kills = 0;
            kd = 0;
            x_p = 0;
            PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
            {
                FunctionName = "UpdatePlayerStats", // Arbitrary function name (must exist in your uploaded cloud.js file)
                FunctionParameter = new { time1 = best_time1, highscore1 = player_highscore1, kills1 = kill1, deaths1 = death1, time2 = best_time2, deaths2 = death2, time3 = best_time3, deaths3 = death3, deaths = total_deaths, tot_time = total_time, online_kills = mult_kills, online_deaths = mult_deaths, kdr = kd, xp = x_p }, // The parameter provided to your function
                                                                                                                                  //GeneratePlayStreamEvent = true, // Optional - Shows this event in PlayStream
            }, OnCloudUpdateStat, OnErrorShared);
        }

        else if (scene.name == "Lvl3")
        {
            //singleplayer
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
            //online
            mult_deaths = 0;
            mult_kills = 0;
            kd = 0;
            x_p = 0;
            PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
            {
                FunctionName = "UpdatePlayerStats", // Arbitrary function name (must exist in your uploaded cloud.js file)
                FunctionParameter = new { time1 = best_time1, highscore1 = player_highscore1, kills1 = kill1, deaths1 = death1, time2 = best_time2, deaths2 = death2, time3 = best_time3, deaths3 = death3, deaths = total_deaths, tot_time = total_time, online_kills = mult_kills, online_deaths = mult_deaths, kdr = kd, xp = x_p }, // The parameter provided to your function
                                                                                                                                                                                                                                      //GeneratePlayStreamEvent = true, // Optional - Shows this event in PlayStream
            }, OnCloudUpdateStat, OnErrorShared);
        }

        else if (scene.name == "Zenith Base")
        {
            //singleplayer
            player_highscore1 = 0;
            player_highscore2 = 0;
            player_highscore3 = 0;
            best_time3 = -10000;
            best_time1 = -10000;
            best_time2 = -10000;
            death3 = -100;
            death1 = -100;
            death2 = -100;
            kill1 = 0;
            kill2 = 0;
            kill3 = 0;
            total_kills = 0;
            total_score = 0;
            total_deaths = 0;
            total_time = 0;
            //online
            mult_deaths = Preserve.multiplayer_deaths;
            mult_kills = Preserve.multiplayer_kills;
            double kdr = (double)o_kills / o_deaths;
            kdr = System.Math.Round(kdr, 2);
            kd = (int) (kdr * 100);
            x_p = Preserve.multiplayer_xp;
            PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
            {
                FunctionName = "UpdatePlayerStats", // Arbitrary function name (must exist in your uploaded cloud.js file)
                FunctionParameter = new { time1 = best_time1, highscore1 = player_highscore1, kills1 = kill1, deaths1 = death1, time2 = best_time2, deaths2 = death2, time3 = best_time3, deaths3 = death3, deaths = total_deaths, tot_time = total_time, online_kills = mult_kills, online_deaths = mult_deaths, kdr = kd, xp = x_p }, // The parameter provided to your function
                                                                                                                                                                                                                                                                                                              //GeneratePlayStreamEvent = true, // Optional - Shows this event in PlayStream
            }, OnCloudUpdateStat, OnErrorShared);
        }
        else if (scene.name == "Circuit 11")
        {
            //singleplayer
            player_highscore1 = 0;
            player_highscore2 = 0;
            player_highscore3 = 0;
            best_time3 = -10000;
            best_time1 = -10000;
            best_time2 = -10000;
            death3 = -100;
            death1 = -100;
            death2 = -100;
            kill1 = 0;
            kill2 = 0;
            kill3 = 0;
            total_kills = 0;
            total_score = 0;
            total_deaths = 0;
            total_time = 0;
            //online
            mult_deaths = 0;
            mult_kills = 0;
            kd = 0;
            x_p = 0;
            PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
            {
                FunctionName = "UpdatePlayerStats", // Arbitrary function name (must exist in your uploaded cloud.js file)
                FunctionParameter = new { time1 = best_time1, highscore1 = player_highscore1, kills1 = kill1, deaths1 = death1, time2 = best_time2, deaths2 = death2, time3 = best_time3, deaths3 = death3, deaths = total_deaths, tot_time = total_time, online_kills = mult_kills, online_deaths = mult_deaths, kdr = kd, xp = x_p }, // The parameter provided to your function
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
