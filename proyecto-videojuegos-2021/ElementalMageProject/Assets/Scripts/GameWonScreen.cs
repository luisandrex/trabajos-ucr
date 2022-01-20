using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWonScreen : MonoBehaviour
{
    private float timer;

    [SerializeField] private float gameWonTimer;
    // Start is called before the first frame update
    [SerializeField] private Animator sceneTransitions;
    void Start()
    {
        AudioManager.instance.PlaySfx("WonScreen");
        StartCoroutine(fadeOut());        
    }

    IEnumerator fadeOut(){
        sceneTransitions.SetInteger("Animation", 9);
        yield return new WaitForSeconds(0.44f);
        sceneTransitions.SetInteger("Animation", 0);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= gameWonTimer){
            GetComponent<MapChanger>().toCredits();
        }
    }
}
