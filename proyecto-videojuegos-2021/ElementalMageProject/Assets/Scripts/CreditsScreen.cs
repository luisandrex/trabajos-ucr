using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScreen : MonoBehaviour
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

    // Update is called once per frame

    public void backToMainMenu(){
        GetComponent<MapChanger>().backToMainMenu();
    }

    public void exit(){
        GetComponent<MapChanger>().exit();
    }


}
