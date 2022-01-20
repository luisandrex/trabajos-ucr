using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProgressManager : MonoBehaviour
{
    private Dictionary <string, UnityEvent> eventDictionary;
    private static ProgressManager _instance;
    private bool fire_boss_is_dead = false;

    private int dead_boss_counter = 0;


    public static ProgressManager Instance
    {
        get
        {
            if (!_instance)
                _instance = FindObjectOfType (typeof (ProgressManager)) as ProgressManager;
                if (!_instance)
                {
                    Debug.LogError ("There needs to be one active EventManager script on a GameObject in your scene.");
                }
                else
                {
                    _instance.Init (); 
                }
            return _instance;
        }
    }

    void Init ()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent>();
        }
    }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } 
        else 
        {
            _instance = this;
        }    
        EventManager.StartListening("Air Boss Death", air_boss_death);
        EventManager.StartListening("Earth Boss Death", earth_boss_death);
        EventManager.StartListening("Fire Boss Death", fire_boss_death);
        EventManager.StartListening("Water Boss Death", water_boss_death);
        EventManager.StartListening("Player Death", player_death);
        EventManager.StartListening("Game Won", game_won);
    }

    private void player_death(){
        GetComponent<MapChanger>().playerDied();
    }

    private void game_won(){
        GetComponent<MapChanger>().gameWon();
    }
    private void fire_boss_death()
    {
        if(++dead_boss_counter == 4)
        {
            EventManager.TriggerEvent("Enable platform");
        }
        // unlock ability
        GetComponent<MapChanger>().deadBoss();
        //aca iria como la parte de cambiar animacion
    }
    private void water_boss_death()
    {
        if(++dead_boss_counter == 4)
        {
            EventManager.TriggerEvent("Enable platform");
        }
        // unlock ability
        GetComponent<MapChanger>().deadBoss();
        //aca iria como la parte de cambiar animacion
    }
    
    private void air_boss_death()
    {
        if(++dead_boss_counter == 4)
        {
            EventManager.TriggerEvent("Enable platform");
        }
        // unlock ability
        GetComponent<MapChanger>().deadBoss();
        //aca iria como la parte de cambiar animacion
    }
    
    private void earth_boss_death()
    {
        if(++dead_boss_counter == 4)
        {
            EventManager.TriggerEvent("Enable platform");
        }
        // unlock ability
        GetComponent<MapChanger>().deadBoss();
        //aca iria como la parte de cambiar animacion
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
