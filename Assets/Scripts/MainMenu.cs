using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    [Header("Tags")]
    [SerializeField] private string settingsTag = "";
    [SerializeField] private string MainMenuTag = "";
    [SerializeField] private string LevelMenuTag = "";
    [SerializeField] private string setDifficultTag = "";


    [Header("Canvases")]
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private Canvas setCanvas;
    [SerializeField] private Canvas levelCanvas;
    [SerializeField] private Canvas setDifficult;



    public bool SettingsIsActive;
    public bool RandomChooseIsActive;

    // Start is called before the first frame update
    void Start()
    {
        mainCanvas = GameObject.FindGameObjectWithTag(MainMenuTag).GetComponent<Canvas>();
        setCanvas = GameObject.FindGameObjectWithTag(settingsTag).GetComponent<Canvas>();
        levelCanvas = GameObject.FindGameObjectWithTag(LevelMenuTag).GetComponent<Canvas>();
        setDifficult = GameObject.FindGameObjectWithTag(setDifficultTag).GetComponent<Canvas>();

        setCanvas.gameObject.SetActive(false);
        levelCanvas.gameObject.SetActive(false);
        setDifficult.gameObject.SetActive(false);
        SettingsIsActive = false;
        RandomChooseIsActive = false;
    }


    public void Level_i(int i) 
    {
        gameObject.GetComponent<SaveSystem>().SetPrevScene(i-1);
        SceneManager.LoadScene(i);
    }

    public void SetDifficult(int i)
    {
        gameObject.GetComponent<SaveSystem>().SetDifficult(i);
       if(!SettingsIsActive) SceneManager.LoadScene("RandomWorld");

    }
    public void StartRandom()
    {
        gameObject.GetComponent<SaveSystem>().SetPrevScene(0);
        if (gameObject.GetComponent<SaveSystem>().GetDifficult() == -1)
            ChooseDifficult();
        else
            SceneManager.LoadScene("RandomWorld");
    }

    public void ChooseDifficult()
    {
        setDifficult.gameObject.SetActive(true);

        setCanvas.gameObject.SetActive(false);
        mainCanvas.gameObject.SetActive(false);
        RandomChooseIsActive = true;
    }
    public void Settings()
    {
        setCanvas.gameObject.SetActive(true);
        mainCanvas.gameObject.SetActive(false);
        SettingsIsActive = true;
    }

    public void Levels()
    {
        levelCanvas.gameObject.SetActive(true);
        mainCanvas.gameObject.SetActive(false);
       
    }

    public void Cancel()
    {
        if (RandomChooseIsActive)
        {
            setDifficult.gameObject.SetActive(false);
            if (SettingsIsActive)
                setCanvas.gameObject.SetActive(true);
            else
                mainCanvas.gameObject.SetActive(true);
            RandomChooseIsActive = false;
            return;
        }

        setCanvas.gameObject.SetActive(false);
        mainCanvas.gameObject.SetActive(true);
        levelCanvas.gameObject.SetActive(false);

        if (SettingsIsActive)
        {
            FindObjectOfType<SaveSystem>().SetDataVolume(FindObjectOfType<GetVolumeSettings>().Volume);
            SettingsIsActive = false;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ResetSave()
    {
        FindObjectOfType<SaveSystem>().GetComponent<SaveSystem>().ClearSave();
        levelCanvas.GetComponent<LevelsCanvas>().Refresh();
    }



    public void SetAudioVolume(float vol)
    {
        FindObjectOfType<GetVolumeSettings>().SetVolume(vol);
    }

}
