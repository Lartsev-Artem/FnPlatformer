using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightEnemy : MoveEnemy
{
    private bool _was_earth;
    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        StatesCharachter charachter = collider.GetComponent<StatesCharachter>();

        if (charachter)
        {
             charachter.ReceiveDamage(_force_hit);
        }
    }
    protected override void Start()
    {
        hearts = new GameObject[_maxLives];
        hearts[0] = transform.Find("HeartEnemy").gameObject;
        hearts[1] = transform.Find("HeartEnemy1").gameObject;
        hearts[2] = transform.Find("HeartEnemy2").gameObject;

        for (int i = _maxLives-1; i >= livesEnemy; i--)
        {
            hearts[i].SetActive(false);
        }
        if (livesEnemy == 1) hearts[0].SetActive(false);

        direction = transform.right;
        rayLength = GetComponent<BoxCollider2D>().size.x * GetComponent<Transform>().localScale.x / 1.5f + GetComponent<BoxCollider2D>().offset.x;

        RaycastHit2D hit = Physics2D.Linecast(transform.position, (Vector2)transform.position + 5f * Vector2.down,enemyMask);
        if (hit.collider != null)
        {
            _was_earth = true;
            transform.position = new Vector2(hit.collider.gameObject.transform.position.x, hit.collider.gameObject.transform.position.y + 2);
        }
        else
            _was_earth = false;
    }
    private bool canflip = true;
    protected override void FixedUpdate()
    {

        Vector2 lineCastPos = new Vector2(transform.position.x, transform.position.y);//myTrans.position-myTrans.right*myWidth;

        RaycastHit2D hit = Physics2D.Linecast(lineCastPos, lineCastPos + 3 * rayLength * Vector2.down);
        RaycastHit2D hit1 = Physics2D.Linecast(lineCastPos, lineCastPos + 1.8f * rayLength * Vector2.down);
        RaycastHit2D hit2 = Physics2D.Linecast(lineCastPos, lineCastPos + 1.8f * rayLength * Vector2.right*direction);

        // bool isGroundeddown = Physics2D.Linecast(lineCastPos + direction * rayLength * Vector2.right, lineCastPos + direction * rayLength * Vector2.right + Vector2.down, enemyMask); // проверка на блок внизу

        if (canflip&&((hit1.collider != null && !hit1.collider.GetComponent<StatesCharachter>()) || 
            (hit.collider == null && _was_earth) || (hit.collider != null && !_was_earth) || (hit2.collider != null && !hit2.collider.GetComponent<StatesCharachter>())))
        // hit встречает poligon и не доходит до box
        {
            Flip();
            StartCoroutine("WaitFlip");
        }
    }

    private IEnumerator WaitFlip()
    {
        canflip = false;
        yield return new WaitForSeconds(0.1f);
        canflip = true;

    }
}
