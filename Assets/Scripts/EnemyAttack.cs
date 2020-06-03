using UnityEngine;
using System.Collections;



public class EnemyAttack : MonoBehaviour
{
    public float attack_interval = 0.5f;     // The time in seconds between each attack.
    public int attack_damage = 10;               // The amount of health taken away per attack.
    public AudioSource Smack;
    public AudioSource Growl;
    public GameObject player;                    

    Animator anim;                              // Reference to the animator component.
    
    PlayerHealth player_health;                  // Reference to the player's health.
    EnemyHealth enemy_health;                    // Reference to this enemy's health.
    bool player_in_range;                         // Whether player is within the trigger collider and can be attacked.
    float timer;                                // Timer for counting up to the next attack.


    void Awake()
    {
        // Setting up the references
        player_health = player.GetComponent<PlayerHealth>();
        enemy_health = GetComponent<EnemyHealth>();
        anim = GetComponent<Animator>();
    }


    void OnTriggerEnter(Collider other)
    {
        // If the entering collider is the player...
        if (other.gameObject == player)
        {
            // ... the player is in range.
            Growl.Play();
            player_in_range = true;
        }
    }


    void OnTriggerExit(Collider other)
    {
        // If the exiting collider is the player...
        if (other.gameObject == player)
        {
            // ... the player is no longer in range.
            player_in_range = false;
            anim.SetTrigger("Idle");
        }
    }


    void Update()
    {
        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;

        // If the timer exceeds the time between attacks, the player is in range and this enemy is alive...
        if (timer >= attack_interval && player_in_range && enemy_health.current_health > 0)
        {
            // ... attack.
            Attack();
        }

    }


    void Attack()
    {
        // Reset the timer.
        timer = 0f;

        // If the player has health to lose...
        if (player_health.current_health > 0)
        {
            // ... damage the player.
            Smack.Play();
            anim.SetTrigger("Attack");
            player_health.TakeDamage(attack_damage);
            anim.SetTrigger("Idle");
        }
    }
}
