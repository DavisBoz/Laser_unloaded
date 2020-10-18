using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int starting_health = 150;            // The amount of health the enemy starts the game with.
    public int current_health;                   // The current health the enemy has
    public int score_value = 10;                 // The amount added to the player's score when the enemy dies.
    public static int count;                         // The number of enemies in the scene
    bool is_dead;
    int hit;

    CapsuleCollider capsule_collider;
    SphereCollider sphere_collider;
    Animator anim;                              // Reference to the animator.
    public AudioSource alien_death;


    void Awake()
    {
        // Setting up the references.
        anim = GetComponent<Animator>();
        capsule_collider = GetComponent<CapsuleCollider>();
        sphere_collider = GetComponent<SphereCollider>();
        // Setting the current health when the enemy first spawns.
        current_health = starting_health;
        count += 1;
    }

    public void TakeDamage(int amount)
    {
        if (is_dead)
            return;


        // Reduce the current health by the amount of damage sustained.
        if(hit<1)
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
        // Tell the animator that the enemy is dead.
        capsule_collider.enabled = false;
        sphere_collider.enabled = false;
        count -= 1;
        is_dead = true;
        alien_death.Play();
        anim.SetTrigger("Dead");
        Destroy(gameObject, 1.5f);
        ScoreManager.score += score_value;
        Preserve.kills += 1;
    }

}


