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
            at_exit = true;
        }
    }

    void Update()
    {
        if (at_exit)
        {
            FailLevel(won_background_image_canvas, won_audio);
        }
    }

    void FailLevel(CanvasGroup image_canvas, AudioSource audio_source)
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
                SceneManager.LoadScene("Lvl 1");
            }

            else if (scene.name == "Lvl 1")
            {
                SceneManager.LoadScene("Lvl 2");
            }
            else
                Application.Quit();
        }
    }
}

