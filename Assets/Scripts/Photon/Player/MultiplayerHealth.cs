using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon;

public class MultiplayerHealth : MonoBehaviourPun
{
    public int starting_health = 100;                            // The amount of health the player starts the game with.
    public int current_health;                                   // The current health the player has.
    public Slider health_slider;                                 // Reference to the UI's health bar.
    public Image damage_image;                                   // Reference to an image to flash on the screen on being hurt
    public GameObject health_canvas;
    public float flash_speed = 5f;                               // The speed the damage_image will fade at.
    public Color flash_colour = new Color(1f, 0f, 0f, 0.1f);     // The colour the damage_image is set to, to flash.
    private ArenaManager manager;
    private GameObject player;


    bool is_dead;                                                // Whether the player is dead.
    bool damaged;                              // True when the player gets damaged.


    void Awake()
    {
        current_health = starting_health;
        is_dead = false;
    }

    private void Start()
    {
        health_canvas.SetActive(photonView.IsMine);
        if (photonView.IsMine)
        {
            player = transform.root.gameObject;
            manager = GameObject.Find("GameManager").GetComponent<ArenaManager>();
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (damaged && current_health > 10)
            {
                damage_image.color = flash_colour;
            }
            else
            {
                damage_image.color = Color.Lerp(damage_image.color, Color.clear, flash_speed * Time.deltaTime);
            }
            damaged = false;
        }
    }


    public void TakeDamage(int amount)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("takeDamage", RpcTarget.All, amount);
        }
    }

    [PunRPC]
    void takeDamage(int amount)
    {
        damaged = true;
        current_health -= amount;
        health_slider.value = current_health;
        if (current_health <= 0 && !is_dead)
        {
            manager.Spawn();
            PhotonNetwork.Destroy(player);
        }

    }
}

