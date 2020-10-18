using UnityEngine;
using Photon.Pun;



public class OnlineScoreManage : MonoBehaviourPun
{
    public TMPro.TextMeshProUGUI text;                      // Reference to the Text component.
    public static int pickups_used;
    public static int score;

    private void Awake()
    {
        score = 0;
    }


    void Update()
    {
        if (photonView.IsMine)
        {
            text.text = "Score: " + score;
        }
    }

    // Set the displayed text to be the word "Score" followed by the score value
}

