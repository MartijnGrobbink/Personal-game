using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBolt : MonoBehaviour
{
    Vector3 shootDir;
    WeaponDirection weaponDirection;
    public float fireBoltSpeed = 5;
    public float damage = 10;
    // check weapons direction and move in that direction
    void Start()
    {
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        weaponDirection = player[0].GetComponent<WeaponDirection>();

        if(weaponDirection.direction == WeaponDirection.Direction.Up)
       {
           shootDir = new Vector3(0,1);
       }
        if(weaponDirection.direction == WeaponDirection.Direction.Down)
       {
           shootDir = new Vector3(0,-1);
       }
        if(weaponDirection.direction == WeaponDirection.Direction.Left)
       {
           shootDir = new Vector3(-1,0);
       }
        if(weaponDirection.direction == WeaponDirection.Direction.Right)
       {
           shootDir = new Vector3(1,0);
       }
    }

    //movement
    void Update()
    {
        transform.position += shootDir * fireBoltSpeed * Time.deltaTime;
    }
    //check if flammable set on fire if zombie do damage
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Flammable")
        {
            Fire fire = other.gameObject.GetComponent<Fire>();
            fire.FireContact = true;
        } 
        else if(other.gameObject.tag == "Zombie")
        {
            NewHealth newHealth = other.gameObject.GetComponent<NewHealth>();
            newHealth.damage = damage;
        }
        //remove when you hit something for optimisation
        Destroy(gameObject);
    }
}
