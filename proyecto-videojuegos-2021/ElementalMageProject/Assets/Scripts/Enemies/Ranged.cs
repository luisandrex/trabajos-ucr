using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : MonoBehaviour
{
    [SerializeField] float projectileDamage = 1f;
    [SerializeField] public float projectileSpeed = 2f;

    [SerializeField] float radius = 5f;

    [SerializeField] float timeToShoot = 5f;

    [SerializeField] GameObject projectilePrefab;    

    private Colliders collisions;

    private bool attacking = false;
    private Basic basicInfo;
    private float timer;
    public Transform target;
 
    // Start is called before the first frame update
    void Start()
    {
        basicInfo = this.GetComponent<Basic>();
        collisions = GetComponent<Colliders>();
    }

    // Update is called once per frame
    void Update()
    {
        if(target){
            if(!TryGetComponent<Boss>(out Boss boss)){
                timer += Time.deltaTime;
                if (timer >= timeToShoot){
                    Shoot();
                    timer = timer % timeToShoot;
                }
            }
            // else{
            //     float distance = Vector2.Distance(this.transform.position, target.position);
            //     if(distance < radius){
            //         Vector2 position = Vector2.MoveTowards(
            //             this.transform.position,
            //             target.position,
            //             -1*basicInfo.currentMovementSpeed*Time.deltaTime*(radius-distance)
            //         );
            //         position.x = Mathf.Clamp(position.x, -8.825f + (transform.localScale.x / 2), 8.825f - (transform.localScale.x / 2));
            //         position.y = Mathf.Clamp(position.y, -4.95f + (transform.localScale.y /2) , 4.95f - (transform.localScale.y /2));
            //         if(collisions.collisions[Colliders.ObstacleCollisionDirection.Right].Count != 0){
            //             position.x = Mathf.Clamp(position.x, gameObject.transform.position.x, float.PositiveInfinity);
            //         }
            //         if(collisions.collisions[Colliders.ObstacleCollisionDirection.Left].Count != 0){
            //             position.x = Mathf.Clamp(position.x, float.NegativeInfinity, gameObject.transform.position.x);                
            //         }
            //         if(collisions.collisions[Colliders.ObstacleCollisionDirection.Up].Count != 0){
            //             position.y = Mathf.Clamp(position.y, gameObject.transform.position.y, float.PositiveInfinity);                
            //         }
            //         if(collisions.collisions[Colliders.ObstacleCollisionDirection.Down].Count != 0){
            //             position.y = Mathf.Clamp(position.y,float.NegativeInfinity, gameObject.transform.position.y);
            //         }
            //         transform.position = position;
            //     }
            // }
        }
    }

    private void FixedUpdate()
    {

    }

    private void Shoot(){
        if(target){
            attacking = true;
            StartCoroutine(stopDamageAnim());
            basicInfo.animator.SetBool("Attacking", true);
            GameObject projectile = GameObject.Instantiate(projectilePrefab, transform.position, transform.rotation);
            if(basicInfo.scale < 1){
                projectile.transform.localScale /= 2;
            }
            projectile.GetComponent<Projectile>().setup(projectileDamage, projectileSpeed, target, Projectile.ProjectileType.Normal, basicInfo);
        }
    }

    public void fireBoss(){
        if(target){
            attacking = true;
            StartCoroutine(stopDamageAnim());
            basicInfo.animator.SetBool("Attacking", true);
            GameObject projectile = GameObject.Instantiate(projectilePrefab, transform.position, transform.rotation);
            projectile.GetComponent<Projectile>().setup(projectileDamage, projectileSpeed, target, Projectile.ProjectileType.Debuff, basicInfo);
        }
    }

    public void waterBoss(){
        if(target){
            attacking = true;
            StartCoroutine(stopDamageAnim());
            basicInfo.animator.SetBool("Attacking", true);
            GameObject projectile = GameObject.Instantiate(projectilePrefab, transform.position, transform.rotation);
            projectile.GetComponent<Projectile>().setup(projectileDamage, projectileSpeed, target, Projectile.ProjectileType.Lifesteal, basicInfo);
        }
    }

    private IEnumerator stopDamageAnim()
    {
        yield return new WaitForSeconds(0.3f);
        attacking = false;
        basicInfo.animator.SetBool("Attacking", false);
    }


}
