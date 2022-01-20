using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Platform : MonoBehaviour
{
    GameObject LeftDoor;
    GameObject RightDoor;
    GameObject UpperDoor;
    GameObject LowerDoor;

    GameObject LeftTeleporter;
    GameObject RightTeleporter;
    GameObject UpperTeleporter;
    GameObject LowerTeleporter;
    
    GameObject airSwitch;
    GameObject earthSwitch;
    GameObject fireSwitch;
    GameObject waterSwitch;

    GameObject CentralTeleporter;
    [SerializeField] private Animator sceneTransitions;

    void Awake()
    {
        LeftDoor  = GameObject.Find("LeftDoor");
        RightDoor = GameObject.Find("RightDoor");
        UpperDoor = GameObject.Find("UpperDoor");
        LowerDoor = GameObject.Find("LowerDoor");
        LeftTeleporter  = GameObject.Find("LeftTeleporter");
        RightTeleporter = GameObject.Find("RightTeleporter");
        UpperTeleporter = GameObject.Find("UpperTeleporter");
        LowerTeleporter = GameObject.Find("LowerTeleporter");

        airSwitch = GameObject.Find("AirSwitch");
        earthSwitch = GameObject.Find("EarthSwitch");
        fireSwitch = GameObject.Find("FireSwitch");
        waterSwitch = GameObject.Find("WaterSwitch");

        CentralTeleporter = GameObject.Find("CenterTeleporter");

        CentralTeleporter.SetActive(false);
        LeftDoor.SetActive(false);
        LowerDoor.SetActive(false);
        RightDoor.SetActive(false);
        UpperDoor.SetActive(false);        
    }
    void Start()
    {
       StartCoroutine(fadeIn());
    }
    IEnumerator fadeIn(){
        sceneTransitions.SetInteger("Animation", 9);
        yield return new WaitForSeconds(0.45f);
        sceneTransitions.SetInteger("Animation", 0);
    }


    // Update is called once per frame
    void Update()
    {

    }

     // called first
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // CentralTeleporter.SetActive(true);
        if (GameManager.FireBossDefeated)
        {
            LeftDoor.SetActive(true);
            LeftTeleporter.SetActive(false);
            fireSwitch.GetComponent<SpriteRenderer>().color = Color.white;
        }
        if (GameManager.WaterBossDefeated)
        {
            LowerDoor.SetActive(true);
            LowerTeleporter.SetActive(false);
            waterSwitch.GetComponent<SpriteRenderer>().color = Color.white;
        }
        if (GameManager.EarthBossDefeated)
        {
            RightDoor.SetActive(true);
            RightTeleporter.SetActive(false);
            earthSwitch.GetComponent<SpriteRenderer>().color = Color.white;
        }
        if (GameManager.AirBossDefeated)
        {
            UpperDoor.SetActive(true);
            UpperTeleporter.SetActive(false);
            airSwitch.GetComponent<SpriteRenderer>().color = Color.white;
        }
        if(GameManager.FireBossDefeated && GameManager.AirBossDefeated && GameManager.WaterBossDefeated && GameManager.EarthBossDefeated)
        {
            CentralTeleporter.SetActive(true);
        }
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
