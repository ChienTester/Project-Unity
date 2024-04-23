using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    [SerializeField] GameObject MenuWindow;
    public static bool isMenuWindow = false;
    public void ToStartScreen()
    {

        SceneManager.LoadScene("Start");
        Time.timeScale = 1f;
        PauseMenu.isPaused = false;
    }

    public void ToGameScreen()
    {
        SceneManager.LoadScene("tesst");
        Time.timeScale = 0f;
    }


    public void Pause()
    {
        PauseMenu.isPaused = true;
    }

    public void Resume()
    {
        PauseMenu.isPaused = false;
    }
    public void Retry()
    {
        SceneManager.LoadScene("tesst");
        Time.timeScale = 1f;
        PauseMenu.isPaused = false;
    }
    public void Menu()
    {
        MenuWindow.SetActive(true);

    }
    public void ExitMenu()
    {
        MenuWindow.SetActive(false);

    }
}
