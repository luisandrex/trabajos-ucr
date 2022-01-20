using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Spawner : MonoBehaviour
{
    private List<Transform> spawnZones = new List<Transform>();

    [SerializeField]
    private List<GameObject> enemyPrefabs= new List<GameObject>();

    private Basic.ElementType element;
    // Start is called before the first frame update
    private bool[] spawnZonesUsed;
    private int counter = 0;

    private bool canSpawn = true;
    void Awake()
    {
        GameObject path_parent = GameObject.Find("WalkablePaths");
        spawnZones = new List<Transform>(path_parent.GetComponentsInChildren<Transform>());
        spawnZonesUsed = new bool[spawnZones.Count];
        for(int i = 0; i < spawnZones.Count; ++i){spawnZonesUsed[i] = false;}        
    }
    void Start()
    {
        if(TryGetComponent<Boss>(out Boss boss)){                
            this.element = GetComponent<Basic>().element;
        }
    }

    void Update(){
        if( GameObject.FindGameObjectsWithTag("Enemy").Length == 0){    
            canSpawn = true;
        }            
    }

    public void spawnEnemies(int enemyQuantity){
        Scene scene = SceneManager.GetActiveScene();
        while(enemyQuantity != 0 && canSpawn){
            int spawn_number = Random.Range(0, spawnZones.Count);

            if (!scene.name.Contains("Boss"))
            {
                while(spawnZonesUsed[spawn_number])
                {            
                    spawn_number = Random.Range(0, spawnZones.Count);
                }
                spawnZonesUsed[spawn_number] = true;
            }
            int enemy_number = Random.Range(0, enemyPrefabs.Count);
            GameObject enemy = Instantiate(enemyPrefabs[enemy_number], spawnZones[spawn_number].transform.position, spawnZones[spawn_number].transform.rotation);
            if(TryGetComponent<Basic>(out Basic basic)){
                if(basic.element == Basic.ElementType.All){
                    enemy.transform.localScale /= 2;
                    basic.movementSpeed /= 2;
                    basic.scale /= 2;
                }
            }
            enemy.name = enemyPrefabs[enemy_number].ToString() + counter;
            enemy.GetComponent<Basic>().element = this.element;
            if(enemy.TryGetComponent<Ranged>(out Ranged ranged)){                
                ranged.projectileSpeed /= 2;
                ranged.target = GameObject.Find("Player").transform;
            }
            enemyQuantity--;
            counter++;
        }
        canSpawn = false;        
    }
}
