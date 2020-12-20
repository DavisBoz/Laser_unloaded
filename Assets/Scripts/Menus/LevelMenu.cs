using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelMenu : MonoBehaviour
{
    public void PlayLvl1()
    {
        SceneManager.LoadScene("Lvl1");
    }

    public void PlayLvl2()
    {
        SceneManager.LoadScene("Lvl2");
    }

    public void PlayLvl3()
    {
        SceneManager.LoadScene("Lvl3");
    }

    public void PlayTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
