using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootCharacter : MonoBehaviour
{

    private Bullet _bullet;
    public Sprite  _sprite_bullet;
    public Sprite  _uber_sprite_bullet;
    public Transform _gun_point;

    public bool isUber = false; // Есть ли спец снаряд
    public int KillCounter = 0;
    public int UberCount = 0;


    private bool isShoot = false;
    private float speed_Shoot = 0.6f;
    private float cur_speed_shoot;
    private float _time_buf;
    public float Speed_shoot
    {
        get { return cur_speed_shoot; }
        set { cur_speed_shoot = value; }
    }

    private MoveCharacter moveCharacter;

    private void Awake()
    {
        moveCharacter = GetComponent<MoveCharacter>();
        _bullet = Resources.Load<Bullet>("Bullet");

        _time_buf = 10f;
        cur_speed_shoot = speed_Shoot;
    }

    private void Update()
    {

#if UNITY_ANDROID

#else
      if (Input.GetButtonDown("Fire1") && !isShoot) { isShoot = true; moveCharacter._animator.SetBool("Shoot", true); /*Invoke("Shoot", Speed_shoot / 2); */Shoot();
            StartCoroutine("WaitShoot"); }
#endif

        // if (Input.GetKey(KeyCode.LeftShift) && UberCount > 0) { isUber = true; }

    }
    public void ShootAndroid()
    {
        if (!isShoot)
        {
            isShoot = true;
            moveCharacter._animator.SetBool("Shoot", true);
            Shoot();
            StartCoroutine("WaitShoot");
        } 
    }

    private void Shoot()
    {
        // State = CharState.Shoot;

        Bullet newBullet;
        Vector3 position = _gun_point.position;// transform.position; position.y += 0.4F;

        if (!isUber || UberCount == 0)
        {
            _bullet.GetComponentInChildren<SpriteRenderer>().sprite = _sprite_bullet;
            newBullet = Instantiate(_bullet, position, _bullet.transform.rotation) as Bullet;
        }
        else
        {
            _bullet.GetComponentInChildren<SpriteRenderer>().sprite = _uber_sprite_bullet;
            newBullet = Instantiate(_bullet, position, _bullet.transform.rotation) as Bullet;
            newBullet.isUber = true;
            if (UberCount > 0)
            {
                KillCounter -= 5;
                UberCount--;
            }
            isUber = false;
            //     superShot.GetComponent<Text>().color = Color.black;
        }

        newBullet.Parent = gameObject;
        newBullet.Direction = newBullet.transform.right * (moveCharacter.FacingRight ? 1.0F : -1.0F);
    }

    IEnumerator WaitShoot()
    {
        yield return new WaitForSeconds(0.01f);
        moveCharacter._animator.SetBool("Shoot", false);

        yield return new WaitForSeconds(Speed_shoot);
        isShoot = false;
    }


    private IEnumerator StartEnergy(float newspeed)
    {
        Speed_shoot = newspeed;
        yield return new WaitForSeconds(_time_buf);
        Speed_shoot = speed_Shoot;
    }
    public void StartEnergyShoot(float speed)
    {
        StartCoroutine("StartEnergy", speed);
    }

}
