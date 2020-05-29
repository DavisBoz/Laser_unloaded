using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int starting_health = 100;            // The amount of health the enemy starts the game with.
    public int current_health;                   // The current health the enemy has
    public int score_value = 10;                 // The amount added to the player's score when the enemy dies.
    bool is_dead;
    int hit;

    CapsuleCollider capsule_collider;
    Animator anim;                              // Reference to the animator.
    public AudioSource alien_death;


    void Awake()
    {
        // Setting up the references.
        anim = GetComponent<Animator>();
        capsule_collider = GetComponent<CapsuleCollider>();
        // Setting the current health when the enemy first spawns.
        current_health = starting_health;
    }

    public void TakeDamage(int amount)
    {
        if (is_dead)
            return;


        // Reduce the current health by the amount of damage sustained.
        if(hit<=1)
        {
            anim.SetTrigger("Hit");
            hit += 1;
        }
        
       
        current_health -= amount;

        // If the current health is less than or equal to zero...
        if (current_health <= 0)
        {
            // ... the enemy is dead.
            Death();
        }
    }


    void Death()
    {
        is_dead = true;
        // Tell the animator that the enemy is dead.

        alien_death.Play();
        anim.SetTrigger("Dead");
        capsule_collider.isTrigger = true;
        Destroy(gameObject, 1.5f);
        ScoreManager.score += score_value;
    }

}


