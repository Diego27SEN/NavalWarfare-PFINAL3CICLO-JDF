using UnityEngine;
using UnityEngine.SceneManagement;


public class ScenesController : MonoBehaviour
{
    public void MainPage()
    {
        SceneManager.LoadScene("MAINPAGE");
    }

    public void MenuScene()
    {
        SceneManager.LoadScene("SelectScene");
    }

    public void ModeScene()
    {
        SceneManager.LoadScene("ModeSelector");
    }

    public void Quitgame()
    {
        Application.Quit();
    }
}
