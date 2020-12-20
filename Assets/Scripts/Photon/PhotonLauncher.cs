using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using TMPro;

[System.Serializable]
public class ProfileData
{
    public string username;
    public int level;
    public int xp;
    public string color;

    public ProfileData()
    {
        this.username = "";
        this.level = 0;
        this.xp = 0;
        this.color = "";
    }

    public ProfileData(string u, int l, int x, string c)
    {
        this.username = u;
        this.level = l;
        this.xp = x;
        this.color = c;
    }
}

[System.Serializable]
public class MapData
{
    public string name;
    public int scene;
}

public class PhotonLauncher : MonoBehaviourPunCallbacks
{
    #region Variables
    public TMP_InputField roomnameField;
    public TextMeshProUGUI mapValue;
    public TextMeshProUGUI modeValue;
    public Slider maxPlayersSlider;
    public TextMeshProUGUI maxPlayersValue;
    public GameObject selection_menu;
    public GameObject room_list_menu;
    public GameObject create_room_menu;
    public GameObject change_skin_menu;
    public GameObject connect_text;
    public TextMeshProUGUI user;
    public TextMeshProUGUI level;
    public TextMeshProUGUI xp;
    public TextMeshProUGUI kills;
    public TextMeshProUGUI deaths;
    public TextMeshProUGUI kdr;
    public GameObject connection_menu;
    public GameObject button_room;
    public GameObject stats;
    public GameObject pur_buttons;
    public GameObject st_buttons;
    public string mode;
    public bool floaty;
    public bool rolly = true;
    public static ProfileData my_profile = new ProfileData();

    public MapData[] maps;
    public MapData[] race_maps;
    private int currentmap = 0;
    private string[] modes = { "Arena", "Deathmatch", "Race" };

    private List<RoomInfo> room_list;

    #endregion Variables

