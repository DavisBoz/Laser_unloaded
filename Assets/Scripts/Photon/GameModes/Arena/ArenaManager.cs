using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System.Collections;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using TMPro;

public class PlayerInfo
{
    public ProfileData profile;
    public int actor;
    public short kills;
    public short deaths;
    public bool awayTeam;

    public PlayerInfo(ProfileData p, int a, short k, short d, bool t)
    {
        this.profile = p;
        this.actor = a;
        this.kills = k;
        this.deaths = d;
        this.awayTeam = t;
    }
}

public enum GameState
{
    Waiting = 0,
    Starting = 1,
    Playing = 2,
    Ending = 3
}

public class ArenaManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public float spawn_time;
    public GameObject LobbyCamera;
    public GameObject joystick;
    public GameObject timer_parent;
    public Transform[] spawns;
    public TextMeshProUGUI spawn;
    public TextMeshProUGUI respawn;
    public TextMeshProUGUI ui_timer;
    public GameObject arena_badge;
    public GameObject tdm_badge;
    public Transform arena_leaderboard;
    public Transform tdm_leaderboard;
    public Transform end_arena_leaderboard;
    public Transform end_tdm_leaderboard;
    public TextMeshProUGUI tdm_home_score;
    public TextMeshProUGUI tdm_away_score;
    public GameObject tdm_score;
    public GameObject arena_score;
    public GameObject usertext;
    public TextMeshProUGUI user;
    public TextMeshProUGUI arena_sc;
    public TextMeshProUGUI fst_arena_sc;
    public Button pause;
    public Button leaderb;
    public Button new_m;
    public Image crosshair;
    public Transform ui_leaderboard;
    public Transform ui_endgame;
    public List<PlayerInfo> playerInfo = new List<PlayerInfo>();
    public int my_ind;
    public int matchLength = 30;
    public bool perpetual = false;

    float timer;

    private bool playerAdded;

    private GameState state = GameState.Waiting;
    private Coroutine timerCoroutine;
    private int killcount = 15;
    private int currentMatchTime;

    public enum EventCodes : byte
    {
        NewPlayer,
        UpdatePlayers,
        ChangeStat,
        NewMatch,
        RefreshTimer
    }


    #region CallBacks
    // Start is called before the first frame update
    void Start()
    {
        LobbyCamera.SetActive(true);

        if (GameSettings.GameMode == GameMode.ARENA)
        {
            matchLength = 300;
            killcount = 5;
            arena_badge.SetActive(true);
            arena_score.SetActive(true);
        }
        if (GameSettings.GameMode == GameMode.DEATHMATCH)
        {
            matchLength = 500;
            killcount = 10;
            tdm_badge.SetActive(true);
            tdm_score.SetActive(true);
        }
        ValidateConnection();
        InitializeTimer();
        NewPlayer_S(PhotonLauncher.my_profile);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        spawn.text = "Spawning...";
        if (timer >= spawn_time)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (!playerAdded)
                {
                    playerAdded = true;
                    Spawn();
                }
            }
            timer = 0;
        }
        if (state == GameState.Ending)
        {
            return;
        }
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    #endregion CallBacks

    #region Photon
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        SceneManager.LoadScene("MainMenu");
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code >= 200) return;

        EventCodes e = (EventCodes)photonEvent.Code;
        object[] o = (object[])photonEvent.CustomData;

        switch (e)
        {
            case EventCodes.NewPlayer:
                NewPlayer_R(o);
                break;
            case EventCodes.UpdatePlayers:
                UpdatePlayer_R(o);
                break;
            case EventCodes.ChangeStat:
                ChangeStat_R(o);
                break;
            case EventCodes.NewMatch:
                NewMatch_R();
                break;
            case EventCodes.RefreshTimer:
                RefreshTimer_R(o);
                break;
        }

    }
    #endregion Photon

    #region Methods
    public void OpenLeaderboard()
    {
        if (ui_leaderboard.gameObject.activeSelf) ui_leaderboard.gameObject.SetActive(false);
        else Leaderboard(ui_leaderboard);
    }

    public void StartNewMatch()
    {
        perpetual = true;
    }

    public void Leave()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonNetwork.LeaveRoom();
    }

    private void RefreshMyStats()
    {
        if (playerInfo.Count > my_ind)
        {
            arena_sc.text = playerInfo[my_ind].kills.ToString();
        }
        else
        {
            arena_sc.text = "0";
        }
        if (ui_leaderboard.gameObject.activeSelf) Leaderboard(ui_leaderboard);
    }

    private bool CalculateTeam()
    {
        return PhotonNetwork.CurrentRoom.PlayerCount % 2 == 0;
    }

    private void ValidateConnection()
    {
        if (PhotonNetwork.IsConnected) return;
        SceneManager.LoadScene("MainMenu");
    }

    private void StateCheck()
    {
        if (state == GameState.Ending)
        {
            EndGame();
        }
    }

    private void ArenaScoreSum()
    {
        // set defaults
        short highest = -1;

        // grab next highest player
        foreach (PlayerInfo a in playerInfo)
        {
            if (a.kills > highest)
            {
                highest = a.kills;
            }
        }
        if (highest > 0) fst_arena_sc.text = "1st " + highest.ToString();
        else fst_arena_sc.text = "1st 0";
    }

    private void ScoreSum()
    {
        int away_score = 0;
        int home_score = 0;
        foreach (PlayerInfo a in playerInfo)
        {
            if (a.awayTeam)
            {
                away_score += a.kills;
            }
            else
            {
                home_score += a.kills;
            }
        }
        tdm_away_score.text = away_score.ToString();
        tdm_home_score.text = home_score.ToString();
    }

    private void ScoreCheck()
    {
        bool detectwin = false;
        if (GameSettings.GameMode == GameMode.ARENA)
        {
            foreach (PlayerInfo a in playerInfo)
            {
                if (a.kills >= killcount)
                {
                    detectwin = true;
                    break;
                }
            }
        }
        if (GameSettings.GameMode == GameMode.DEATHMATCH)
        {
            int away_score = 0;
            int home_score = 0;
            foreach (PlayerInfo a in playerInfo)
            {
                if (a.awayTeam)
                {
                    away_score += a.kills;
                }
                else
                {
                    home_score += a.kills;
                }
            }
            if ((away_score >= killcount) || (home_score >= killcount))
            {
                detectwin = true;
            }
        }
        if (detectwin)
        {
            if (PhotonNetwork.IsMasterClient && state != GameState.Ending)
            {
                UpdatePlayer_S((int)GameState.Ending, playerInfo);
            }
        }
    }

    private void EndGame()
    {
        state = GameState.Ending;
        Preserve.multiplayer_kills = playerInfo[my_ind].kills;
        Preserve.multiplayer_deaths = playerInfo[my_ind].deaths;
        Preserve.multiplayer_xp = playerInfo[my_ind].kills * 100;
        PlayFabController.PFC.StartCloudUpdatePlayer();
        PlayFabController.PFC.GetStats();
        // set timer to 0
        if (timerCoroutine != null) StopCoroutine(timerCoroutine);
        currentMatchTime = 0;
        RefreshTimerUI();
        timer_parent.SetActive(false);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.DestroyAll();
        }
        LobbyCamera.SetActive(true);
        ui_endgame.gameObject.SetActive(true);
        Leaderboard(ui_endgame.Find("Leaderboard"));
        if (PhotonNetwork.IsMasterClient) new_m.gameObject.SetActive(true);
        StartCoroutine(End(6f));
    }



    private void Leaderboard(Transform p_lb)
    {
        if (GameSettings.GameMode == GameMode.ARENA)
        {
            if (state == GameState.Ending) p_lb = end_arena_leaderboard;
            else p_lb = arena_leaderboard;

        }
        if (GameSettings.GameMode == GameMode.DEATHMATCH)
        {
            if (state == GameState.Ending) p_lb = end_tdm_leaderboard;
            else p_lb = tdm_leaderboard;
        }

        // clean up
        for (int i = 2; i < p_lb.childCount; i++)
        {
            Destroy(p_lb.GetChild(i).gameObject);
        }

        // set details
        p_lb.Find("Header/Mode").GetComponent<TextMeshProUGUI>().text = System.Enum.GetName(typeof(GameMode), GameSettings.GameMode);
        p_lb.Find("Header/Map").GetComponent<TextMeshProUGUI>().text = SceneManager.GetActiveScene().name;

        // set scores
        if (GameSettings.GameMode == GameMode.DEATHMATCH)
        {
            p_lb.Find("Header/Home").GetComponent<TextMeshProUGUI>().text = "0";
            p_lb.Find("Header/Away").GetComponent<TextMeshProUGUI>().text = "0";
        }

        // cache prefab
        GameObject playercard = p_lb.GetChild(1).gameObject;
        playercard.SetActive(false);

        // sort
        List<PlayerInfo> sorted = SortPlayers(playerInfo);

        // display
        bool t_alternateColors = false;
        foreach (PlayerInfo a in sorted)
        {
            GameObject newcard = Instantiate(playercard, p_lb) as GameObject;

            if (GameSettings.GameMode == GameMode.DEATHMATCH)
            {
                newcard.transform.Find("Home").gameObject.SetActive(!a.awayTeam);
                newcard.transform.Find("Away").gameObject.SetActive(a.awayTeam);
            }

            if (t_alternateColors) newcard.GetComponent<Image>().color = new Color32(0, 0, 0, 180);
            t_alternateColors = !t_alternateColors;

            newcard.transform.Find("Level Value").GetComponent<TextMeshProUGUI>().text = a.profile.level.ToString("00");
            newcard.transform.Find("Username").GetComponent<TextMeshProUGUI>().text = a.profile.username;
            newcard.transform.Find("Score Value").GetComponent<TextMeshProUGUI>().text = (a.kills * 100).ToString();
            newcard.transform.Find("Kills Value").GetComponent<TextMeshProUGUI>().text = a.kills.ToString();
            newcard.transform.Find("Deaths Value").GetComponent<TextMeshProUGUI>().text = a.deaths.ToString();
            double kdr = (double)a.kills / a.deaths;
            kdr = System.Math.Round(kdr, 2);
            newcard.transform.Find("KDR Value").GetComponent<TextMeshProUGUI>().text = kdr.ToString();
            newcard.SetActive(true);
        }

        // activate
        p_lb.gameObject.SetActive(true);
        p_lb.parent.gameObject.SetActive(true);
    }

    private List<PlayerInfo> SortPlayers(List<PlayerInfo> p_info)
    {
        List<PlayerInfo> sorted = new List<PlayerInfo>();

        if (GameSettings.GameMode == GameMode.ARENA)
        {
            while (sorted.Count < p_info.Count)
            {
                // set defaults
                short highest = -1;
                PlayerInfo selection = p_info[0];

                // grab next highest player
                foreach (PlayerInfo a in p_info)
                {
                    if (sorted.Contains(a)) continue;
                    if (a.kills > highest)
                    {
                        selection = a;
                        highest = a.kills;
                    }
                }
                // add player
                sorted.Add(selection);
            }
        }
        if (GameSettings.GameMode == GameMode.DEATHMATCH)
        {
            List<PlayerInfo> homeSorted = new List<PlayerInfo>();
            List<PlayerInfo> awaySorted = new List<PlayerInfo>();

            int homeSize = 0;
            int awaySize = 0;

            foreach (PlayerInfo p in p_info)
            {
                if (p.awayTeam) awaySize++;
                else homeSize++;
            }

            while (homeSorted.Count < homeSize)
            {
                // set defaults
                short highest = -1;
                PlayerInfo selection = p_info[0];

                // grab next highest player
                foreach (PlayerInfo a in p_info)
                {
                    if (a.awayTeam) continue;
                    if (homeSorted.Contains(a)) continue;
                    if (a.kills > highest)
                    {
                        selection = a;
                        highest = a.kills;
                    }
                }

                // add player
                homeSorted.Add(selection);
            }

            while (awaySorted.Count < awaySize)
            {
                // set defaults
                short highest = -1;
                PlayerInfo selection = p_info[0];

                // grab next highest player
                foreach (PlayerInfo a in p_info)
                {
                    if (!a.awayTeam) continue;
                    if (awaySorted.Contains(a)) continue;
                    if (a.kills > highest)
                    {
                        selection = a;
                        highest = a.kills;
                    }
                }

                // add player
                awaySorted.Add(selection);
            }

            sorted.AddRange(homeSorted);
            sorted.AddRange(awaySorted);
        }
        return sorted;
    }

    public void Respawn()
    {
        respawn.gameObject.SetActive(true);
        crosshair.gameObject.SetActive(false);
        LobbyCamera.SetActive(true);
        joystick.SetActive(false);
        StartCoroutine("Respawning");
    }

    public void Spawn()
    {
        Transform t_spawn = spawns[Random.Range(0, spawns.Length)];
        spawn.gameObject.SetActive(false);
        LobbyCamera.SetActive(false);
        joystick.SetActive(true);
        usertext.SetActive(true);
        pause.gameObject.SetActive(true);
        leaderb.gameObject.SetActive(true);
        crosshair.gameObject.SetActive(true);
        timer_parent.SetActive(true);
        if (GameSettings.GameMode == GameMode.DEATHMATCH)
        {
            if (GameSettings.IsAwayTeam)
            {
                playerInfo[my_ind].profile.color = "RedPlayer";
                user.color = new Color(234, 91, 85, 255);
            }
            else
            {
                playerInfo[my_ind].profile.color = "BluePlayer";
            }
        }
        else
        {
            if(playerInfo[my_ind].profile.color == "")
            {
                playerInfo[my_ind].profile.color = "GreenPlayer";
            }
        }
        PhotonNetwork.Instantiate(playerInfo[my_ind].profile.color, t_spawn.position, t_spawn.rotation);
    }

    private void InitializeTimer()
    {
        currentMatchTime = matchLength;
        RefreshTimerUI();

        if (PhotonNetwork.IsMasterClient)
        {
            timerCoroutine = StartCoroutine(Timer());
        }
    }

    private void RefreshTimerUI()
    {
        string minutes = (currentMatchTime / 60).ToString("00");
        string seconds = (currentMatchTime % 60).ToString("00");
        ui_timer.text = $"{minutes}:{seconds}";
    }
    #endregion Methods

    #region Stats
    public void NewPlayer_S(ProfileData p)
    {
        object[] package = new object[8];

        package[0] = p.username;
        package[1] = p.level;
        package[2] = p.xp;
        package[3] = p.color;
        package[4] = PhotonNetwork.LocalPlayer.ActorNumber;
        package[5] = (short)0;
        package[6] = (short)0;
        package[7] = CalculateTeam();

        PhotonNetwork.RaiseEvent(
                (byte)EventCodes.NewPlayer,
                package,
                new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient },
                new SendOptions { Reliability = true });
    }

    public void NewPlayer_R(object[] data)
    {
        PlayerInfo p = new PlayerInfo(
            new ProfileData(
                (string) data[0],
                (int) data[1],
                (int) data[2],
                (string) data[3]
            ),
            (int) data[4],
            (short) data[5],
            (short) data[6],
            (bool) data[7]
        );
        playerInfo.Add(p);
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Player"))
        {
            gameObject.GetComponent<PlayerControllerOnline>().TrySync();
        }


        UpdatePlayer_S((int)state, playerInfo);
    }

    public void UpdatePlayer_S(int state, List<PlayerInfo> info)
    {
        object[] package = new object[info.Count + 1];

        package[0] = state;
        for (int i = 0; i < info.Count; i++)
        {
            object[] piece = new object[8];
            piece[0] = info[i].profile.username;
            piece[1] = info[i].profile.level;
            piece[2] = info[i].profile.xp;
            piece[3] = info[i].profile.color;
            piece[4] = info[i].actor;
            piece[5] = info[i].kills;
            piece[6] = info[i].deaths;
            piece[7] = info[i].awayTeam;

            package[i + 1] = piece;
        }
        PhotonNetwork.RaiseEvent(
            (byte)EventCodes.UpdatePlayers,
            package,
            new RaiseEventOptions { Receivers = ReceiverGroup.All },
            new SendOptions { Reliability = true }
        );   
    }

    public void UpdatePlayer_R(object[] data)
    {
        state = (GameState)data[0];
        if (playerInfo.Count < data.Length - 1)
        {
            foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Player"))
            {
                //if so, resync our local player information
                gameObject.GetComponent<PlayerControllerOnline>().TrySync();
            }
        }


        playerInfo = new List<PlayerInfo>();

        for (int i = 1; i < data.Length; i++)
        {
            object[] extract = (object[])data[i];

            PlayerInfo p = new PlayerInfo(
                new ProfileData(
                    (string)extract[0],
                    (int)extract[1],
                    (int)extract[2],
                    (string) extract[3]
                ),
                (int)extract[4],
                (short)extract[5],
                (short)extract[6],
                (bool)extract[7]
            );

            playerInfo.Add(p);

            if (PhotonNetwork.LocalPlayer.ActorNumber == p.actor)
            {
                my_ind = i - 1;

                //if we have been waiting to be added to the game then spawn us in
                if (!playerAdded)
                {
                    playerAdded = true;
                    GameSettings.IsAwayTeam = p.awayTeam;
                    Spawn();
                }
            }
        }
        StateCheck();
    }

    public void ChangeStat_S(int actor, byte stat, byte amt)
    {
        object[] package = new object[] { actor, stat, amt };

        PhotonNetwork.RaiseEvent(
            (byte)EventCodes.ChangeStat,
            package,
            new RaiseEventOptions { Receivers = ReceiverGroup.All },
            new SendOptions { Reliability = true }
        );
    }

    public void ChangeStat_R(object[] data)
    {
        int actor = (int) data[0];
        byte stat = (byte) data[1];
        byte amt = (byte) data[2];

        for (int i = 0; i < playerInfo.Count; i++)
        {
            if (playerInfo[i].actor == actor)
            {
                switch (stat)
                {
                    case 0:
                        playerInfo[i].kills += amt;
                        Debug.Log($"Player {playerInfo[i].profile.username} : kills = {playerInfo[i].kills}");
                        break;

                    case 1:
                        playerInfo[i].deaths += amt;
                        Debug.Log($"Player {playerInfo[i].profile.username} : deaths = {playerInfo[i].deaths}");
                        break;

                }
                if (i == my_ind) RefreshMyStats();
                if (ui_leaderboard.gameObject.activeSelf) Leaderboard(ui_leaderboard);
                break;
            }
        }
        ScoreCheck();
        if (GameSettings.GameMode == GameMode.DEATHMATCH) ScoreSum();
        if (GameSettings.GameMode == GameMode.ARENA) ArenaScoreSum();
    }

    public void NewMatch_S()
    {
        PhotonNetwork.RaiseEvent(
            (byte)EventCodes.NewMatch,
            null,
            new RaiseEventOptions { Receivers = ReceiverGroup.All },
            new SendOptions { Reliability = true });
    }

    public void NewMatch_R()
    {
        state = GameState.Waiting;
        LobbyCamera.SetActive(false);
        new_m.gameObject.SetActive(false);
        ui_endgame.gameObject.SetActive(false);
        foreach (PlayerInfo p in playerInfo)
        {
            p.kills = 0;
            p.deaths = 0;
        }
        RefreshMyStats();
        if (GameSettings.GameMode == GameMode.DEATHMATCH) ScoreSum();
        if (GameSettings.GameMode == GameMode.ARENA) ArenaScoreSum();
        Spawn();
    }

    public void RefreshTimer_S()
    {
        object[] package = new object[] { currentMatchTime };

        PhotonNetwork.RaiseEvent(
            (byte)EventCodes.RefreshTimer,
            package,
            new RaiseEventOptions { Receivers = ReceiverGroup.All },
            new SendOptions { Reliability = true }
        );
    }
    public void RefreshTimer_R(object[] data)
    {
        currentMatchTime = (int)data[0];
        RefreshTimerUI();
    }
    #endregion Stats

    #region CoRoutines
    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(1f);

        currentMatchTime -= 1;

        if (currentMatchTime <= 0)
        {
            timerCoroutine = null;
            UpdatePlayer_S((int)GameState.Ending, playerInfo);
        }
        else
        {
            RefreshTimer_S();
            timerCoroutine = StartCoroutine(Timer());
        }
    }

    private IEnumerator End(float p_wait)
    {
        yield return new WaitForSeconds(p_wait);
        new_m.gameObject.SetActive(false);
        if (perpetual)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                NewMatch_S();
            }
        }
        else
        {

            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.AutomaticallySyncScene = false;
            PhotonNetwork.LeaveRoom();
        }
    }

    IEnumerator Respawning()
    {
        yield return new WaitForSeconds(3f);
        respawn.gameObject.SetActive(false);
        Spawn();
    }
    #endregion CoRoutines
}