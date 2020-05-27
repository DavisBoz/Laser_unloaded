
using UnityEngine;

public class ResetLevel : MonoBehaviour
{
    public GameObject player;

    PlayerHealth playerHealth;
    bool m_IsPlayerAtExit;
    

    void Awake()
    {
        // Setting up the references
        playerHealth = player.GetComponent<PlayerHealth>();
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            m_IsPlayerAtExit = true;
        }
    }

    void Update()
    {
        if (m_IsPlayerAtExit)
        {
            playerHealth.TakeDamage(150);
        }
    }

}

