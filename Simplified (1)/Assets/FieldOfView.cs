using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
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
        Collider2D rangeChecks = Physics2D.OverlapCircle(transform.position, radius, targetMask);

        if(rangeChecks == true)
        {
            Transform target = rangeChecks.transform;
            Vector2 directionToTarget = (target.position - transform.position).normalized;

            if(Vector2.Angle(newEnemyInput.EnemyDirection, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);

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
