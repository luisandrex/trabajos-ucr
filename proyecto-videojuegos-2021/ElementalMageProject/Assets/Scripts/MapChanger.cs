using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Directions
{
    NONE,
    UP,
    DOWN,
    LEFT,
    RIGHT
}

public class MapChanger : MonoBehaviour
{
    [SerializeField] string sceneToLoad = "";
    [SerializeField] Directions playerSpawnDir;

    public Animator sceneTransitions;

    IEnumerator changeScene()
    {
        switch (playerSpawnDir)
        {
            case Directions.RIGHT:
                sceneTransitions.SetInteger("Animation", 6);
                break;
            case Directions.LEFT:
                sceneTransitions.SetInteger("Animation", 4);
                break;
            case Directions.DOWN:
                sceneTransitions.SetInteger("Animation", 8);
                break;
            case Directions.UP:
                sceneTransitions.SetInteger("Animation", 1);
                break;
        }
        yield return new WaitForSeconds(0.4f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
    }

    public void deadBoss(){
        Destroy(GameObject.Find("UI"));
        // Destroy(FindObjectOfType<UIManager>().gameObject);
        GameManager.PlayerHealth = GameManager.MaxPlayerHealth;
        StartCoroutine(fadeIn(1));
    }
    public void backToHub(){
        StartCoroutine(fadeIn(2));
    }

    public void playerDied(){
        Destroy(GameObject.Find("UI"));
        // Destroy(FindObjectOfType<UIManager>().gameObject);
        GameManager.PlayerHealth =  GameManager.MaxPlayerHealth;
        StartCoroutine(fadeIn(3));
    }

    public void gameWon(){
        Destroy(GameObject.Find("UI"));
        // Destroy(FindObjectOfType<UIManager>().gameObject);
        GameManager.PlayerHealth = GameManager.MaxPlayerHealth;
        StartCoroutine(fadeIn(4));        
    }

    public void backToMainMenu(){
        GameManager.PlayerHealth = GameManager.MaxPlayerHealth;
        StartCoroutine(fadeIn(5));        
    }

    public void toCredits(){
        StartCoroutine(fadeIn(6));        
    }
    public void howToPlay(){
        StartCoroutine(fadeIn(7));        
    }

    public void exit(){
        StartCoroutine(fadeIn(8));        
    }

    IEnumerator fadeIn(int scene){
        sceneTransitions.SetInteger("Animation", 10);
        yield return new WaitForSeconds(0.4f);
        switch (scene)
        {
            case 1:
                UnityEngine.SceneManagement.SceneManager.LoadScene("BuyMenu");
                break;
            case 2:
                UnityEngine.SceneManagement.SceneManager.LoadScene("Hub");
                break;
            case 3:
                UnityEngine.SceneManagement.SceneManager.LoadScene("DeathScreen");
                break;
            case 4:
                UnityEngine.SceneManagement.SceneManager.LoadScene("WonScreen");
                break;
            case 5:
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
                break;
            case 6:
                UnityEngine.SceneManagement.SceneManager.LoadScene("CreditsScreen");
                break;
            case 7:
                UnityEngine.SceneManagement.SceneManager.LoadScene("HowToPlayScreen");
                break;
            case 8:
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #else
                    Application.Quit();
                #endif
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            GameManager.LastDirection = playerSpawnDir;
            StartCoroutine(changeScene());
        }
    }
}
