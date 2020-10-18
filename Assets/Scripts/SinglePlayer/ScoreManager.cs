using UnityEngine;



public class ScoreManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;                      // Reference to the Text component.
    public static int pickups_used;
    public static int score;

    private void Awake()
    {
        pickups_used = 0;
        score = 0;
    }


    void Update()
    {
          text.text = "Score: " + score;
    }

    // Set the displayed text to be the word "Score" followed by the score value
}

