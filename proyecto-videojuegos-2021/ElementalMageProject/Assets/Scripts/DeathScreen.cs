using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    private float timer;

    [SerializeField] private float deathTimer;

    [SerializeField] private Animator sceneTransitions;
    // Start is called before the first frame update
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
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= deathTimer){
            GetComponent<MapChanger>().backToHub();
        }
    }
}
