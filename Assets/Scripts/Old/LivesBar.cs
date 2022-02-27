using UnityEngine;
using System.Collections;

public class LivesBar : MonoBehaviour
{
    [SerializeField]
    private Transform[] hearts;
    private int _max_size = 5;
    private StatesCharachter character;


    private void Awake()
    {
        character = FindObjectOfType<StatesCharachter>();

        hearts = new Transform[_max_size];
        for (int i = 0; i < _max_size; i++)
        {
            hearts[i] = transform.GetChild(i);
        }
    }

    public void Refresh()
    {
        for (int i = 0; i < _max_size; i++)
        {
            if (i < character.Lives) hearts[i].gameObject.SetActive(true);
            else hearts[i].gameObject.SetActive(false);
        }
    }
}
