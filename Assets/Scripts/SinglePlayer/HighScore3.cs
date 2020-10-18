using UnityEngine;

public class HighScore3 : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    // Start is called before the first frame update
    private void Awake()
    {
        text.text = "High Score: " + PlayFabController.highscore3;
    }
}
