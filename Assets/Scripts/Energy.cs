using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour
{
    [SerializeField]
    private float _move_speed;
    [SerializeField]
    private float _shoot_speed;

    private void Awake()
    {
        _move_speed = 6f;
        _shoot_speed = 0.3f;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        MoveCharacter character_move = collider.GetComponent<MoveCharacter>();

        if (character_move)
        {
            character_move.StartEnergyMove(_move_speed);
            character_move.gameObject.GetComponent<ShootCharacter>().StartEnergyShoot(_shoot_speed);
            Destroy(gameObject);
        }
    }
}
