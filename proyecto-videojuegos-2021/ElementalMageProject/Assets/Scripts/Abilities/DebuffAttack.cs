using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffAttack : MonoBehaviour
{
    [SerializeField] float speed = 25f;
    [SerializeField] public float debuffTime = 3.2f;
    private Vector2 velocity;
    private Vector3 mousePosition;
    private Vector2 startPosition;
    private Rigidbody2D rb;

    void Start()
    {
        startPosition = gameObject.transform.position;
        mousePosition = Input.mousePosition;
        Vector2 MouseScreenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 MouseWorldPosition = Camera.main.ScreenToWorldPoint(MouseScreenPosition);
        Vector2 analog = new Vector2(Input.GetAxisRaw("AnalogHorizontal"), Input.GetAxisRaw("AnalogVertical"));
        if (analog.x != 0 || analog.y != 0)
        {
            analogMovement(analog);
        }
        else
        {
            startMovement(MouseWorldPosition);
        }
    }

    void Update()
    {
        if (Vector2.Distance(startPosition, gameObject.transform.position) > 16)
        {
            Destroy(gameObject);
        }
    }

    public void startMovement(Vector3 mouseDir)
    {
        Vector3 direction = mouseDir - gameObject.transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x);

        var x = Mathf.Cos(angle);
        var y = Mathf.Sin(angle);

        var xSpd = (float)x * speed;
        var yspd = (float)y * speed;

        velocity = new Vector2(xSpd, yspd);
        gameObject.GetComponent<Rigidbody2D>().velocity = velocity;
        transform.Rotate(0,0,Mathf.Atan2(direction.y, direction.x) * 180 / Mathf.PI);
    }

    public void analogMovement(Vector2 analog)
    {
        Vector2 movement = new Vector2(analog.x * speed, analog.y * speed);
        float pythagoras = ((movement.x * movement.x) + (movement.y * movement.y));
        if (pythagoras > (speed * speed))
        {
            float magnitude = Mathf.Sqrt(pythagoras);
            float multiplier = speed / magnitude;
            movement.x *= multiplier;
            movement.y *= multiplier;
        }
        transform.Rotate(0,0,Mathf.Atan2(movement.y, movement.x) * 180 / Mathf.PI);
        gameObject.GetComponent<Rigidbody2D>().velocity = movement;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name != "Projectile(Clone)" && other.name != "Player")
        {
            Basic enemy;
            if (other.TryGetComponent<Basic>(out enemy))
            {
                enemy.startDebuff(debuffTime);
            }
            Destroy(gameObject);
        }
    }
}
