using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    protected float _force_hit = 8f;
    private void Start()
    {
        StartCoroutine("WairRGDynemic");
    }
    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        StatesCharachter character = collider.GetComponent<StatesCharachter>();
        if (character)
        {
            character.ReceiveDamage(_force_hit);
        }
    }

    IEnumerator WairRGDynemic()
    {  
            yield return new WaitForSeconds(2);
            this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        
    }

}
