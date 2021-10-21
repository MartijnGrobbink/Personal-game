using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewZombie : MonoBehaviour
{
    private FieldOfView fov;
    private Movement movement;

    Vector3 targetPosition;
    // Start is called before the first frame update
    void Start()
    {
        fov = this.GetComponent<FieldOfView>();
        movement = GetComponent<Movement>();
        movement.SpeedModifier = 1;
        StartCoroutine(SearchRoutine());
        targetPosition = transform.position;
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
    void Update(){
        Move();

    }
    void SeenPlayer(){
        //move to the position where you have seen the player
        Vector3 playerPosition = fov.playerRef.transform.position;

        if(fov.canSeePlayer == true){
            targetPosition = playerPosition;
        }    
    }
    void Move(){
        transform.position = Vector3.MoveTowards(transform.position, targetPosition,  2 * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPosition) < 0.001f){
            RandomMove();
        }    
    }

    void RandomMove(){
        float randomTime;
        float x = 0;
        float y = 0;
        randomTime = Random.Range(0,3);
        if(x == 0 && y == 0){
            x = Random.Range(-1,1);
            y = Random.Range(-1,1);
        }
    }
}