    #region CallBacks
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        my_profile.username = PlayFabController.PFC.username;
        my_profile.xp = PlayFabController.PFC.exp;
        my_profile.level = (int)PlayFabController.PFC.exp / 10000;
    }

    private void Start()
    {
        Debug.Log("Connecting");
        if(PhotonNetwork.IsConnected)
        {
            connect_text.SetActive(false);
            selection_menu.SetActive(true);
            stats.SetActive(true);
            user.text = my_profile.username;
            level.text = "Lvl " + my_profile.level.ToString();
            xp.text = my_profile.xp.ToString() + " Total XP";
            kills.text = "Kills: " + PlayFabController.PFC.o_kills.ToString();
            deaths.text = "Deaths: " + PlayFabController.PFC.o_deaths.ToString();
            kdr.text = "K/D Ratio: " + (PlayFabController.PFC.killdeath /100).ToString();
        }
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server");
        connect_text.SetActive(false);
        selection_menu.SetActive(true);
        stats.SetActive(true);
        user.text = my_profile.username;
        level.text = "Lvl " + my_profile.level.ToString();
        xp.text = my_profile.xp.ToString() + " Total XP";
        kills.text = "Kills: " + PlayFabController.PFC.o_kills.ToString();
        deaths.text = "Deaths: " + PlayFabController.PFC.o_deaths.ToString();
        kdr.text = "K/D Ratio: " + PlayFabController.PFC.killdeath.ToString();
        PhotonNetwork.JoinLobby();
    }

    public void MainMenu()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("MainMenu");
    }
    #endregion CallBacks

    #region Rooms
    public void Arena()
    {
        GameSettings.GameMode = (GameMode)0;
    }

    public void Deathmatch()
    {
        GameSettings.GameMode = (GameMode)1;
    }

    public void Race()
    {
        GameSettings.GameMode = (GameMode)2;
    }

    //rolly guy
    public void Blue()
    {
        my_profile.color = "BluePlayer";
        return;
    }

    public void White()
    {
        my_profile.color = "TitaniumPlayer";
        return;
    }

    public void Red()
    {
        my_profile.color = "RedPlayer";
        return;
    }

    public void Orange()
    {
        my_profile.color = "OrangePlayer";
        return;
    }

    public void Pink()
    {
        my_profile.color = "PinkPlayer";
        return;
    }

    public void Purple()
    {
        my_profile.color = "PurplePlayer";
        return;
    }
    public void Yellow()
    {
        my_profile.color = "GoldPlayer";
        return;
    }

    public void Green()
    {
        my_profile.color = "GreenPlayer";
        return;
    }

    public void Diamond()
    {
        my_profile.color = "DiamondPlayer";
        return;
    }

    public void Lightning()
    {
        my_profile.color = "LightningPlayer";
        return;
    }

    //floaty guy
    public void BlueF()
    {
        my_profile.color = "BlueFloatGuy";
        return;
    }

    public void WhiteF()
    {
        my_profile.color = "TitaniumFloatGuy";
        return;
    }

    public void RedF()
    {
        my_profile.color = "RedFloatGuy";
        return;
    }

    public void OrangeF()
    {
        my_profile.color = "OrangeFloatGuy";
        return;
    }

    public void PinkF()
    {
        my_profile.color = "PinkFloatGuy";
        return;
    }

    public void PurpleF()
    {
        my_profile.color = "PurpleFloatGuy";
        return;
    }
    public void YellowF()
    {
        my_profile.color = "GoldFloatGuy";
        return;
    }

    public void GreenF()
    {
        my_profile.color = "GreenFloatGuy";
        return;
    }

    public void DiamondF()
    {
        my_profile.color = "DiamondFloatGuy";
        return;
    }

    public void LightningF()
    {
        my_profile.color = "LightningFloatGuy";
        return;
    }

    //characters
    public void Floaty()
    {
        floaty = true;
        rolly = false;
        return;
    }

    public void Rolly()
    {
        rolly = true;
        floaty = false;
        return;
    }

    //Skins
    public void ChangeSkin()
    {
        selection_menu.SetActive(false);
        change_skin_menu.SetActive(true);
        if (rolly)
        {
            st_buttons.SetActive(true);
            pur_buttons.SetActive(false);
        }
        else if (floaty)
        {
            pur_buttons.SetActive(true);
            st_buttons.SetActive(false);
        }
    }

    public void Random()
    {
        Debug.Log("Arena Button Clicked");
        selection_menu.SetActive(false);
        connection_menu.SetActive(true);
        PhotonNetwork.AutomaticallySyncScene = true;
        ExitGames.Client.Photon.Hashtable customPropreties = new ExitGames.Client.Photon.Hashtable();
        customPropreties["mode"] = (int)GameSettings.GameMode;
        PhotonNetwork.JoinRandomRoom(customPropreties, 0);
    }

    public void Create()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = (byte)maxPlayersSlider.value;

        options.CustomRoomPropertiesForLobby = new string[] { "map", "mode" };

        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
        properties.Add("map", currentmap);
        properties.Add("mode", (int)GameSettings.GameMode);
        options.CustomRoomProperties = properties;

        PhotonNetwork.CreateRoom(roomnameField.text, options);
    }

    public void CreateRandom()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 8;

        options.CustomRoomPropertiesForLobby = new string[] { "map", "mode" };

        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
        properties.Add("map", currentmap);
        properties.Add("mode", (int)GameSettings.GameMode);
        options.CustomRoomProperties = properties;

        PhotonNetwork.CreateRoom(PhotonLauncher.my_profile.username, options);
    }

    public void ChangeMap()
    {
        if ((int)GameSettings.GameMode == 2)
        {
            if (++currentmap >= race_maps.Length) currentmap = 0;
            mapValue.text = "MAP: " + race_maps[currentmap].name.ToUpper();
        }

        else
        {
            if (++currentmap >= maps.Length) currentmap = 0;
            mapValue.text = "MAP: " + maps[currentmap].name.ToUpper();
        }
    }

    public void ChangeMode()
    {
        int newMode = (int)GameSettings.GameMode + 1;
        if (newMode >= System.Enum.GetValues(typeof(GameMode)).Length) newMode = 0;
        GameSettings.GameMode = (GameMode)newMode;
        if ((int)GameSettings.GameMode == 2)
        {
            currentmap = 0;
            mapValue.text = "MAP: " + race_maps[currentmap].name.ToUpper();
        }
        else
        {
            mapValue.text = "MAP: " + maps[currentmap].name.ToUpper();
        }
        modeValue.text = "MODE: " + System.Enum.GetName(typeof(GameMode), newMode);
    }

    public void ChangeMaxPlayersSlider(float t_value)
    {
        maxPlayersValue.text = Mathf.RoundToInt(t_value).ToString();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined the room");
        StartGame();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        CreateRandom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Create();
    }

    public void OnCancelButtonClicked()
    {
        connection_menu.SetActive(false);
        selection_menu.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
    #endregion Rooms

    #region roomList
    public void TabCloseAll()
    {
        selection_menu.SetActive(false);
        room_list_menu.SetActive(false);
        create_room_menu.SetActive(false);
    }

    public void TabOpenArena()
    {
        TabCloseAll();
        selection_menu.SetActive(true);
    }

    public void TabOpenRooms()
    {
        TabCloseAll();
        room_list_menu.SetActive(true);
    }

    public void TabOpenCreate()
    {
        TabCloseAll();
        create_room_menu.SetActive(true);

        roomnameField.text = "";

        currentmap = 0;
        mapValue.text = "MAP: " + maps[currentmap].name.ToUpper();

        GameSettings.GameMode = (GameMode)0;
        modeValue.text = "MODE: " + System.Enum.GetName(typeof(GameMode), (GameMode)0);

        maxPlayersSlider.value = maxPlayersSlider.maxValue;
        maxPlayersValue.text = Mathf.RoundToInt(maxPlayersSlider.value).ToString();
    }

    private void ClearRoomList()
    {
        Transform content = room_list_menu.transform.Find("Scroll View/Viewport/Content");
        foreach (Transform a in content) Destroy(a.gameObject);
    }

    public override void OnRoomListUpdate(List<RoomInfo> p_list)
    {
        room_list = p_list;
        ClearRoomList();

        Transform content = room_list_menu.transform.Find("Scroll View/Viewport/Content");

        foreach (RoomInfo a in room_list)
        {
            GameObject newRoomButton = Instantiate(button_room, content) as GameObject;

            newRoomButton.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = a.Name;
            newRoomButton.transform.Find("Players").GetComponent<TextMeshProUGUI>().text = a.PlayerCount + " / " + a.MaxPlayers;

            if (a.CustomProperties.ContainsKey("map"))
                newRoomButton.transform.Find("Map").GetComponent<TextMeshProUGUI>().text = maps[(int)a.CustomProperties["map"]].name;
            else
                newRoomButton.transform.Find("Map").GetComponent<TextMeshProUGUI>().text = "-----";
            if (a.CustomProperties.ContainsKey("mode"))
                newRoomButton.transform.Find("Mode").GetComponent<TextMeshProUGUI>().text = modes[(int)a.CustomProperties["mode"]];
            else
                newRoomButton.transform.Find("Mode").GetComponent<TextMeshProUGUI>().text = "-----";

            newRoomButton.GetComponent<Button>().onClick.AddListener(delegate { JoinRoom(newRoomButton.transform); });
        }

        base.OnRoomListUpdate(room_list);
    }

    public void JoinRoom(Transform p_button)
    {
        string t_roomName = p_button.Find("Name").GetComponent<TextMeshProUGUI>().text;

        RoomInfo roomInfo = null;
        Transform buttonParent = p_button.parent;
        for (int i = 0; i < buttonParent.childCount; i++)
        {
            if (buttonParent.GetChild(i).Equals(p_button))
            {
                roomInfo = room_list[i];
                break;
            }
        }

        if (roomInfo != null)
        {
            LoadGameSettings(roomInfo);
            PhotonNetwork.JoinRoom(t_roomName);
        }
    }

    public void LoadGameSettings(RoomInfo roomInfo)
    {
        GameSettings.GameMode = (GameMode)roomInfo.CustomProperties["mode"];
    }

    public void StartGame()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            if ((int)GameSettings.GameMode == 2)
            {
                PhotonNetwork.LoadLevel(race_maps[currentmap].scene);
            }
            PhotonNetwork.LoadLevel(maps[currentmap].scene);
        }
    }
    #endregion roomList

}
