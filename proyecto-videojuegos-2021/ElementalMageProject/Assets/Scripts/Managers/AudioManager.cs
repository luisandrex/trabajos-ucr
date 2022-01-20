using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Sound{
    [Header("General")]
    public string Name;
    public AudioClip Clip;
    public AudioSource Source;

    [Header("Properties")]
    [Range(0f,1f)]
    public float Volume = 0.5f;

    [Range(0f,2f)]
    public float Pitch = 1f;

    public bool Loop = false;

    public void SetSource(AudioSource _source){
        Source = _source;
        Source.clip = Clip;
    }

    public void Play(int i){
        Source.pitch = Pitch;
        if(i == 0){
            Source.volume = Volume * AudioManager.instance.SFXVolume;
        }
        else{
            Source.volume = Volume * AudioManager.instance.musicVolume;
        }
        Source.loop = Loop;
        Source.Play();
    }

    public void Stop(){
        Source.Stop();
    }
}


public class AudioManager : MonoBehaviour
{
   public static AudioManager instance;
   [Header("Sound effects")] 
   [SerializeField] private Sound[] Sfx = new Sound [0];
   
   [Header("Enemies")]
   [SerializeField] private Sound[] EnemyHurt= new Sound [0];

   [SerializeField] private Sound[] EnemyDied= new Sound [0];
   [Header("Player")]
   [SerializeField] private Sound[] PlayerHurt= new Sound [0];
   [SerializeField] private Sound[] PlayerDied= new Sound [0];

   [Header("Music")] 

   [SerializeField] private Sound[] Music= new Sound [0]; 

    [SerializeField] private List<string> sceneNames;

    private static string previousScene = "MainMenu";
    private static string currentScene = "MainMenu";

    private string musicBeingPlayed = "Hub";

    public float musicVolume = 1f;
    public float SFXVolume = 1f;

   void Awake(){

       if(instance == null){
           instance = this;
           GameObject.DontDestroyOnLoad(this);
        }
       else if(instance != this){
           Destroy(gameObject);
        }
        setSounds(EnemyDied);
        setSounds(EnemyHurt);
        setSounds(PlayerDied);
        setSounds(PlayerHurt);
        setSounds(Sfx);
        setSounds(Music);    
        if(currentScene == "MainMenu" && previousScene == "MainMenu"){
            PlayMusic("Hub");
        }
   }

    private void setSounds(Sound [] sound){
        for(int i = 0; i < sound.Length; i++){
            GameObject _go = new GameObject("SFX_" + i + "_" + sound[i].Name);
            _go.transform.parent = transform;
            sound[i].SetSource(_go.AddComponent<AudioSource>());
        }
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        currentScene = scene.name;
        if(!sceneNames.Contains(previousScene) || !sceneNames.Contains(currentScene)){
            if(previousScene == "Hub" ){
                if(previousScene != currentScene.Substring(0,6) && !sceneNames.Contains(currentScene)){
                    changeAreaMusic(scene);
                }
            }
            else if (currentScene == "Hub"){
                if(previousScene.Substring(0,6) != currentScene && !sceneNames.Contains(previousScene)){
                    changeAreaMusic(scene);
                }
            }
            else if(previousScene.Substring(0,6) != currentScene.Substring(0,6)){
                changeAreaMusic(scene);
            }
        }
    }

    void changeAreaMusic(Scene scene){
        preStopMusic(previousScene);            
        previousScene = currentScene;
        if(sceneNames.Contains(scene.name)){
            PlayMusic("Hub");
        }
        else if(scene.name.Contains("Boss")){
            switch (scene.name)
            {
                case "AirBossRoom":
                    PlayMusic("Boss1");
                    break;
                case "EarthBossRoom":
                    PlayMusic("Boss2");
                    break;
                case "FireBossRoom":
                    PlayMusic("Boss3");
                    break;
                case "WaterBossRoom":
                    PlayMusic("Boss4");
                    break;
                case "FinalBoss":
                    PlayMusic("FinalBoss");
                    break;
            }
        }
        else {
            PlayMusic("AllAreas");
        }
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

   public void PlaySfx(string name){
       Sound audio = null;
       switch (name){
           case "EnemyHurt":
               audio = SearchSound(name, EnemyHurt);
               break;
           case "EnemyDied":
               audio = SearchSound(name, EnemyDied);
               break;
           case "PlayerHurt":
               audio = SearchSound(name, PlayerHurt);
               break;
           case "PlayerDied":
               audio = SearchSound(name, PlayerDied);
               break;
           case "WonScreen":
               audio = SearchSound(name, Sfx);
               break;
       }
       if(audio != null){
           audio.Play(0);
       }
   }
   public void PlayMusic(string name){
        Sound audio = SearchSound(name, Music);
        if(audio != null){
           audio.Play(1);
        }
   }
   public void preStopMusic(string name){
        if(sceneNames.Contains(name)){
        // if(name == "Hub" || name == "MainMenu" || name == "DeathScreen" || name == "WonScreen" || name == "BuyMenu" || name == "CreditsScreen" && name == "GameWonScreen" && name == "HowToPlayScreen"){
            StopMusic("Hub");
        }
        else if(name.Contains("Boss")){
            switch (name)
            {
                case "AirBossRoom":
                    StopMusic("Boss1");
                    break;
                case "EarthBossRoom":
                    StopMusic("Boss2");
                    break;
                case "FireBossRoom":
                    StopMusic("Boss3");
                    break;
                case "WaterBossRoom":
                    StopMusic("Boss4");
                    break;
                case "FinalBoss":
                    StopMusic("FinalBoss");
                    break;
            }
        }
        else {
            StopMusic("AllAreas");
        }
   }

   public void StopMusic(string name){
        Sound audio = SearchSound(name, Music);
        if(audio != null){
           audio.Stop();
        }
   }

   private Sound SearchSound(string name, Sound [] audio){
        if(audio == EnemyDied || audio == EnemyHurt ||audio == PlayerHurt ||audio == PlayerDied){
            int i = Random.Range(0,audio.Length);
            return audio[i];
        }
        else{
            for(int i = 0 ; i < audio.Length; i++){
                if(audio[i].Name == name){
                    musicBeingPlayed = audio[i].Name;
                    return audio[i];
                }
            }
        }
       Debug.LogError("AudioManager: audio" + name + " not found");
       return null;
   }

   public void changeMusicVolume(){
       SearchSound(musicBeingPlayed, Music).Source.volume = musicVolume;
   }
}
