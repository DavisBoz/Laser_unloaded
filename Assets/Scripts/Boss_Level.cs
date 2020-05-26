using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Level : MonoBehaviour
{
    public GameObject player;
    public GameObject mainaudio;
    public GameObject BossObject;

    private AudioSource BossAudio;
    private AudioSource mainsource;

    bool m_IsAtBoss = false;
    bool m_hasAudioPlayed = false;

    private void Start()
    {
        mainsource = mainaudio.GetComponent<AudioSource>();
        BossAudio = BossObject.GetComponent<AudioSource>();
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            m_IsAtBoss = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsAtBoss && !m_hasAudioPlayed)
        {
            playbossaudio(BossAudio);
            m_hasAudioPlayed = true;
        }

    }
    void playbossaudio(AudioSource audio)
    {
        mainsource.mute = !mainsource.mute;
        BossAudio.Play();

    }
}
