using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;
    public GameObject[] startRoom;

    public List<GameObject> rooms;

    public float waitTime;
    private bool spawnedBoss;
    public GameObject boss;
    public float minRooms;
    public bool reset;

    int lastRoomLength;

    void Update(){
        if(waitTime <= 0 && reset == false){
            if(rooms.Count <= minRooms){
                reset = true;
            }
        } 
        else if(reset == true){
            for(int i = 0; 0 < rooms.Count;){
                Destroy(rooms[i].gameObject);
                rooms.Remove(rooms[i].gameObject);
            } 
            reset = false;
            waitTime = 2;
            }else{ waitTime -= Time.deltaTime;}
    }  
}