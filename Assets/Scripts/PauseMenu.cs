using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private MenuControll _menu_controll;

    private void Start()
    {
        _menu_controll = FindObjectOfType<MenuControll>();
    }


    public void Continue()
    {
        _menu_controll.ContinueOrPause();
    }

    public void ExitMainMenu()
    {
        //character.SaveScore();
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
       // character.SaveScore();
        Application.Quit();
    }
}
