using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
//used to get data and sent data
private RoomData data;
private MonsterManager monsterManager;
//used for setting the gameobject to this and then to set it's parent to the room
private GameObject monster;
//random variables
private float randX;
private float randY;
private float randAmount;
private int randMonster;

    void Start(){
        Invoke("MonsterSpawning", 0.4f);
        data = this.GetComponent<RoomData>();
        monsterManager = GameObject.FindGameObjectWithTag("MonsterManager").GetComponent<MonsterManager>();  
        randAmount = Random.Range(3, 5);
    }

    void MonsterSpawning(){
        for(int i = 0; i < randAmount; i++){
            //setting rand spawn variables -9 and 9 in randx, randy is the room size
            randX = Random.Range(-9, 9);
            randY = Random.Range(-9, 9);
            randMonster = Random.Range(0, monsterManager.level1Monsters.Length);
            //spawning setting parent and adding to the rooms monster list
            monster = Instantiate(monsterManager.level1Monsters[randMonster], transform.position + new Vector3 (randX, randY , 0), Quaternion.identity);
            monster.transform.SetParent(transform, true);
            data.monsterlist.Add(monster);
        }
    }
}
