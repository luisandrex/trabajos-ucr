using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colliders : MonoBehaviour
{
    public enum ObstacleCollisionDirection {Left, Up, Right, Down };

    [SerializeField] public Dictionary<ObstacleCollisionDirection, List<string>> collisions = new Dictionary<ObstacleCollisionDirection, List<string>>();

    // Start is called before the first frame update
    void Awake()
    {
        collisions.Add(ObstacleCollisionDirection.Right, new List<string>());
        collisions.Add(ObstacleCollisionDirection.Left, new List<string>());
        collisions.Add(ObstacleCollisionDirection.Up, new List<string>());
        collisions.Add(ObstacleCollisionDirection.Down, new List<string>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other){
        Obstacle obstacle = other.gameObject.GetComponent<Obstacle>();
        if(!obstacle || obstacle.obstacleType == Obstacle.ObstacleType.Nonwalkable){
            if(other.contacts.Length == 1){
                if(other.contacts[0].point.x >= transform.position.x){// horizontal collision
                    collisions[ObstacleCollisionDirection.Right].Add(other.gameObject.name);
                    if(other.contacts[0].point.y >= gameObject.transform.position.y){//right side
                        collisions[ObstacleCollisionDirection.Up].Add(other.gameObject.name);
                    }
                    else{// left side
                        collisions[ObstacleCollisionDirection.Down].Add(other.gameObject.name);
                    }
                }
                else{
                    collisions[ObstacleCollisionDirection.Left].Add(other.gameObject.name);
                    if(other.contacts[0].point.y >= gameObject.transform.position.y){//right side
                        collisions[ObstacleCollisionDirection.Up].Add(other.gameObject.name);
                    }
                    else{// down side
                        collisions[ObstacleCollisionDirection.Down].Add(other.gameObject.name);
                    }
                }
            }
            else if(other.contacts.Length == 2){
                if(other.contacts[0].point.x == other.contacts[1].point.x){// horizontal collision
                    if(other.contacts[0].point.x > gameObject.transform.position.x){//right side
                        collisions[ObstacleCollisionDirection.Left].Add(other.gameObject.name);
                    }
                    else{// left side
                        collisions[ObstacleCollisionDirection.Right].Add(other.gameObject.name);
                    }
                }
                else{
                    if(other.contacts[0].point.y > gameObject.transform.position.y){//up side
                        collisions[ObstacleCollisionDirection.Down].Add(other.gameObject.name);
                    }
                    else{// down side
                        collisions[ObstacleCollisionDirection.Up].Add(other.gameObject.name);
                    }
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other){
        Obstacle obstacle = other.gameObject.GetComponent<Obstacle>();
        if(!obstacle || obstacle.obstacleType == Obstacle.ObstacleType.Nonwalkable){
            if(collisions[ObstacleCollisionDirection.Right].Contains(other.gameObject.name)){
                collisions[ObstacleCollisionDirection.Right].Remove(other.gameObject.name);
            }
            if(collisions[ObstacleCollisionDirection.Left].Contains(other.gameObject.name)){
                collisions[ObstacleCollisionDirection.Left].Remove(other.gameObject.name);
            }
            if(collisions[ObstacleCollisionDirection.Up].Contains(other.gameObject.name)){
                collisions[ObstacleCollisionDirection.Up].Remove(other.gameObject.name);
            }
            if(collisions[ObstacleCollisionDirection.Down].Contains(other.gameObject.name)){// left side
                collisions[ObstacleCollisionDirection.Down].Remove(other.gameObject.name);
            }
        }
    }
}
