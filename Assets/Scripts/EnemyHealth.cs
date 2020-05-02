using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;            // The amount of health the enemy starts the game with.
    public int currentHealth;                   // The current health the enemy has.
    public float sinkSpeed = 2.5f;              // The speed at which the enemy sinks through the floor when dead.
    public int scoreValue = 10;                 // The amount added to the player's score when the enemy dies.                
    int hit;
    bool isDead;

    Animator anim;                              // Reference to the animator.
    


    void Awake()
    {
        // Setting up the references.
        anim = GetComponent<Animator>();

        // Setting the current health when the enemy first spawns.
        currentHealth = startingHealth;
    }


    public void TakeDamage(int amount)
    {
        if (isDead)
            return;


        // Reduce the current health by the amount of damage sustained.
        if (hit < 2)
        {
            anim.SetTrigger("Hit");
            hit += 1;
        }
        currentHealth -= amount;

        

        // If the current health is less than or equal to zero...
        if (currentHealth <= 0)
        {
            // ... the enemy is dead.
            Death();
        }
    }


    void Death()
    {
        isDead = true;
        // Tell the animator that the enemy is dead.
        anim.SetTrigger("Dead");
        Destroy(gameObject, 2f);
    }

}


