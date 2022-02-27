using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DieCanvas : MonoBehaviour
{

    public void OnEnable()
    {
        if (SceneManager.GetActiveScene().name == "RandomWorld")
        {
            Text _text_score = GameObject.FindGameObjectWithTag("RecordCanvas").GetComponent<Text>();
            _text_score.gameObject.SetActive(true);
            int score = FindObjectOfType<Score>()._Score;
            int record = FindObjectOfType<SaveSystem>().GetRecord();

            if (score > record)
            {
                _text_score.text = "New record: " + score.ToString();
                FindObjectOfType<SaveSystem>().SetRecord(score);
            }
            else
            {
                _text_score.text = "score: " + score.ToString() + "\nrecord: " + record.ToString();
            }

        }
    }
    public void ReStart()
    {
      
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitMainMenu()
    {
        //character.SaveScore();
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
