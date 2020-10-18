using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class PhotonLobbyArena : MonoBehaviourPunCallbacks
{
    public static PhotonLobbyArena arena;
    RoomInfo[] rooms;
    public GameObject selection_menu;
    public GameObject connect_text;
    public GameObject connection_menu;

    private void Awake()
    {
        arena = this;
    }

    private void Start()
    {
        Debug.Log("Connecting");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server");
        connect_text.SetActive(false);
        selection_menu.SetActive(true);
    }

    public void OnArenaButtonClicked()
    {
        Debug.Log("Arena Button Clicked");
        selection_menu.SetActive(false);
        connection_menu.SetActive(true);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined the room");
        SceneManager.LoadScene("ArenaMode");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        CreateRoom();
    }

    void CreateRoom()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        Debug.Log("trying to create room");
        int randomRoomName = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 10 };
        PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        CreateRoom();
    }

    public void OnCancelButtonClicked()
    {
        connection_menu.SetActive(false);
        selection_menu.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }

}
