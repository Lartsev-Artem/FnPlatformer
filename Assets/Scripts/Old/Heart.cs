using UnityEngine;
using System.Collections;

public class Heart : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        StatesCharachter character = collider.GetComponent<StatesCharachter>();
        
        if (character)
        {
            if (character.Lives < 5)
            {
                character.Lives++;
                Destroy(gameObject);
            }
        }
    }
}
