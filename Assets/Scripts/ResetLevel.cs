using UnityEngine;

public class ResetLevel : MonoBehaviour
{
    public GameObject player;

    PlayerHealth player_health;
    bool at_exit;
    

    void Awake()
    {
        // Setting up the references
        player_health = player.GetComponent<PlayerHealth>();
        
    }

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
            player_health.TakeDamage(150);
        }
    }

}

