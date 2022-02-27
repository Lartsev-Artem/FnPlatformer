using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticDieSpace : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        StatesCharachter character = other.GetComponent<StatesCharachter>();
        if (character)
        {
            Time.timeScale = 0f;
            FindObjectOfType<MenuControll>().StartDie();
        }
        else Destroy(other.gameObject);
    }
}
