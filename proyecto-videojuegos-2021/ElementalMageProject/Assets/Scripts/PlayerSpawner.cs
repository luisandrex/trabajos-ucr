using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] GameObject NorthPosition;
    [SerializeField] GameObject SouthPosition;
    [SerializeField] GameObject EastPosition;
    [SerializeField] GameObject WestPosition;
    [SerializeField] GameObject player;

    [SerializeField] Animator sceneTransitions;

    void Start()
    {
        switch (GameManager.LastDirection){
            case Directions.UP:
                player.transform.position = NorthPosition.transform.position;
                break;
            case Directions.DOWN:
                player.transform.position = SouthPosition.transform.position;
                break;
            case Directions.RIGHT:
                player.transform.position = EastPosition.transform.position;
                break;
            case Directions.LEFT:
                player.transform.position = WestPosition.transform.position;
                break;
            default:
                break;                
        }
        StartCoroutine(animate());
    }

    IEnumerator animate(){
        switch (GameManager.LastDirection){
            case Directions.UP:
                sceneTransitions.SetInteger("Animation", 2);
                player.transform.position = NorthPosition.transform.position;
                break;
            case Directions.DOWN:
                sceneTransitions.SetInteger("Animation", 7);
                player.transform.position = SouthPosition.transform.position;
                break;
            case Directions.RIGHT:
                sceneTransitions.SetInteger("Animation", 5);
                player.transform.position = EastPosition.transform.position;
                break;
            case Directions.LEFT:
                sceneTransitions.SetInteger("Animation", 3);
                player.transform.position = WestPosition.transform.position;
                break;
            default:
                break;                
        }
        yield return new WaitForSeconds(0.5f);
        sceneTransitions.SetInteger("Animation", 0);
    }

}
