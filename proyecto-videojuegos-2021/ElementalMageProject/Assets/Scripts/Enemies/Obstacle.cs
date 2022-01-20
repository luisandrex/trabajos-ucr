using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] public float effect = 1;
    [SerializeField] public ObstacleType obstacleType;

    public enum ObstacleType { Walkable, Nonwalkable };


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other){        
        if(obstacleType == ObstacleType.Walkable){
            if(other.gameObject.TryGetComponent<Player>(out Player player)){
                player.applySlow(effect);
            }
            else if(other.gameObject.TryGetComponent<Basic>(out Basic basic)){
                basic.applySlow(effect);
            }
        }
        else{
            if(other.gameObject.TryGetComponent<Player>(out Player player)){
                if(effect != 0){
                    player.takeDamage(effect);
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other){
        if(obstacleType == ObstacleType.Walkable){
            if(other.gameObject.TryGetComponent<Player>(out Player player)){
                player.removeSlow();
            }
            else if(other.gameObject.TryGetComponent<Basic>(out Basic basic)){
                basic.removeSlow();
            }
        }
    }

}
