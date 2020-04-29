using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupDouble : MonoBehaviour
{
    ParticleSystem m_particlesystem;
    // Start is called before the first frame update
    void Start()
    {
        m_particlesystem = gameObject.GetComponent<ParticleSystem>();
        m_particlesystem.Play();
    }
}
