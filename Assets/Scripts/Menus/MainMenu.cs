using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void PlayTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
