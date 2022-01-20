using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    enum Skill : int 
    { 
        Debuff, 
        SpecialAttack, 
        Shield, 
        RangedAttack
    };

    const int SKILLS_AMOUNT = 4;
    private bool is_menu_shown = false;
    private static Menu _instance;
    public Text available_points_text;
    public static int available_points = 0;
    [SerializeField] Skill selected_skill;
    GameObject[] skills_available;
    GameObject canvas;
    public Button buy_debuff, buy_special_attack, buy_shield, buy_ranged_attack ;
    public Button debuff, special_attack, shield, ranged_attack;

    private bool[] enabled_skills;

    [SerializeField] private Animator sceneTransitions;

    public static Menu Instance
    {
        get
        {
            if (!_instance)
                _instance = FindObjectOfType (typeof (Menu)) as Menu;
                if (!_instance)
                {
                    Debug.LogError ("There needs to be one active EventManager script on a GameObject in your scene.");
                }
            return _instance;
        }
    }

    private void Awake()
    {
        canvas = GameObject.Find("Canvas");
        EventManager.StartListening("Fire Boss Death" , fire_boss_defeated); 
        EventManager.StartListening("Water Boss Death", water_boss_defeated);   
        EventManager.StartListening("Earth Boss Death", earth_boss_defeated);   
        EventManager.StartListening("Air Boss Death"  , air_boss_defeated);

        gameObject.transform.Find("Debuff").gameObject.SetActive(!GameManager.DebuffUnlocked);
        gameObject.transform.Find("Buy_debuff").gameObject.SetActive(!GameManager.DebuffUnlocked);
        gameObject.transform.Find("SpAttack").gameObject.SetActive(!GameManager.DashUnlocked);
        gameObject.transform.Find("Buy_spattack").gameObject.SetActive(!GameManager.DashUnlocked);
        gameObject.transform.Find("Shield").gameObject.SetActive(!GameManager.ShieldUnclocked);
        gameObject.transform.Find("Buy_shield").gameObject.SetActive(!GameManager.ShieldUnclocked);
        gameObject.transform.Find("RangedAttack").gameObject.SetActive(!GameManager.LifestealUnlocked);
        gameObject.transform.Find("Buy_rgattack").gameObject.SetActive(!GameManager.LifestealUnlocked);




        enabled_skills  = new bool[4];
        
        for (int counter = 0; counter < SKILLS_AMOUNT ; ++counter)
        {
            enabled_skills[counter] = false;
        }
    }
    // Start is called before the first frame update

    /// <summary>method <c>Start</c> Hides the menu and adds the listeners for the buttons.</summary>
	void Start () {
		Time.timeScale = 1;
		skills_available = GameObject.FindGameObjectsWithTag("ShowOnPause");
		// hide_paused();
        // available_points_text = gameObject.transform.Find("Available Points").GetComponent<Text>();

        //Add listeners for the skills buying buttons
        buy_debuff.onClick.AddListener(delegate{enable_skill(0);});
        buy_special_attack.onClick.AddListener(delegate{enable_skill(1);});
        buy_shield.onClick.AddListener(delegate{enable_skill(2);});
        buy_ranged_attack.onClick.AddListener(delegate{enable_skill(3);});

        //disable_buttons();
        // add_points();
        enable_buttons();
        //enable_skills();
        // show_paused();
        StartCoroutine(fadeOut());        
	}

    IEnumerator fadeOut(){
        sceneTransitions.SetInteger("Animation", 9);
        yield return new WaitForSeconds(0.44f);
        sceneTransitions.SetInteger("Animation", 0);
    }


    // Update is called once per frame
    void Update()
    {
        // add_points();
       
        // //Time.timeScale = 0;
        // // show_paused();
        // if (available_points > 0)
        // {
        //     enable_buttons();
        // }
        // else
        // {
        //     disable_buttons();
        // }
        //     
        
    }


    /// <summary>method <c>enable_skills</c> Enables those skills already bought by the player.</summary>
    void enable_skills()
    {
        GameObject button;
        for (int counter = 0; counter < SKILLS_AMOUNT ; ++counter)
        {
            switch(counter)
            {
                case 0:
                    if(enabled_skills[counter])
                    {
                        button = gameObject.transform.Find ("Debuff").gameObject;
                        button.SetActive(false);
                        button = gameObject.transform.Find("Buy_debuff").gameObject;
                        button.SetActive(false);
                    }
                    break;
                case 1:
                    if(enabled_skills[counter])
                    {
                        button = gameObject.transform.Find ("SpAttack").gameObject;
                        button.SetActive(false);
                        button = gameObject.transform.Find("Buy_spattack").gameObject;
                        button.SetActive(false);
                    }
                    break;
                case 2:
                    if(enabled_skills[counter])
                    {
                        button = gameObject.transform.Find ("Shield").gameObject;
                        button.SetActive(false);
                        button = gameObject.transform.Find("Buy_shield").gameObject;
                        button.SetActive(false);
                    }
                    break;
                default:
                    if(enabled_skills[counter])
                    {
                        button = gameObject.transform.Find ("RangedAttack").gameObject;
                        button.SetActive(false);
                        button =  gameObject.transform.Find("Buy_rgattack").gameObject;
                        button.SetActive(false);
                    }
                    break;
            }
        }
    }


    void enable_buttons()
    {
        for (int counter = 0; counter < SKILLS_AMOUNT ; ++counter)
        {
            switch(counter)
            {
                case 0:
                {
                    buy_debuff.enabled = enabled_skills[counter]? false:true;

                }
                break;
                case 1:
                {
                    buy_special_attack.enabled = enabled_skills[counter]? false:true;           
                }
                break;                
                case 2:
                {
                    buy_shield.enabled = enabled_skills[counter]? false:true;              
                }
                break;                
                default:
                {
                    buy_ranged_attack.enabled = enabled_skills[counter]? false:true;                
                }
                break;
            }
        }
    }

    public void fire_boss_defeated()
    {
        available_points ++;
        GameManager.FireBossDefeated = true;
        increaseHealth();
    }

    public void water_boss_defeated()
    {
        available_points ++;
        GameManager.WaterBossDefeated = true;
        increaseHealth();
    }
    public void air_boss_defeated()
    {
        available_points ++;
        GameManager.AirBossDefeated = true;
        increaseHealth();
    }
    public void earth_boss_defeated()
    {
        available_points ++;
        GameManager.EarthBossDefeated = true;
        increaseHealth();
    }

    /// <summary>method <c>enable_skill</c> Enables a skill bought by the player and assigns it
    ///          as the one being used.</summary>
    void enable_skill(int skill)
    {
        enabled_skills[skill] = true;
        switch (skill)
        {
            case 0:
                GameManager.DebuffUnlocked = true;
                break;
            case 1:
                GameManager.DashUnlocked = true;
                break;
            case 2:
                GameManager.ShieldUnclocked = true;
                break;
            case 3:
                GameManager.LifestealUnlocked = true;
                break;
        }
        available_points--;
        GetComponent<MapChanger>().backToHub();
    }

    public void increaseHealth()
    {
        GameManager.MaxPlayerHealth++;
        GameManager.PlayerHealth = GameManager.MaxPlayerHealth;
    }
}
