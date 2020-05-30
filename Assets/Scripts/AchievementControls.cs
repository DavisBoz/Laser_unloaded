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
        bool are_all_enemies_dead = EnemyHealth.count == 0;
        bool ace = ScoreManager.score == 100;
        bool ace_2 = ScoreManager.score == 200;
        bool ace_3 = ScoreManager.score == 300;
        bool all_pow = PlayerControls.count == pickup_ct;

        if (are_all_enemies_dead)
        {
            text.text = "Man Down ~ Eliminated All Enemies!";
            ach_box.SetActive(true);
            StartCoroutine(Disappear(ach_box, 5.0f)); // 5 seconds
        }

        if (ace)
        {
            text.text = "You got to 100 points!";
            ach_box.SetActive(true);
            StartCoroutine(Disappear(ach_box, 5.0f)); // 5 seconds
        }

        if (ace_2)
        {
            text.text = "You got to 200 points!";
            ach_box.SetActive(true);
            StartCoroutine(Disappear(ach_box, 5.0f)); // 5 seconds
        }

        if (ace_3)
        {
            text.text = "You got to 300 points!";
            ach_box.SetActive(true);
            StartCoroutine(Disappear(ach_box, 5.0f)); // 5 seconds
        }

        if (all_pow)
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
