using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class ScoreManager : MonoBehaviour
{
    public static int score;        // The player's score.
    Text text;                      // Reference to the Text component.
    private Scene scene;


    void Awake()
    {
        // Get current script
        scene = SceneManager.GetActiveScene();
        // Set up the reference.
        text = GetComponent<Text>();

        // Reset the score.
        score = 0;
    }


    void Update()
    {
        //text.text = "Score: " + score;
        // Set the displayed text to be the word "Score" followed by the score value.
        if (scene.name != "Tutorial")
        {
            text.text = "Score: " + score;
        }
        else if (score != 0)
        {
            text.text = "Score:" + score;
        }
    }
}
