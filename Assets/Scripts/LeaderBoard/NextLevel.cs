using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public void PlayLvl2()
    {
        SceneManager.LoadScene("Lvl2");
    }

    public void PlayLvl3()
    {
        SceneManager.LoadScene("Lvl3");
    }

    public void PlayTotals()
    {
        SceneManager.LoadScene("TotalBoard");
    }

    public void PlayMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
