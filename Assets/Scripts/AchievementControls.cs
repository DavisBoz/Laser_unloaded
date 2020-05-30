using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AchievementControls : MonoBehaviour
{
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
        bool areAllEnemiesDead = EnemyHealth.count == 0;

        if (areAllEnemiesDead)
        {
            text.text = "Man Down ~ Eliminated All Enemies!";
            ach_box.SetActive(true);
            StartCoroutine(ShowAndHide(ach_box, 5.0f)); // 5 seconds
        }
    }

    IEnumerator ShowAndHide(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        text.text = "";
        go.SetActive(false);
    }
}
