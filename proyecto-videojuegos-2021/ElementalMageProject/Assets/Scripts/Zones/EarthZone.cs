using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EarthZone : MonoBehaviour
{
    [SerializeField]
    public int EnemiesToSpawn = 5;

    private Spawner spawner;
    //private bool spawnFirst = true;
    private Basic basic;
    
    GameObject[] teleporters;
    GameObject[] doors;

    void Start()
    {
        teleporters = GameObject.FindGameObjectsWithTag("Teleporter");
        doors = GameObject.FindGameObjectsWithTag("Door");
        disable_teleporters();

        this.basic = GetComponent<Basic>();

        int min = GameManager.BossesDefeated * 2 + 1;
        int max = min + 3;
        EnemiesToSpawn = Random.Range(min, max);
        this.spawner = GetComponent<Spawner>();

        if (SceneManager.GetActiveScene().name != "EarthBossRoom")
        {
            spawner.spawnEnemies(EnemiesToSpawn);
        }        
    }

    // Update is called once per frame
    void Update()
    {
        if( GameObject.FindGameObjectsWithTag("Enemy").Length == 0){    
            enable_teleporters();
        }            
    }

    void disable_teleporters()
    {
        foreach(GameObject game_object in teleporters){
			game_object.SetActive(false);
		}
        foreach(GameObject game_object in doors){
			game_object.SetActive(true);
		}
    }

    
    void enable_teleporters(){
        foreach (GameObject game_object in teleporters){
            game_object.SetActive(true);
        }      
        foreach(GameObject game_object in doors){
			game_object.SetActive(false);
		}  
    }
}
