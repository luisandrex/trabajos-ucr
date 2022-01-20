using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed = 0;
    private float damage = 0;

    private Basic basic;
    private ProjectileType type;
    public enum ProjectileType {Normal, Debuff, Lifesteal}

    void Start()
    {

    }
    private void OnTriggerEnter2D(Collider2D other){        
        if(other.name == "Player" || other.name.Contains("Wall")){
            if(other.TryGetComponent<Player>(out Player player)){
                switch (type)
                {
                    case ProjectileType.Normal:
                        player.takeDamage(damage);
                        break;
                    case ProjectileType.Debuff:
                        player.hitWithDebuff();
                        break;
                    case ProjectileType.Lifesteal:
                        player.takeDamage(damage);
                        basic.hp += damage;
                        break;
                }
            }
            Destroy(gameObject);
        } 
    }

    public void setup(float damage, float projectileSpeed, Transform target, ProjectileType type, Basic basic){
        this.basic = basic;
        this.damage = damage;
        this.speed = projectileSpeed;
        this.type = type;

        Vector3 startPosition = gameObject.transform.position;

        Vector3 direction = target.position - gameObject.transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x);

        var x = Mathf.Cos(angle);
        var y = Mathf.Sin(angle);

        var xSpd = (float)x * speed;
        var yspd = (float)y * speed;

        Vector2 velocity = new Vector2(xSpd, yspd);
        gameObject.GetComponent<Rigidbody2D>().velocity = velocity;

        if(type == ProjectileType.Lifesteal){
            this.GetComponentInChildren<SpriteRenderer>().color = Color.blue;
        }

        transform.Rotate(0,0,Mathf.Atan2(direction.y, direction.x) * 180 / Mathf.PI);
    }

}
