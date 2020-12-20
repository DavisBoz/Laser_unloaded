using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;


public class Pause : MonoBehaviour
{
    public static bool paused = false;
    public Image crosshair;
    public TMPro.TextMeshProUGUI mode;
    private bool disconnecting = false;

    private void Start()
    {
        if (GameSettings.GameMode == GameMode.ARENA) mode.text = "Arena Mode";
        else if (GameSettings.GameMode == GameMode.DEATHMATCH) mode.text = "Deathmatch";
        else if(GameSettings.GameMode == GameMode.RACE) mode.text = "Race Mode";
    }

    public void TogglePause()
    {
        if (disconnecting) return;
        paused = !paused;
        transform.GetChild(0).gameObject.SetActive(paused);
        crosshair.gameObject.SetActive(!paused);

    }

    public void Quit()
    {
        disconnecting = true;
        //PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("MainMenu");
    }
}
