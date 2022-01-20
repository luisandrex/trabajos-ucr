using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic : MonoBehaviour
{
    [SerializeField] public float hp = 1f;
    [SerializeField] public float maxHp = 1f;

    [SerializeField] public float movementSpeed = 1f;

    public float currentMovementSpeed;

    [SerializeField] public ElementType element;

    private bool debuffed = false;
    private float maxDebufTime = 0f;
    public float currentDebuffTime = 0f;
    private float lastDebuffDamage = 0f;
    public int elementCount = 4;

    public float scale = 1f;
    private bool damaged = false;

    private float timer = 0f;
    private float timerToSwitch = 0.5f;


    public enum ElementType { Fire, Water, Earth, Air, All };
    public Vector3 lastPosition;
    public SpriteRenderer sprite;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
        currentMovementSpeed = movementSpeed;
        lastPosition = transform.position;
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (lastPosition != transform.position)
        {
            if (animator!=null)
                animator.SetBool("Moving", true);
        }
        else
        {
            if(animator!=null)
                animator.SetBool("Moving", false);
        }

        if(timer >= timerToSwitch){
            if (lastPosition.x > transform.position.x)
            {
                sprite.flipX = true;
            }
            else if(lastPosition.x < transform.position.x)
            {
                sprite.flipX = false;
            }            
            lastPosition = transform.position;
            timer = 0f;            
        }

        if (debuffed)
        {
            if(lastDebuffDamage >= 1f)
            {
                tookDamage(1f);
                lastDebuffDamage = 0f;
            }
            if(currentDebuffTime < maxDebufTime)
            {
                lastDebuffDamage += Time.deltaTime;
                currentDebuffTime += Time.deltaTime;
            }
            else
            {
                debuffed = false;
            }
        }


        if ( hp <= 0){
            AudioManager.instance.PlaySfx("EnemyDied");
            if(TryGetComponent<Boss>(out Boss boss)){                
                boss.death();
            }
            Destroy(gameObject);
        }
        timer += Time.deltaTime;
    }

    public void tookDamage(float damage){        
        if(TryGetComponent<Physical>(out Physical physical) && physical.isShielding){
            return;
        }
        else{
            hp -= damage;
            damaged = true;
            if (animator != null)
                animator.SetBool("Damaged", true);
            StartCoroutine(stopDamageAnim());
            if(hp > 0){
                AudioManager.instance.PlaySfx("EnemyHurt");
            }            
        }
    }

    private IEnumerator stopDamageAnim()
    {
        yield return new WaitForSeconds(0.3f);
        damaged = false;
        if (animator != null)
            animator.SetBool("Damaged", false);
    }

    public void startDebuff(float debuffTime)
    {
        maxDebufTime = debuffTime;
        currentDebuffTime = 0f;
        lastDebuffDamage = 0f;
        debuffed = true;
    }

    public void applySlow(float slowPercent){
        this.currentMovementSpeed = currentMovementSpeed * slowPercent / 100;
    }

    public void removeSlow(){
        this.currentMovementSpeed = movementSpeed;
    }

}
