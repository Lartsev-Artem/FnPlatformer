using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacter : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    public Animator _animator;
    private NewCharState State
    {
        get { return (NewCharState)_animator.GetInteger("State"); }
        set { _animator.SetInteger("State", (int)value); }
    }

    [SerializeField]
    private float speed = 5.0f;//3.0F;
    public float cur_speed;
    public float Speed
    {
        get { return cur_speed; }
        set { cur_speed = value; }
    }

    [SerializeField]
    private float jumpForce = 4.0F;
    [SerializeField]
    private float _wait_jump_time = 0.15f;

    [SerializeField]
    private bool isGrounded = false;
    public bool IsGrounded { get { return isGrounded; } }
    private bool B_FacingRight = true;
    public bool FacingRight
    {
        get
        {
            return B_FacingRight;
        }
    }
    public bool waitJump;


    private float _time_buf;

    [SerializeField]
    private Joystick _joystick;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        waitJump = false;
        _time_buf = 10f;
        cur_speed = speed;
    }
    private void Start()
    {
        _joystick = FindObjectOfType<Joystick>();
    }

    private void FixedUpdate()
    {
        CheckGround();
        if (isGrounded) State = NewCharState.Idle;
        else State = NewCharState.Jump;

#if UNITY_ANDROID
        //if (Mathf.Abs(_joystick.Horizontal) > 0.01f) Run();
        if (isGrounded && _joystick.Vertical > 0.7f && !waitJump) Jump();
#else
    //    if (Input.GetButton("Horizontal")) Run();
        if (isGrounded && Input.GetButton("Jump") && !waitJump) Jump();
#endif

    }

    private void Update()
    {
        //    if (Input.GetButtonDown("Fire1") && !isShoot && !press) { isShoot = true; animator.SetBool("Shoot", true); Invoke("Shoot", speed_Shoot / 2); }// Shoot();  = true; Invoke("Shoot", 0.5f); }// Shoot();  //{ isShoot = true; State = CharState.Shoot;  Invoke("Shoot", 0.5f); }// Shoot(); }

#if UNITY_ANDROID
        if (Mathf.Abs(_joystick.Horizontal) > 0.01f) Run();
    //    if (isGrounded && _joystick.Vertical > 0.7f && !waitJump) Jump();
#else
        if (Input.GetButton("Horizontal")) Run();
    //    if (isGrounded && Input.GetButtonDown("Jump") && !waitJump) Jump();
#endif
    }

    private void CheckGround()
    {
        Physics2D.queriesStartInColliders = false;

       // Debug.DrawLine(new Vector2(transform.position.x - 0.3f, transform.position.y), new Vector2(transform.position.x - 0.3f, transform.position.y - 0.9f));

        RaycastHit2D hit = Physics2D.Linecast(new Vector2(transform.position.x - 0.3f, transform.position.y ), new Vector2(transform.position.x - 0.3f, transform.position.y - 0.9f)); // +0.2, -0.5- чтобы луч начинался выше земли и кончался в ней
        RaycastHit2D hit2 = Physics2D.Linecast(new Vector2(transform.position.x + 0.3f, transform.position.y), new Vector2(transform.position.x + 0.3f, transform.position.y - 0.9f));

        if (hit.collider != null && hit.collider.gameObject.CompareTag("Block")) { isGrounded = true;  }
        else if (hit2.collider != null && hit2.collider.gameObject.CompareTag("Block")) {isGrounded = true;  }
        else if (hit2.collider != null && hit2.collider.gameObject.CompareTag("Enemy")) isGrounded = true;
        else if (hit.collider != null && hit.collider.gameObject.CompareTag("Enemy")) isGrounded = true;
        else
            isGrounded = false;
    }

    private void Run()
    {
#if UNITY_ANDROID
        Vector3 direction = transform.right * _joystick.Horizontal;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, cur_speed * Time.deltaTime);
#else
        Vector3 direction = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, cur_speed * Time.deltaTime);
#endif
        // sprite.flipX = direction.x > 0.0F;

        if (direction.x < 0.0F && B_FacingRight)
        {
            Flip();
        }
        else if (direction.x > 0.0F && !B_FacingRight)
        {
            Flip();
        }

        if (isGrounded) State = NewCharState.Run;
    }
    private void Flip()
    {
        B_FacingRight = !B_FacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    private void Jump()
    {
        AddForce(jumpForce);
       // _rigidbody.velocity = new Vector2(0, jumpForce); 
    }

    public void AddForce(float _force)
    {
        if (!waitJump) _rigidbody.AddForce(transform.up * _force, ForceMode2D.Impulse);
        StartWaitJump();
       
    }

    private IEnumerator WaitJump()
    {
        waitJump = true;
        //yield return new WaitForSeconds(_wait_jump_time);
        while (_rigidbody.velocity.y > 0) { 
            yield return new WaitForSeconds(0.001f); 
        }
        waitJump = false;

    }

    public void StartWaitJump()
    {
        StartCoroutine("WaitJump");
    }

    private IEnumerator StartEnergy(float newspeed)
    {
        Speed = newspeed;
        yield return new WaitForSeconds(_time_buf);
        Speed = speed;
    }
    public void StartEnergyMove(float speed)
    {
        StartCoroutine("StartEnergy", speed);
    }
}
public enum NewCharState
{
    Idle,
    Run,
    Jump
}
