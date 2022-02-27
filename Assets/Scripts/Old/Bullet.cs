using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    private GameObject parent;
    public GameObject Parent { set { parent = value; }  get { return parent; } }

    [SerializeField]
    private float speed = 10.0F;

    public bool isUber = false;
    private Vector3 direction;
    public Vector3 Direction { set { direction = value; } }

    private SpriteRenderer sprite;
    GameObject _charachter;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        _charachter = FindObjectOfType<StatesCharachter>().gameObject;
    }

    private void Start()
    {
        Destroy(gameObject, 1.4F);
    }
	

    private void Update()
    {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);   
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {

       StatesCharachter charachter = collider.GetComponent<StatesCharachter>();
        if (collider.tag == "Block") { Destroy(gameObject); return; }
        else if (collider.GetComponent<Shield>() && _charachter != Parent) { Destroy(gameObject); return; }
        else if (charachter && charachter.gameObject != Parent) { charachter.ReceiveDamage(5f); Destroy(gameObject); return; }
        else if (collider.tag == "Bullet") { Destroy(collider.gameObject); Destroy(gameObject); return; }
        else if (collider.GetComponent<FlightEnemy>() && _charachter == Parent) { collider.GetComponent<FlightEnemy>().ReceiveDamage(); Destroy(gameObject); return; }
        else if (collider.GetComponent<Obstacle>()) { Destroy(gameObject); return; }
    }
}

