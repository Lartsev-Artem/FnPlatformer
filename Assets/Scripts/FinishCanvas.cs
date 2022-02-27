using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishCanvas : MonoBehaviour
{
    public int _number_next_scene;
    public void Next()
    {
        FindObjectOfType<SaveSystem>().SetPrevScene(_number_next_scene);
        Time.timeScale = 1;
        SceneManager.LoadScene(_number_next_scene+1);
    }

    public void ExitMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
