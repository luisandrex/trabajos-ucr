using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MonoBehaviour
{
    [SerializeField] float speed = 25f;
    [SerializeField] public float damage = 2f;
    [SerializeField] bool lifeSteal = false;
    private Vector2 velocity;
    private Vector3 mousePosition;
    private Vector2 startPosition;
    private Player player;
    private bool invulnerability = true;

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
        player = GetComponentInParent<Player>();
        StartCoroutine(stopInvul());
    }

    private IEnumerator stopInvul()
    {
        yield return new WaitForSeconds(0.1f);
        invulnerability = false;
    }

    void Update()
    {
        if (Vector2.Distance(startPosition, gameObject.transform.position)>16)
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
        gameObject.GetComponent<Rigidbody2D>().velocity = movement;
        transform.Rotate(0,0,Mathf.Atan2(movement.y, movement.x) * 180 / Mathf.PI);
    }

    private void OnTriggerEnter2D(Collider2D other){      
        if(other.name != "ProjectileArcher(Clone)"&& other.name != "ProjectileWizard(Clone)" && other.name != "Player" && other.gameObject.tag != "PlayerAttack"){
            if (invulnerability){
                if(other.gameObject.tag != "Wall" && other.gameObject.tag != "Trap")
                {
                    collisionBehavior(other);
                }
            }
            else
            {
                collisionBehavior(other);
            }
        }

    }


    private void collisionBehavior(Collider2D other)
    {
        if (other.TryGetComponent<Basic>(out Basic enemy))
        {
            enemy.tookDamage(damage);
            if (lifeSteal)
            {
                player.heal(damage);
            }
        }
        Destroy(gameObject);
    }

}
