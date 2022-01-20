using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    // [SerializeField] Text health;
    [SerializeField] GameObject debuff;
    [SerializeField] GameObject dash;
    [SerializeField] GameObject shield;
    [SerializeField] GameObject lifesteal;

    [SerializeField] GameObject container;
    [SerializeField] Button btnResume, btnExit;
    [SerializeField] Image debuffCooldown;
    [SerializeField] Image dashCooldown;
    [SerializeField] Image shieldCooldown;
    [SerializeField] Image lifestealCooldown;
    [SerializeField] GameObject firstOption;
    private EventSystem eventSystem;

    [SerializeField] static Image _healthBarImage;



    private bool canDebuff = false;
    private bool canDash = false;
    private bool canShield = false;
    private bool canLifesteal = false;
    public bool inMenu = false;

    // Start is called before the first frame update

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        pauseOff();
    }

    void Start()
    {
        _healthBarImage = GameObject.Find("UI/Canvas/_Health").GetComponent<Image>();
        // health.text = "HP: " + GameManager.PlayerHealth;
        debuff.SetActive(GameManager.DebuffUnlocked);
        dash.SetActive(GameManager.DashUnlocked);
        shield.SetActive(GameManager.ShieldUnclocked);
        lifesteal.SetActive(GameManager.LifestealUnlocked);
        debuffCooldown.color = Color.white;
        dashCooldown.color = Color.white;
        shieldCooldown.color = Color.white;
        lifestealCooldown.color = Color.white;

        btnExit.onClick.AddListener(delegate{
                                            exit();
                                            });

        btnResume.onClick.AddListener(delegate{
                                            resume();
                                            });

    }

    private void FixedUpdate()
    {
        _healthBarImage.fillAmount = GameManager.PlayerHealth/GameManager.MaxPlayerHealth;

        if(_healthBarImage.fillAmount <= 0.2f)
        {
            SetHealthBarColor(Color.red);
        }
        else if(_healthBarImage.fillAmount <= 0.5f)
        {
            SetHealthBarColor(Color.yellow);
        }
        else
        {
            SetHealthBarColor(Color.green);
        }
    }

    public static void SetHealthBarColor(Color healthColor)
    {
        _healthBarImage.color = healthColor;
    }

    public void manageDebuff(bool isReady)
    {
        if (isReady == canDebuff)
            return;

        if (isReady)
        {
            debuffCooldown.color = Color.white;
            canDebuff = true;
        }
        else
        {
            debuffCooldown.color = Color.gray;
            canDebuff = false;
        }
    }

    public void manageDash(bool isReady)
    {
        if (isReady == canDash)
            return;

        if (isReady)
        {
            dashCooldown.color = Color.white;
            canDash = true;
        }
        else
        {
            dashCooldown.color = Color.gray;
            canDash = false;
        }
    }

    public void manageShield(bool isReady)
    {
        if (isReady == canShield)
            return;

        if (isReady)
        {
            shieldCooldown.color = Color.white;
            canShield = true;
        }
        else
        {
            shieldCooldown.color = Color.gray;
            canShield = false;
        }
    }

    public void manageLifesteal(bool isReady)
    {
        if (isReady == canLifesteal)
            return;

        if (isReady)
        {
            lifestealCooldown.color = Color.white;
            canLifesteal = true;
        }
        else
        {
            lifestealCooldown.color = Color.gray;
            canLifesteal = false;
        }
    }

    public void menuManager()
    {
        Time.timeScale = Time.timeScale==1?0:1;
        inMenu = !inMenu;

        if (Time.timeScale == 1)
            pauseOff();
        else
            pauseOn();
    }
    public void pauseOff()
    {
        container.SetActive(false);
    }

    public void pauseOn()
    {
        container.SetActive(true);
        eventSystem = FindObjectOfType<EventSystem>();
        if (eventSystem.gameObject.scene.IsValid()){
            eventSystem.SetSelectedGameObject(firstOption);
        }


    }

    public void resume()
    {
        menuManager();
    }

    public void exit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif   
    }

    public void setMusicVolume(){
        AudioManager.instance.musicVolume = GameObject.Find("Music").GetComponent<Slider>().value;
        AudioManager.instance.changeMusicVolume();
    }

    public void setSFXVolume(){
        AudioManager.instance.SFXVolume = GameObject.Find("SFX").GetComponent<Slider>().value;
    }

}
