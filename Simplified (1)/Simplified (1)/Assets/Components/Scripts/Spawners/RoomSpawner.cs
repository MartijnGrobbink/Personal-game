using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    //sources
    //part 1
    //https://www.youtube.com/watch?v=qAf9axsyijY
    //part 2
    //https://www.youtube.com/watch?v=eR74EjkA_4s
    //part 3
    //https://www.youtube.com/watch?v=CUdKdHmT8xA

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
    //checking if the generation is done than remove the roomspawners as they are not needed any more
    void Update(){
        if(templates.roomsGenerated == true)
        Destroy(gameObject);
    }
    //check if the spot is occupied if so delete the spawnpoint before spawing a room
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("SpawnPoint") && other.GetComponent<RoomSpawner>().spawned == true){
            Destroy(gameObject);
        }
    }
}