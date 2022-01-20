using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physical : MonoBehaviour
{
    [SerializeField] float damage = 1f;

    private Basic basicInfo;
    private Colliders collisions;
   // public Transform target;

    // Dash for bosses
    private bool isDashing = false;
    private float dashX = 0f;
    private float dashY = 0f;

    float currDashTime = 0f;
    bool shielding = false;
    [SerializeField] float maxDashTime = 0.3f;
    [SerializeField] float dashSpeed = 20f;

    // Shield for bosses

    public bool isShielding = false;
    float currShieldTime = 0f;
    [SerializeField] float maxShieldTime = 2f;

    [SerializeField] private Transform target;

    private Color color;

    // Start is called before the first frame update
    void Start()
    {
        basicInfo = this.GetComponent<Basic>();
        collisions = this.GetComponent<Colliders>();
    }

    // Update is called once per frame
    void Update()
    {
        if(target && this.enabled && TryGetComponent<Boss>(out Boss boss)){            
            Vector2 position = transform.position;
            if(isDashing){
                currDashTime -= Time.deltaTime;
                if(currDashTime <= 0){
                    isDashing = false;                    
                }
                position = Vector2.MoveTowards(this.transform.position, target.position, dashSpeed*Time.deltaTime);
            }
            else{
                position = Vector2.MoveTowards(this.transform.position, target.position, basicInfo.currentMovementSpeed*Time.deltaTime);
            }
            if(isShielding){
                currShieldTime -= Time.deltaTime;
                if(currShieldTime <= 0){
                    isShielding = false;
                    gameObject.GetComponentInChildren<SpriteRenderer>().color = this.color;               
                }
            }
            position.x = Mathf.Clamp(position.x, -8.825f + (transform.localScale.x / 2), 8.825f - (transform.localScale.x / 2));
            position.y = Mathf.Clamp(position.y, -4.95f + (transform.localScale.y /2) , 4.95f - (transform.localScale.y / 2));
            position.x = Mathf.Clamp(position.x, -8.825f + (transform.localScale.x / 2), 8.825f - (transform.localScale.x / 2));
            position.y = Mathf.Clamp(position.y, -4.95f + (transform.localScale.y /2) , 4.95f - (transform.localScale.y /2));
            if(collisions.collisions[Colliders.ObstacleCollisionDirection.Right].Count != 0){
                position.x = Mathf.Clamp(position.x, gameObject.transform.position.x, float.PositiveInfinity);
            }
            if(collisions.collisions[Colliders.ObstacleCollisionDirection.Left].Count != 0){
                position.x = Mathf.Clamp(position.x, float.NegativeInfinity, gameObject.transform.position.x);                
            }
            if(collisions.collisions[Colliders.ObstacleCollisionDirection.Up].Count != 0){
                position.y = Mathf.Clamp(position.y, gameObject.transform.position.y, float.PositiveInfinity);                
            }
            if(collisions.collisions[Colliders.ObstacleCollisionDirection.Down].Count != 0){
                position.y = Mathf.Clamp(position.y,float.NegativeInfinity, gameObject.transform.position.y);
            }
            transform.position = position;
        }
    }

    public void airBoss(){
        isDashing = true;
        currDashTime = maxDashTime;
    }

    public void earthBoss(){
        shielding = true;
        basicInfo.animator.SetBool("Shielding", true);
        gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.magenta;
        StartCoroutine(stopShieldAnim());
        isShielding = true;
        currShieldTime = maxShieldTime;
    }

    private IEnumerator stopShieldAnim()
    {
        yield return new WaitForSeconds(maxShieldTime);
        shielding = false;
        basicInfo.animator.SetBool("Shielding", false);
        this.color = gameObject.GetComponentInChildren<SpriteRenderer>().color;
        gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
    }

    private void OnCollisionEnter2D(Collision2D other){        
        if(other.gameObject.name == "Player"){
            if(other.gameObject.TryGetComponent<Player>(out Player player) && this.enabled){
                player.takeDamage(damage);
            }
        }
    }

}
