using UnityEngine;

public class Timer : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    public static int ticker;
    private float start_time;
    private bool finished = false;
    // Start is called before the first frame update
    void Start()
    {
        start_time = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (finished)
            return;
        float t = Time.time - start_time;
        int mins = ((int)t / 60);
        float secs = (t % 60);
        double sec = System.Math.Round(secs, 2);
        float s = (float)sec;
        string minutes = mins.ToString();
        string seconds = secs.ToString("f2");
        string clock = minutes + ":" + seconds;
        text.text = "Time: " + clock;
        float tick = (mins + s) * 100;
        ticker = (int)tick;
    }

    public void Finish()
    {
        finished = true;
    }
}
