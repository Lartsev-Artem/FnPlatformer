using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    private SaveSystem save_level;
    MenuControll menu;
    private void Start()
    {
         menu = FindObjectOfType<MenuControll>();
        save_level = menu.gameObject.GetComponent<SaveSystem>();
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        StatesCharachter charachter = collider.GetComponent<StatesCharachter>();
        if (charachter)
        {
            if (!charachter.Was_damage) save_level.SaveCurResult(3);
            else if (charachter.Lives == 5) save_level.SaveCurResult(2);
            else if (charachter.Lives >= 3) save_level.SaveCurResult(1);
            else save_level.SaveCurResult(0);

            if (save_level.GetPrevScene() < save_level.Number_of_levels-1)
                menu.StartFinish(save_level.GetPrevScene()+1);
            else menu.StartFinish(-1);
        }
    }
}
