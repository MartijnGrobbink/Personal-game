using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewZombie : MonoBehaviour
{
    private FieldOfView fov;
    private Movement movement;

    Vector3 targetPosition;
    Vector3 lastPosition;

    float randomX;
    float randomY;
    float randomTime;
    float delayTime;

    bool stop = true;
    bool atSeenLocation = true;

    void Start()
    {
        fov = GetComponent<FieldOfView>();
        movement = GetComponent<Movement>();

        targetPosition = transform.position;
        movement.SpeedModifier = 2;

        StartCoroutine(SearchRoutine());
    }

        private IEnumerator SearchRoutine()
    {
        float delay = 0.2f;
        WaitForSeconds wait = new WaitForSeconds(delay);

        while(true)
        {
            yield return wait;
            PlayerCheck();
        }
    }

    void PlayerCheck()
    {
        SeenPlayer();        
    }

    void Update()
    {
        Move(); 
    }

    void SeenPlayer()
    {
        //move to the position where you have seen the player
        Vector3 playerPosition = fov.playerRef.transform.position;

        if(fov.canSeePlayer == true)
        {
            targetPosition = playerPosition;  
            atSeenLocation = false; 
        }  
    }

    void Move()
    {
        if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
        {
            atSeenLocation = true;
            RandomMove();
        }   
        else if(atSeenLocation == false) {
            movement.Move(new Vector3(0,0));
            transform.position = Vector3.MoveTowards(transform.position, targetPosition,  2 * Time.deltaTime);
        } 
        if(atSeenLocation == true){
            //search void
            RandomMove();
        }
    }

    void RandomMove()
    {
        if(randomX == 0 && randomY == 0 || randomX != 0 && randomY != 0 || stop == true)
        {
            randomTime = Random.Range(2,4);
            randomX = Random.Range(-1,2);
            randomY = Random.Range(-1,2);
            stop = false;
        } 

        if(randomTime >= 0)
        {
        movement.Move(new Vector3(randomX, randomY));
        }

        if(randomTime <= 0) 
        {
            stop = true;
            movement.Move(new Vector3(0,0));
        }
        if(delayTime >= 0.2f)
        {
            lastPosition = transform.position;
        } 
        else if(delayTime <= 0.1f)
        {
            delayTime = 0.3f;
            if(Vector3.Distance(transform.position, lastPosition) < 0.001f){
                stop = true;
            }
        }
        randomTime -= Time.deltaTime;
        delayTime -= Time.deltaTime;
    }
}