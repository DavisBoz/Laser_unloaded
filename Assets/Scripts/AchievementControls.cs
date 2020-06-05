using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AchievementControls : MonoBehaviour
{
    public int pickup_ct;
    public GameObject ach_box;

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
        
        if (EnemyHealth.count == 0)
        {
            text.text = "Man Down ~ Eliminated All Enemies!";
            ach_box.SetActive(true);
            StartCoroutine(Disappear(ach_box, 5.0f)); // 5 seconds
        }

        if (ScoreManager.score >= 100)
        {
            text.text = "Ace ~ You got to 100 points!";
            ach_box.SetActive(true);
            StartCoroutine(Disappear(ach_box, 5.0f)); // 5 seconds
        }

        if (ScoreManager.score >= 200)
        {
            text.text = "Mad Man ~  You got to 200 points!";
            ach_box.SetActive(true);
            StartCoroutine(Disappear(ach_box, 5.0f)); // 5 seconds
        }

        if (ScoreManager.score >= 300)
        {
            text.text = "Mastery ~ You got to 300 points!";
            ach_box.SetActive(true);
            StartCoroutine(Disappear(ach_box, 5.0f)); // 5 seconds
        }

        if (PlayerControls.count == pickup_ct)
        {
            text.text = "Juice Acquired ~ All Pick Ups Used!";
            ach_box.SetActive(true);
            StartCoroutine(Disappear(ach_box, 5.0f)); // 5 seconds
        }
    }

    IEnumerator Disappear(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        text.text = "";
        go.SetActive(false);
    }
}
