using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float fade_duration = 1f;
    public float image_duration = 1f;
    public CanvasGroup reset_image;
    public AudioSource reset_audio;
    public int starting_health = 100;                            // The amount of health the player starts the game with.
    public int current_health;                                   // The current health the player has.
    public Slider health_slider;                                 // Reference to the UI's health bar.
    public Image damage_image;                                   // Reference to an image to flash on the screen on being hurt
    public float flash_speed = 5f;                               // The speed the damage_image will fade at.
    public Color flash_colour = new Color(1f, 0f, 0f, 0.1f);     // The colour the damage_image is set to, to flash.
    public string load_this;


    bool is_dead;                                                // Whether the player is dead.
    bool damaged;
    float timer;
    bool audio_played;                              // True when the player gets damaged.


    void Awake()
    {

        // playerShooting = GetComponentInChildren<PlayerShooting>();

        // Set the initial health of the player.
        current_health = starting_health;
    }


    void Update()
    {
        if (is_dead)
            FailLevel(reset_image, reset_audio);

        // If the player has just been damaged...
        if (damaged)
        {
            // ... set the colour of the damageImage to the flash colour.
            damage_image.color = flash_colour;
        }
        // Otherwise...
        else
        {
            // ... transition the colour back to clear.
            damage_image.color = Color.Lerp(damage_image.color, Color.clear, flash_speed * Time.deltaTime);
        }

        // Reset the damaged flag.
        damaged = false;
    }


    public void TakeDamage(int amount)
    {
        // Set the damaged flag so the screen will flash.
        damaged = true;

        // Reduce the current health by the damage amount.
        current_health -= amount;

        // Set the health bar's value to the current health.
        health_slider.value = current_health;



        // If the player has lost all it's health and the death flag hasn't been set yet...
        if (current_health <= 0 && !is_dead)
        {
            // ... it should die.
            is_dead = true;
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
            SceneManager.LoadScene(load_this);
        }

    }
}

