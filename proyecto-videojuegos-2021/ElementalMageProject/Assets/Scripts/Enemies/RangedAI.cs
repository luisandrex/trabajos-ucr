using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class RangedAI : MonoBehaviour
{
    public Transform target;

    public float speed = 5f;
    public float nextWaypointDistance = 3f;
    [SerializeField] float radius = 5f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    private Colliders collisions;
    private Basic basicInfo;
    private Vector2 targetPosition;

    void Start()
    {
        target = GetComponent<Ranged>().target;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        basicInfo = this.GetComponent<Basic>();
        collisions = GetComponent<Colliders>();    
        InvokeRepeating("updatePath", 0f, .1f);
    }

    void updatePath()
    {
        if (seeker.IsDone())
        {
            updateTargetPostion();
            seeker.StartPath(rb.position, targetPosition, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    
    private void updateTargetPostion()
    {
        float distance = Vector2.Distance(this.transform.position, target.position);
        if (distance < radius)
        {
            targetPosition = Vector2.MoveTowards(
                this.transform.position,
                target.position,
                -1 * basicInfo.currentMovementSpeed  * (radius - distance)
            );
        }
        else
        {
            targetPosition = rb.position;
        }
    }

    void FixedUpdate()
    {

        if (target)
        {
            float distance = Vector2.Distance(this.transform.position, target.position);
            if (distance < radius)
            {

                if (path == null)
                {
                    return;
                }

                updateTargetPostion();
                if(currentWaypoint>= path.vectorPath.Count)
                {
                    reachedEndOfPath = true;
                    return;
                }
                else
                {
                    reachedEndOfPath = false;
                }

                var spd = basicInfo.movementSpeed * Time.fixedDeltaTime *(radius - distance);
                Vector2 newPosition = targetPosition = Vector2.MoveTowards(
                    this.transform.position,
                    (Vector2)path.vectorPath[currentWaypoint],
                    spd
                );

                newPosition.x = Mathf.Clamp(newPosition.x, -8.825f + (transform.localScale.x / 2), 8.825f - (transform.localScale.x / 2));
                newPosition.y = Mathf.Clamp(newPosition.y, -4.95f + (transform.localScale.y / 2), 4.95f - (transform.localScale.y / 2));
                if (collisions.collisions[Colliders.ObstacleCollisionDirection.Right].Count != 0)
                {

                    newPosition.x = Mathf.Clamp(newPosition.x, gameObject.transform.position.x, float.PositiveInfinity);
                }
                if (collisions.collisions[Colliders.ObstacleCollisionDirection.Left].Count != 0)
                {
                    newPosition.x = Mathf.Clamp(newPosition.x, float.NegativeInfinity, gameObject.transform.position.x);
                }
                else if (collisions.collisions[Colliders.ObstacleCollisionDirection.Up].Count != 0)
                {
                    newPosition.y = Mathf.Clamp(newPosition.y, gameObject.transform.position.y, float.PositiveInfinity);
                }
                else if (collisions.collisions[Colliders.ObstacleCollisionDirection.Down].Count != 0)
                {
                    newPosition.y = Mathf.Clamp(newPosition.y, float.NegativeInfinity, gameObject.transform.position.y);
                }
                rb.position = newPosition;
                float wayPointDistance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
                
                if(wayPointDistance < nextWaypointDistance)
                {
                    currentWaypoint++;
                }
            }
            
        }
    }

}
