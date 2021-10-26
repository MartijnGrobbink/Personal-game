using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
private RoomData data;
private MonsterManager monsterManager;

private GameObject monster;
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
        randX = Random.Range(-9, 9);
        randY = Random.Range(-9, 9);
        randMonster = Random.Range(0, monsterManager.level1Monsters.Length);
        monster = Instantiate(monsterManager.level1Monsters[randMonster], transform.position + new Vector3 (randX, randY , 0), Quaternion.identity);
        monster.transform.SetParent(this.transform, true);
        data.monsterlist.Add(monster);
        }
    }
}
