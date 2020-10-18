using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    public static bool game_is_paused;
    public GameObject pause_menu;

    public void Resume()
    {
        pause_menu.SetActive(false);
        Time.timeScale = 1f;
        game_is_paused = false;
    }
    public void Pause()
    {
        pause_menu.SetActive(true);
        Time.timeScale = 0f;
        game_is_paused = true;
    }

    public void LoadMenu()

    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}

