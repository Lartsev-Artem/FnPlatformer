using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsCanvas : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Sprite _sprite_active_star;
    [SerializeField] private Sprite _sprite_empty_star;
    [SerializeField] private SaveSystem _save_system;
    [SerializeField] private GameObject[] _levels;


    [Header("Parametrs")]
    [SerializeField] private int _number_of_levels;
    [SerializeField] private string _button_tag;


    [Header("Data")]
    [SerializeField] private bool[] _is_active_levels;
    [SerializeField] private int[] _number_stars;


    private void Awake()
    {
        //_levels = GameObject.FindGameObjectsWithTag(_button_tag);
        _save_system = FindObjectOfType<SaveSystem>().GetComponent<SaveSystem>();
    }

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        //_save_system = FindObjectOfType<SaveSystem>().GetComponent<SaveSystem>();
       // _levels = GameObject.FindGameObjectsWithTag(_button_tag);
        _save_system.GetDataLevel(ref _number_of_levels, ref _is_active_levels, ref _number_stars);

        for (int i = 0; i < _number_of_levels; i++)
        {
            if (_is_active_levels[i])
            {
                _levels[i].GetComponent<Button>().enabled = true;
                _levels[i].GetComponent<Image>().color = new Color(1f, 1f, 1f);
                Image[] starts = _levels[i].GetComponentsInChildren<Image>();

                for (int j = 0; j < _number_stars[i]; j++)
                {
                    starts[j + 1].sprite = _sprite_active_star;
                }
                for (int j = _number_stars[i]; j < 3; j++)
                {
                    starts[j + 1].sprite = _sprite_empty_star;
                }
            }
            else
            {
                _levels[i].GetComponent<Button>().enabled = false;
                _levels[i].GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f);
                Image[] starts = _levels[i].GetComponentsInChildren<Image>();
                for (int j = _number_stars[i]; j < 3; j++)
                    starts[j + 1].sprite = _sprite_empty_star;

            }
        }

    }

}
