using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Animator sceneTransitions;
    void Start()
    {
        StartCoroutine(fadeOut());        
    }

    IEnumerator fadeOut(){
        sceneTransitions.SetInteger("Animation", 9);
        yield return new WaitForSeconds(0.44f);
        sceneTransitions.SetInteger("Animation", 0);
    }

    public void play(){
        GetComponent<MapChanger>().backToHub();
    }
    public void howToPlay(){
        GetComponent<MapChanger>().howToPlay();        
    }
    public void credits(){
        GetComponent<MapChanger>().toCredits();        
    }
    public void quitGame(){
        GetComponent<MapChanger>().exit();
    }
}
