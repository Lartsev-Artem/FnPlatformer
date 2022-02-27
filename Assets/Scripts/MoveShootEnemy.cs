using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveShootEnemy : MoveEnemy
{
    private Bullet _bullet;
    [SerializeField]
    private  Sprite _sprite_bullet;
    [SerializeField]
    private Transform _gun_point;
    [SerializeField]
    private float _time_shoot = 2.0f;



    private void Awake()
    {
        _bullet = Resources.Load<Bullet>("Bullet");
      
     //   StartCoroutine("ShootEnemy");

    }

    private void Shoot()
    {
        _bullet.GetComponentInChildren<SpriteRenderer>().sprite = _sprite_bullet;
        Bullet newBullet = Instantiate(_bullet, _gun_point.position, _bullet.transform.rotation) as Bullet;
        newBullet.Parent = gameObject;
        newBullet.Direction = newBullet.transform.right * (this.Facing_right ? 1.0F : -1.0F);
    }


    IEnumerator ShootEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(_time_shoot);
            Shoot();
        }
    }
} 
