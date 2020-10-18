using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMainMenu : MonoBehaviour
{

    public void PlayMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
