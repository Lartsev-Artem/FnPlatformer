using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatesCharachter : MonoBehaviour
{
    private LivesBar _live_bar;
    private Shield shield;
    [SerializeField]
    private MoveCharacter _move_character;

    public bool _shield_active;
    [SerializeField]
    private int lives = 5;
    [SerializeField]
    private float _wait_damage_time;
    private bool _wait_damage;
    

    public bool _energy_buf;

    public int Lives
    {
        get { return lives; }
        set { if (value <= 5) { lives = value; _live_bar.Refresh(); } }
    }

    [SerializeField] private bool _was_damage;
    public bool Was_damage { get { return _was_damage; } }

    private void Awake()
    {
        _was_damage = false;
        _energy_buf = false;

        _live_bar = FindObjectOfType<LivesBar>();
        _wait_damage = false;
        _wait_damage_time = 0.2f;

        shield = FindObjectOfType<Shield>();
        if (shield)
        {
            shield.gameObject.SetActive(false);
        }
        _shield_active = false;

        _move_character = this.gameObject.GetComponent<MoveCharacter>();

    }

    private void Update()
    {
#if UNITY_ANDROID
       
#else
        if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.LeftShift)) { shield.gameObject.SetActive(true); _shield_active = true; }
        if (Input.GetKeyUp(KeyCode.Mouse1) || Input.GetKeyUp(KeyCode.LeftShift)) { shield.gameObject.SetActive(false); _shield_active = false; }
#endif
    }

    public void ShieldDown()
    {
        shield.gameObject.SetActive(true); _shield_active = true;
    }
    public void ShieldUp()
    {
        shield.gameObject.SetActive(false); _shield_active = false;
    }


    public void ReceiveDamage(float _force_hit)
    {

        _move_character.AddForce(_force_hit);


        if (!_wait_damage)
        {
            _was_damage = true;
            lives--;
            _live_bar.Refresh();
            StartWaitDamage();
        }

        if (lives <= 0) Die();
    }

    private void Die()
    {
        FindObjectOfType<MenuControll>().StartDie();
    }


    private IEnumerator WaitDamage()
    {
        _wait_damage = true;
        yield return new WaitForSeconds(_wait_damage_time);
        _wait_damage = false;

    }

    public void StartWaitDamage()
    {
        StartCoroutine("WaitDamage");
    }

}
