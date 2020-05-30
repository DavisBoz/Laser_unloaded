using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayLvl1()
    {
        SceneManager.LoadScene("Lvl 1");
    }

    public void PlayLvl2()
    {
        SceneManager.LoadScene("Lvl 2");
    }

    public void PlayLvl3()
    {
        SceneManager.LoadScene("Lvl 3");
    }

    public void PlayTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
