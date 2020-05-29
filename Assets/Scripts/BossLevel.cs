using UnityEngine;

public class BossLevel : MonoBehaviour
{
    public GameObject player;
    public GameObject main_audio;
    public GameObject boss_object;

    private AudioSource boss_audio;
    private AudioSource main_source;

    bool at_boss = false;
    bool audio_played = false;

    private void Start()
    {
        main_source = main_audio.GetComponent<AudioSource>();
        boss_audio = boss_object.GetComponent<AudioSource>();
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            at_boss = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (at_boss && !audio_played)
        {
            PlayBossAudio(boss_audio);
            audio_played = true;
        }

    }
    void PlayBossAudio(AudioSource boss_audio)
    {
        main_source.mute = !main_source.mute;
        boss_audio.Play();

    }
}
