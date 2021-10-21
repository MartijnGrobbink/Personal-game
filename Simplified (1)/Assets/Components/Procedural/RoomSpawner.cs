using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;
    //1 need bottom door
    //2 need top door
    //3 need left door
    //4 need right door
    //5 spawn
    private RoomTemplates templates;
    private int rand;
    [SerializeField]
    private bool spawned = false;

    void Start(){

        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.1f);
        //Delete spawnpoint after 4 seconds except for spawn
        if(openingDirection != 5){
        Invoke("Destroy", 4f);
        }
    }
    void Spawn(){
        // check the opening direction the spawnpoint has and spawn a room with the rules of that spawnpoint
        if(spawned == false){
            if(openingDirection == 1){
                // Need to spawn a room with BOTTOM door
                rand = Random.Range(0, templates.bottomRooms.Length);
                Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation);
            } else if(openingDirection == 2){
                // Need to spawn a room with TOP door
                rand = Random.Range(0, templates.topRooms.Length);
                Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation);
            } else if(openingDirection == 3){
                // Need to spawn a room with LEFT door 
                rand = Random.Range(0, templates.leftRooms.Length);
                Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation);           
            } else if(openingDirection == 4){
                // Need to spawn a room with RIGHT door   
                rand = Random.Range(0, templates.rightRooms.Length);
                Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation);     
            }
            spawned = true; 
        } 
    }

    void Update(){
    //create spawn check if there is a reset if so then reset the spawned to false
    //this is so after an reset the spawn can be created again if this is not done spawn will stay true and no new dungeon will be generated
    if(openingDirection == 5){
        if(spawned == false){
        Instantiate(templates.startRoom[0], transform.position, templates.startRoom[0].transform.rotation);
        spawned = true;
        }
            else if(templates.reset == true){
                spawned = false;
            }
        }
    }
    //check if the spot is occupied if so delete the spawnpoint before spawing a room
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("SpawnPoint") && other.GetComponent<RoomSpawner>().spawned == true){
            Destroy(gameObject);
        }
    }
    void Destroy(){
        Destroy(gameObject);
    }
}