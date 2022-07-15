using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewZombie : MonoBehaviour
{
    private FieldOfView fov;
    private Movement movement;

    public GameObject[] symbols;
    GameObject zombieSymbol;

    Vector3 targetPosition;
    Vector3 lastPosition;

    Vector3 playerPosition;

    float randomX;
    float randomY;
    float randomTime;
    float delayTime;

    public float damage = 20;

    bool stop = true;
    bool atSeenLocation = true;

    void Start()
    {
        //setting components
        fov = GetComponent<FieldOfView>();
        movement = GetComponent<Movement>();
        //is so that the zombie doesn't walk to position (0,0,0)
        targetPosition = transform.position;
        movement.SpeedModifier = 2;
        //start the SearchRoutine
        StartCoroutine(SearchRoutine());
    }
        //for every 0.2f check for the player
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
        if(fov.playerRef != null)
        {
            playerPosition = fov.playerRef.transform.position;
        }
        //if the zombie can see the player set the target position to the player position
        //also show the attack symbol
        if(fov.canSeePlayer == true)
        {
            symbols[0].SetActive(true);
            symbols[1].SetActive(false);

            targetPosition = playerPosition;  
            atSeenLocation = false; 
        }  
        //cannot see the player and is not at the last see location of the player show confusion symbol
        else if (fov.canSeePlayer == false && atSeenLocation == false)
        {
            symbols[0].SetActive(false);
            symbols[1].SetActive(true);
        }
    }
    //movement of the zombie when seen the player
    void Move()
    {
        //at the last seen location of the player go back to random movement
        //NOTE for improvement is was intended to have the zombie look around and the go back to random movement
        if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
        {
            atSeenLocation = true;
        }   
        //if the zombie is not at the last seen location move to that location
        //NOTE for further improvent i have to rework this so it uses the movement script so the animation isn't bugged
        else if(atSeenLocation == false) {
            movement.Move(new Vector3(0,0));
            transform.position = Vector3.MoveTowards(transform.position, targetPosition,  3 * Time.deltaTime);
        } 
        if(atSeenLocation == true){
            RandomMove();
        }
    }

    void RandomMove()
    {
        //Walk in a random staight direction
        if(randomX == 0 && randomY == 0 || randomX != 0 && randomY != 0 || stop == true)
        {
            randomTime = Random.Range(2,4);
            randomX = Random.Range(-1,2);
            randomY = Random.Range(-1,2);
            stop = false;
        //show no symbol
            symbols[0].SetActive(false);
            symbols[1].SetActive(false);
        } 
        //set a random duration to walk in that direction
        if(randomTime >= 0)
        {
        movement.Move(new Vector3(randomX, randomY));
        }
        else 
        {
            stop = true;
            movement.Move(new Vector3(0,0));
        }
        //Checking is the zombie is moving if not set stop true this will cause for the zombie to walk in a new direction
        //this is so that the zombie won't stay a long time walking into a wall
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
        //remove time from the timers
        randomTime -= Time.deltaTime;
        delayTime -= Time.deltaTime;
    }
    //if collided with the player deal damage to the player
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            NewHealth newHealth = other.gameObject.GetComponent<NewHealth>();
            newHealth.damage = damage;
        }
    }
}