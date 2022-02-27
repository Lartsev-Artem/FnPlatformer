using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDie : MonoBehaviour
{
    private Transform character;
    [SerializeField] private float _time_wait;
    [SerializeField] private float _time_speed_fall;
    private void Awake()
    {
        _time_wait = 1f;
        _time_speed_fall = 0.1f;
        character = FindObjectOfType<StatesCharachter>().transform;
    }

    private void FixedUpdate()
    {
        if (character.position.x > transform.position.x + 5)
        {
            StartCoroutine("Die");
        }
    }

    private IEnumerator Die()
    {
        Rigidbody2D rg = GetComponent<Rigidbody2D>();
        yield return new WaitForSeconds(_time_wait);
        rg.mass = 0;
        rg.bodyType = RigidbodyType2D.Dynamic;
        while (true)
        {
            rg.mass += 0.01f;
            yield return new WaitForSeconds(_time_speed_fall);
            
        }
    }
}
