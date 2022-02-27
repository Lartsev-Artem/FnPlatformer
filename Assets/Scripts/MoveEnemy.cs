using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MoveEnemy : Obstacle
{
    protected const int _maxLives = 3;
    [SerializeField]
    protected int livesEnemy = _maxLives;
    public int LivesEnemy
    {
        get { return livesEnemy; }
        set {  livesEnemy = value;   }
    }

    [SerializeField]
    protected GameObject[] hearts;

    public LayerMask enemyMask;

    [SerializeField]
    protected float speed = 2.0F;
    protected Vector3 direction;
    [SerializeField]
    protected bool _facing_right = true;
    public bool Facing_right
    {
        get
        {
            return _facing_right;
        }
    }



    protected void Update()
    {
        Move();
    }

    protected float pos;
    protected float rayLength;
    protected virtual void Start()
    {
        hearts = new GameObject[_maxLives];
        hearts[0] = transform.Find("HeartEnemy").gameObject;
        hearts[1] = transform.Find("HeartEnemy1").gameObject;
        hearts[2] = transform.Find("HeartEnemy2").gameObject;

        for(int i= _maxLives-1; i >= livesEnemy; i--)
        {
            hearts[i].SetActive(false);
        }
        if (livesEnemy == 1) hearts[0].SetActive(false);

        direction = transform.right;
        float Size = GetComponent<BoxCollider2D>().size.y;
        pos = -GetComponent<Transform>().localScale.y * Size / 2 + GetComponent<BoxCollider2D>().offset.y + 0.2f;

        rayLength = GetComponent<BoxCollider2D>().size.x * GetComponent<Transform>().localScale.x / 1.5f + GetComponent<BoxCollider2D>().offset.x;

    }
    protected virtual  void FixedUpdate()
    {

        Vector2 lineCastPos = new Vector2(transform.position.x, transform.position.y + pos);//myTrans.position-myTrans.right*myWidth;
//        Debug.DrawLine(lineCastPos, lineCastPos + direction * rayLength * Vector2.right);
 
        RaycastHit2D hit = Physics2D.Linecast(lineCastPos, lineCastPos + direction * rayLength * Vector2.right);
        bool isGroundeddown = Physics2D.Linecast(lineCastPos + direction * rayLength * Vector2.right, lineCastPos + direction * rayLength * Vector2.right +Vector2.down, enemyMask); // проверка на блок внизу
        RaycastHit2D[] hitt = Physics2D.LinecastAll(lineCastPos, lineCastPos + direction * rayLength * Vector2.right);

        if (!isGroundeddown || (hit.collider != null /*&& !hit.collider.isTrigger*/ && !hit.collider.GetComponent<StatesCharachter>())) 
            // hit встречает poligon и не доходит до box
        {
            Flip();
        }
    }
    protected  void Flip()
    {
        direction *= -1.0F;
        _facing_right = !_facing_right;
        // sprite.flipX = direction.x > 0.0F;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;


    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        StatesCharachter charachter = collider.GetComponent<StatesCharachter>();

        if (charachter)
        {
            if (Mathf.Abs(charachter.transform.position.x - transform.position.x) < 0.8F)
            {
                ReceiveDamage();
                charachter.gameObject.GetComponent<MoveCharacter>().AddForce(_force_hit);
            }
            else charachter.ReceiveDamage(_force_hit);
        }
    }

        protected void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
    }

    public void ReceiveDamage()
    {
        livesEnemy--;
        if (livesEnemy <= _maxLives && livesEnemy >= 0) hearts[livesEnemy].SetActive(false);
        if (livesEnemy <= 0)
            Destroy(gameObject);

    }
}
