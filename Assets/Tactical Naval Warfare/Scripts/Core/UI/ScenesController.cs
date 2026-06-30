using UnityEngine;
using UnityEngine.SceneManagement;


public class ScenesController : MonoBehaviour
{
    public void MainPage()
    {
        SceneManager.LoadScene("OpeningMainScene");
    }

    public void MenuScene()
    {
        SceneManager.LoadScene("SelectScene");
    }

    public void SelectorScene()
    {
        SceneManager.LoadScene("SelectorScene");
    }

    public void Map1()
    {
        SceneManager.LoadScene("TestCinematic");
    }

    public void ModeScene()
    {
        SceneManager.LoadScene("ModeSelector");
    }

    public void CreditsScene()
    {
        SceneManager.LoadScene("CreditsScene");
    }

    public void Winning()
    {
        SceneManager.LoadScene("WinnerScene");
    }

    public void Quitgame()
    {
        Application.Quit();
    }
}
