
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetLevel : MonoBehaviour
{
    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;
    public GameObject player;
    public CanvasGroup ResetBackgroundImageCanvasGroup;
    public AudioSource resetAudio;

    bool m_IsPlayerAtExit;
    float m_Timer;
    bool m_hasAudioPlayed;

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
            FailLevel(ResetBackgroundImageCanvasGroup, resetAudio);
        }
    }

    void FailLevel(CanvasGroup imageCanvasGroup, AudioSource audioSource)
    {
        if (!m_hasAudioPlayed)
        {
            audioSource.Play();
            m_hasAudioPlayed = true;
        }

        m_Timer += Time.deltaTime;

        imageCanvasGroup.alpha = m_Timer / fadeDuration;

        if (m_Timer > fadeDuration + displayImageDuration)
        {
            SceneManager.LoadScene("POC");
        }
    }
}

