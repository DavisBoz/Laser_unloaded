using UnityEngine;
using UnityEngine.SceneManagement;

public class WonLevel : MonoBehaviour
{
    public float fade_duration = 1f;
    public float image_duration = 1f;
    public GameObject player;
    public CanvasGroup won_background_image_canvas;
    public AudioSource won_audio;

    bool at_exit;
    float timer;
    bool audio_played;

    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            player.SendMessage("Finish");
            at_exit = true;
        }
    }

    void Update()
    {
        if (at_exit)
        {
            WinLevel(won_background_image_canvas, won_audio);
        }
    }

    public void SavePlayer1()
    {
        Preserve.total_score += ScoreManager.score;
        Preserve.deaths1 = -1 * Preserve.deaths;
        Preserve.kills1 = Preserve.kills;
        Preserve.total_deaths += Preserve.deaths;
        Preserve.player_highscore1 = ScoreManager.score;
        Preserve.time1 = -1 * Timer.ticker;
        Preserve.kills = 0;
    }

    public void SavePlayer2()
    {
        Preserve.total_score += ScoreManager.score;
        Preserve.kills2 = Preserve.kills;
        Preserve.deaths2 = -1 * Preserve.deaths;
        Preserve.total_deaths += Preserve.deaths;
        Preserve.player_highscore2 = ScoreManager.score;
        Preserve.time2 = -1 * Timer.ticker;
        Preserve.kills = 0;
    }

    public void SavePlayer3()
    {
        Preserve.total_score += ScoreManager.score;
        Preserve.kills3 = Preserve.kills;
        Preserve.total_kills = Preserve.kills1 + Preserve.kills2 + Preserve.kills3;
        Preserve.deaths3 = -1 * Preserve.deaths;
        Preserve.total_deaths += Preserve.deaths;
        Preserve.total_deaths = Preserve.total_deaths * -1;
        Preserve.player_highscore3 = ScoreManager.score;
        Preserve.time3 = -1 * Timer.ticker;
        Preserve.kills = 0;
    }

    void WinLevel(CanvasGroup image_canvas, AudioSource audio_source)
    {
        if (!audio_played)
        {
            audio_source.Play();
            audio_played = true;
        }

        timer += Time.deltaTime;

        image_canvas.alpha = timer / fade_duration;

        if (timer > fade_duration + image_duration)
        {
            Scene scene  = SceneManager.GetActiveScene();
            if (scene.name == "Tutorial")
            {
                SceneManager.LoadScene("Lvl1");
            }

            else if (scene.name == "Lvl1")
            {
                SavePlayer1();
                PlayFabController.PFC.StartCloudUpdatePlayer();
                Preserve.deaths = 0;
                SceneManager.LoadScene("Lvl1Board");
            }
            else if (scene.name == "Lvl2")
            {
                SavePlayer2();
                PlayFabController.PFC.StartCloudUpdatePlayer();
                Preserve.deaths = 0;
                SceneManager.LoadScene("Lvl2Board");
            }
            else if (scene.name == "Lvl3")
            {
                SavePlayer3();
                PlayFabController.PFC.StartCloudUpdatePlayer();
                Preserve.deaths = 0;
                SceneManager.LoadScene("Lvl3Board");
            }
        }
    }
}

