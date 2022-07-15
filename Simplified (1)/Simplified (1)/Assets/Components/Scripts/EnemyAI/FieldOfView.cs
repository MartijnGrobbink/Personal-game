using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    //sources
    //https://www.youtube.com/watch?v=j1-OyLo77ss
    public float radius;
    [Range(0,360)]
    public float angle;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public NewEnemyInput newEnemyInput;
    public Vector3 ZombieDirection;
    public bool canSeePlayer;
    // Start is called before the first frame update
    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        newEnemyInput = GetComponent<NewEnemyInput>();

        StartCoroutine(FOVRoutine());
    }
    void Update()
    {
        ZombieDirection = newEnemyInput.EnemyDirection; 
    }
    //core routing for each 0.2f
    private IEnumerator FOVRoutine()
    {
        float delay = 0.2f;
        WaitForSeconds wait = new WaitForSeconds(delay);

        while(true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }
    
    private void FieldOfViewCheck()
    {
        //look if target is with in circle range
        Collider2D rangeChecks = Physics2D.OverlapCircle(transform.position, radius, targetMask);
        //target is in circle
        if(rangeChecks == true)
        {
            //set the target to the taget in the circle
            Transform target = rangeChecks.transform;
            //look difference in location from out target
            Vector2 directionToTarget = (target.position - transform.position).normalized;
            //look if the difference is within the triangle 
            //angle is defided by 2 because half of the triange is positive and the other half is negative
            if(Vector2.Angle(newEnemyInput.EnemyDirection, directionToTarget) < angle / 2)
            {
                //get the distance of the target
                float distanceToTarget = Vector2.Distance(transform.position, target.position);
                //look if there is no obstuction
                if(!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    canSeePlayer = true;
                else 
                    canSeePlayer = false; 
            }
            else
            canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }
}
