using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    //sources
    //part 1
    //https://www.youtube.com/watch?v=qAf9axsyijY
    //part 2
    //https://www.youtube.com/watch?v=eR74EjkA_4s
    //part 3
    //https://www.youtube.com/watch?v=CUdKdHmT8xA
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;
    public GameObject startRoom;

    public List<GameObject> rooms;

    public float waitTime = 2;
    //private bool spawnedBoss;
    private GameObject spawnedBoss;
    public GameObject boss;
    //------------------------------------------------------------------------------OWN CODE---------------------------------------------------------------------
    public float minRooms;

    //setting up test variable
    int lengthRoomSample;
    //setting up a bool to see if the generation is done
    public bool roomsGenerated;

    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("SpawnPoint").Length == 0 && roomsGenerated == false)
        {
            Instantiate(startRoom, transform.position, Quaternion.identity);
            StartCoroutine(RoomCheck());
        }

        if (roomsGenerated == true)
        {
            StopCoroutine(RoomCheck());

            //when generation is done check if the amount of rooms hit the required amount
            if (minRooms >= rooms.Count)
            {
                ResetRooms();
            }
            //----------------------------------------------------------------BOSS---------------------------------------------------------------------------------
            else if (spawnedBoss == null)
            {
                SpawnBoss();
            }
            //------------------------------------------------------------------------------------------------------------------------------------------------------
        }
    }

    //Spawns boss
    private void SpawnBoss()
    {
        //we go through the list backwards so we start at the latest generated room and go back to find a room that is not an hallway
        spawnedBoss = Instantiate(boss, rooms[rooms.Count - 1].transform.position, Quaternion.identity);
    }

    //Resets the rooms
    private void ResetRooms()
    {
        for (int i = 0; 0 < rooms.Count;)
        {
            Destroy(rooms[i].gameObject);
            rooms.Remove(rooms[i].gameObject);
        }
        roomsGenerated = false;
        if (spawnedBoss != null)
            Destroy(spawnedBoss);
    }

    //Check if the room is being generated
    IEnumerator RoomCheck()
    {
        while (true)
        {
            lengthRoomSample = rooms.Count;

            yield return new WaitForSeconds(1);

            if (lengthRoomSample == rooms.Count)
            {
                lengthRoomSample = 0;
                roomsGenerated = true;
            }
        }
    }
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------- 
}