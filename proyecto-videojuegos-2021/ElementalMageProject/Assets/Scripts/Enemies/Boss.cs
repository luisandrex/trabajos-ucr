using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    // Core variables
    private Basic basic;
    private Ranged ranged;
    private RangedAI rangedAI;
    private Physical physical;

    // Spawner variables

    [SerializeField]
    public int EnemiesToSpawn = 3;
    private Spawner spawner;

    //      Timers
    //  Spawn timers  
    [SerializeField]
    private float timeToSpawn = 5f;
    private float spawnTimer = 0;
    //  Ability timers
    [SerializeField]
    private float timeToUseAbility = 5f;
    private float abilityTimer = 0;

    //  Final boss variable
    private int abilityRotation = 0;

    // Air boss
    [SerializeField] float dashSpeed = 20f;
    float currDashTime = 0f;
    [SerializeField] float maxDashTime = 0.3f;

    private bool dashing = false;
    private float dashX = 0f;
    private float dashY = 0f;



    // Start is called before the first frame update
    void Start()
    {
        basic = GetComponent<Basic>();
        spawner = GetComponent<Spawner>();
        TryGetComponent<Physical>(out this.physical);
        TryGetComponent<Ranged>(out this.ranged);
        TryGetComponent<RangedAI>(out this.rangedAI);
        if(basic.element == Basic.ElementType.Air || basic.element == Basic.ElementType.Earth ||  basic.element == Basic.ElementType.All){
            ranged.enabled = !ranged.enabled;
            if(rangedAI){
                rangedAI.enabled = !rangedAI.enabled;
            }            
        }
        else if(basic.element == Basic.ElementType.Fire || basic.element == Basic.ElementType.Water){
            physical.enabled = !physical.enabled;
        }        

    }

    // Update is called once per frame
    void Update()
    {      
        processSpawns();
        processAbility();
    }
    private void processAbility(){
        if(abilityTimer >= timeToUseAbility){
            switch (basic.element)
            {
                case Basic.ElementType.Air:
                    airBoss();
                    break;
                case Basic.ElementType.Earth:
                    earthBoss();
                    break;
                case Basic.ElementType.Fire:
                    fireBoss();
                    break;
                case Basic.ElementType.Water:
                    waterBoss();
                    break;
                case Basic.ElementType.All:
                    finalBoss();
                    break;
            }
            abilityTimer = 0;
        }
        abilityTimer += Time.deltaTime;
    }

    private void processSpawns(){        
        if(spawnTimer >= timeToSpawn){
            spawner.spawnEnemies(EnemiesToSpawn);
            spawnTimer = 0;
        }
        spawnTimer += Time.deltaTime;
    }

    public void death()
    {
        if(basic.element == Basic.ElementType.Fire){
            EventManager.TriggerEvent("Fire Boss Death");
            GameManager.FireBossDefeated = true;
        }
        if(basic.element == Basic.ElementType.Water){
            EventManager.TriggerEvent("Water Boss Death");
            GameManager.WaterBossDefeated = true;
        }
        if(basic.element == Basic.ElementType.Earth){
            EventManager.TriggerEvent("Earth Boss Death");
            GameManager.EarthBossDefeated = true;
        }
        if(basic.element == Basic.ElementType.Air){
            EventManager.TriggerEvent("Air Boss Death");
            GameManager.AirBossDefeated = true;
        }
        if (basic.element == Basic.ElementType.All){
            EventManager.TriggerEvent("Game Won");
        }
        GameManager.BossesDefeated++;
    }

    private void airBoss(){
        //dash
        physical.airBoss();
    }
    private void earthBoss(){
        //shield
        physical.earthBoss();
    }
    private void fireBoss(){
        //debuff
        ranged.fireBoss();
    }
    private void waterBoss(){
        //lifesteal
        ranged.waterBoss();
    }
    private void finalBoss(){
        //cycle

        switch (abilityRotation)
        {
            case 0:
                airBoss();
                break;
            case 1:
                earthBoss();
                switchAttacks();
                break;
            case 2:
                fireBoss();
                break;
            case 3:
                waterBoss();
                switchAttacks();
                break;
        }
        abilityRotation = (abilityRotation + 1) % basic.elementCount;
    }

    private void switchAttacks(){
        ranged.enabled = !ranged.enabled;
        physical.enabled = !physical.enabled;
        rangedAI.enabled = !rangedAI.enabled;
    }

    public void lifesteal(int amountToHeal){
        basic.hp += amountToHeal;
    }
}