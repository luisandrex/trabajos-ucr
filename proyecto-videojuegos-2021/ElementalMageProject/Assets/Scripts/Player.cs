using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Stats
    private float maxHealth = GameManager.MaxPlayerHealth;
    private float Health = GameManager.PlayerHealth;
    [SerializeField] float moveSpeed = 7f;
    private float currentMovementSpeed;
    [SerializeField] float projectileSpeed = 15f;
    [SerializeField] float dashSpeed = 20f;

    //Attack sprites
    [SerializeField] GameObject bassicAttack;
    [SerializeField] GameObject shield;
    [SerializeField] GameObject debuff;
    [SerializeField] GameObject lifesteal;

    //Timers
    float currFireCooldown = 0f;
    [SerializeField] float maxFireCooldown = 0.5f;
    float currDashCooldown = 0f;
    [SerializeField] float maxDashCooldown = 1f;

    float currShieldCooldown = 0f;
    [SerializeField] float maxShieldCooldown = 5f;

    float currDebuffCooldown = 0f;
    [SerializeField] float maxDebuffCooldown = 5f;

    float currLifestealCooldown = 0f;
    [SerializeField] float maxLifestealCooldown = 10f;

    float currShieldTime = 0f;
    [SerializeField] float maxShieldTime = 2f;

    float currDashTime = 0f;
    [SerializeField] float maxDashTime = 0.3f;

    //States
    private bool dashing = false;
    private float dashX = 0f;
    private float dashY = 0f;
    private bool shielding = false;
    private bool damaged = false;

    private Vector2 movement;
    private Rigidbody2D rigidBody;
    private UIManager uIManager;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Colliders obstaclesColliders;

    // Debuffed by boss
    private bool debuffed = false;
    [SerializeField]private float maxDebufTime = 3.2f;
    public float currentDebuffTime = 0f;
    private float lastDebuffDamage = 0f;

    void Start()
    {
        currentMovementSpeed = moveSpeed;
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        Health = GameManager.PlayerHealth;
        uIManager = FindObjectOfType<UIManager>();
        obstaclesColliders = gameObject.GetComponent<Colliders>();
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        checkForHealth();

        checkCooldowns();

        //regular attack
        if (!uIManager.inMenu)
        {
            if (Input.GetButtonDown("Fire1") && currFireCooldown <= 0)
            {
                Instantiate(bassicAttack, gameObject.transform);
                currFireCooldown = maxFireCooldown;
            }

            if (Input.GetButtonDown("Dash") && GameManager.DashUnlocked)
            {
                if (!dashing && currDashCooldown <= 0)
                {
                    dashing = true;
                    currDashTime = maxDashTime;
                    dashX = dashSpeed * Input.GetAxisRaw("Horizontal");
                    dashY = dashSpeed * Input.GetAxisRaw("Vertical");
                    if(uIManager){
                        uIManager.manageDash(false);
                    }
                }
            }

            if (Input.GetButtonDown("Shield") && GameManager.ShieldUnclocked)
            {
                if (!shielding && currShieldCooldown <= 0)
                {
                    shielding = true;
                    currShieldTime = maxShieldTime;
                    gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
                    if(uIManager){
                        uIManager.manageShield(false);
                    }
                }
            }

            if (Input.GetButtonDown("Debuff") && GameManager.DebuffUnlocked)
            {
                if (currDebuffCooldown <= 0)
                {
                    currDebuffCooldown = maxDebuffCooldown;
                    Instantiate(debuff, gameObject.transform);
                    if(uIManager){
                        uIManager.manageDebuff(false);
                    }
                }
            }

            if (Input.GetButtonDown("LifeSteal") && GameManager.LifestealUnlocked)
            {
                if (currLifestealCooldown <= 0)
                {
                    currLifestealCooldown = maxLifestealCooldown;
                    Instantiate(lifesteal, gameObject.transform);
                    if(uIManager){
                        uIManager.manageLifesteal(false);
                    }
                }
            }
        }
        if (Input.GetButtonDown("Pause"))
        {
            pause();
        }

        checkShield();

        processMovement();        
    }


    void FixedUpdate()
    {
        if (movement != Vector2.zero)
        {
            bool stop = false;
            Vector2 dirVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            RaycastHit2D[] rayHits;
            if (dashing)
            {
                rayHits = Physics2D.RaycastAll(transform.position, dirVector, (25 + dashSpeed) * Time.fixedDeltaTime);
                animator.SetBool("Moving", true);
            }
            else
            {
                rayHits = Physics2D.RaycastAll(transform.position, dirVector, (25 + currentMovementSpeed) * Time.fixedDeltaTime);
                animator.SetBool("Moving", true);
            }
            for (int i = 0; i < rayHits.Length; i++)
            {
                if (rayHits[i].collider != null && rayHits[i].collider.gameObject.tag == "Wall")
                {
                    i = rayHits.Length;
                    stop = true;
                    animator.SetBool("Moving", false);
                    if (dashing)
                    {
                        dashing = false;
                    }
                }
            }
            if (!stop)
            {
                executeMovement();
            }
        }
        else
        {
            animator.SetBool("Moving", false); 
        }
    }

    private void executeMovement()
    {
        float currSpeed = moveSpeed;
        if (dashing)
        {
            currSpeed = dashSpeed;
        }
        float pythagoras = ((movement.x * movement.x) + (movement.y * movement.y));
        if (pythagoras > (currSpeed * currSpeed))
        {
            float magnitude = Mathf.Sqrt(pythagoras);
            float multiplier = currSpeed / magnitude;
            movement.x *= multiplier;
            movement.y *= multiplier;
        }
        Vector2 position = rigidBody.position + movement * Time.fixedDeltaTime;
        if(obstaclesColliders.collisions[Colliders.ObstacleCollisionDirection.Right].Count != 0){
            position.x = Mathf.Clamp(position.x, rigidBody.position.x, float.PositiveInfinity);
        }
        if(obstaclesColliders.collisions[Colliders.ObstacleCollisionDirection.Left].Count != 0){
            position.x = Mathf.Clamp(position.x, float.NegativeInfinity, rigidBody.position.x);                
        }
        if(obstaclesColliders.collisions[Colliders.ObstacleCollisionDirection.Up].Count != 0){
            position.y = Mathf.Clamp(position.y, rigidBody.position.y, float.PositiveInfinity);                
        }
        if(obstaclesColliders.collisions[Colliders.ObstacleCollisionDirection.Down].Count != 0){
            position.y = Mathf.Clamp(position.y,float.NegativeInfinity, rigidBody.position.y);
        }
        rigidBody.MovePosition(position);
        if(movement.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (movement.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        rigidBody.MovePosition(rigidBody.position + movement * Time.fixedDeltaTime);
    }

    private void checkForHealth(){
        if (debuffed)
        {
            if(lastDebuffDamage >= 1f)
            {
                takeDamage(1f);
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
        if(Health <= 0){
            AudioManager.instance.PlaySfx("PlayerDied");
            EventManager.TriggerEvent("Player Death");
        }
    }

    private void checkCooldowns()
    {
        if (currDashCooldown > 0)
        {
            currDashCooldown -= Time.deltaTime;
        }
        else if(!dashing)
        {
            if(uIManager)
                uIManager.manageDash(true);
        }

        if (currDebuffCooldown > 0)
        {
            currDebuffCooldown -= Time.deltaTime;
        }
        else
        {
            if (uIManager)
                uIManager.manageDebuff(true);
        }

        if (currShieldCooldown > 0)
        {
            currShieldCooldown -= Time.deltaTime;
        }
        else if(!shielding)
        {
            if (uIManager)
                uIManager.manageShield(true);
        }

        if (currLifestealCooldown > 0)
        {
            currLifestealCooldown -= Time.deltaTime;
        }
        else
        {
            if (uIManager)
                uIManager.manageLifesteal(true);
        }        
        if(currFireCooldown > 0){
            currFireCooldown -= Time.deltaTime;
        }
    }

    private void processMovement()
    {        
        if (!dashing)
        {
            movement.x = currentMovementSpeed * Input.GetAxisRaw("Horizontal") * transform.localScale.x;
            movement.y = currentMovementSpeed * Input.GetAxisRaw("Vertical") * transform.localScale.y;
        }
        else if (currDashTime <= 0)
        {
            dashing = false;
            currDashCooldown = maxDashCooldown;
        }
        else
        {
            currDashTime -= Time.deltaTime;
            movement.x = dashX;
            movement.y = dashY;
        }        
    }

    private void checkShield()
    {
        if (shielding)
        {
            if (currShieldTime <= 0)
            {
                shielding = false;
                currShieldCooldown = maxShieldCooldown;
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }   
            else
            {
                currShieldTime -= Time.deltaTime;
            }
        }
    }

    public void takeDamage(float damage){
        if (!shielding && !damaged)
        {
            Health -= damage;
            GameManager.PlayerHealth = Health;
            AudioManager.instance.PlaySfx("PlayerHurt");
            damaged = true;
            animator.SetBool("Damaged", true);
            StartCoroutine(stopDamageAnim());
        }
    }

    private IEnumerator stopDamageAnim()
    {
        yield return new WaitForSeconds(0.3f);
        damaged = false;
        animator.SetBool("Damaged", false);
    }

    public void heal(float healValue)
    {
        if ((Health + healValue) < maxHealth)
        {
            Health += healValue;
            GameManager.PlayerHealth = Health;
        }
        else if(Health < maxHealth)
        {
            Health = maxHealth;
            GameManager.PlayerHealth = Health;
        }
    }

    // Obstacles methods

    public void applySlow(float slowPercent){
        this.currentMovementSpeed = currentMovementSpeed * slowPercent / 100;
    }

    public void removeSlow(){
        this.currentMovementSpeed = moveSpeed;
    }

    // Bosses methods

    public void hitWithDebuff(){
        debuffed = true;        
    }
        
    private void pause()
    {
        if (uIManager)
            uIManager.menuManager();
    }

}
