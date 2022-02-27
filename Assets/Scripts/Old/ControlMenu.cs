using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMenu : MonoBehaviour
{
    Canvas[] canvas; 
    void Start()
    {
        canvas = GetComponentsInChildren<Canvas>();
        canvas[1].gameObject.SetActive(false);
    }

    public void SetSettings()
    {
        canvas[0].gameObject.SetActive(false);
        canvas[1].gameObject.SetActive(true);
    }
    public void SetMain()
    {
        canvas[0].gameObject.SetActive(true);
        canvas[1].gameObject.SetActive(false);
    }
}
