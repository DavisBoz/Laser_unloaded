using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AchievementControls : MonoBehaviour
{
    public int pickup_ct;
    public GameObject ach_box;
    public AudioSource ach;
    bool ready1 = true;
    bool ready2 = true;
    bool ready3 = true;
    bool ready4 = true;
    bool ready5 = true;

    Text text;


    void Awake()
    {
        text = GetComponent<Text>();
    }

    void Start()
    {
        ach_box.SetActive(false);
    }

    void Update()
    {

        if ((EnemyHealth.count == 0) && ready1)
        {
            ready1 = false;
            ManDown();
        }

        if ((ScoreManager.score >= 100) && ready2)
        {
            ready2 = false;
            Ace();
        }

        if ((ScoreManager.score >= 200) && ready3)
        {
            ready3 = false;
            MadMan();
        }

        if ((ScoreManager.score >= 300) && ready4)
        {
            ready4 = false;
            Mastery();
        }

        if ((ScoreManager.pickups_used == pickup_ct) && ready5)
        {
            ready5 = false;
            Juice();
        }
    }

    void ManDown()
    {
        ach.Play();
        text.text = "Man Down ~ Eliminated All Enemies!";
        ach_box.SetActive(true);
        Invoke(nameof(Disappear), 4);
    }

    void Ace()
    {
        ach.Play();
        text.text = "Ace ~ You got to 100 points!";
        ach_box.SetActive(true);
        Invoke(nameof(Disappear), 4);
    }

    void MadMan()
    {
        ach.Play();
        text.text = "Mad Man ~  You got to 200 points!";
        ach_box.SetActive(true);
        Invoke(nameof(Disappear), 4);
    }

    void Mastery()
    {
        ach.Play();
        text.text = "Mastery ~ You got to 300 points!";
        ach_box.SetActive(true);
        Invoke(nameof(Disappear), 4);
    }

    void Juice()
    {
        ach.Play();
        text.text = "Juice Acquired ~ All Pick Ups Used!";
        ach_box.SetActive(true);
        Invoke(nameof(Disappear), 4);
    }

    void Disappear()
    {
        ach_box.SetActive(false);
        text.text = "";
    }
}
