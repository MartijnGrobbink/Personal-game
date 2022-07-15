using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    WeaponDirection weaponDirection;
    public GameObject fireBolt;
    public bool trigger;
    Quaternion currentRotation;

    float x;
    float y;
    //finding weapon's direction
    void Start()
    {
        weaponDirection = GetComponent<WeaponDirection>();
    }

    //spawning firebolt in the weapons direction
    void Update()
    {
        if(weaponDirection.direction == WeaponDirection.Direction.Up)
       {
           currentRotation.eulerAngles = new Vector3(0, 0, 0);
           x = 0;
           y = 1;
       }
        if(weaponDirection.direction == WeaponDirection.Direction.Down)
       {
           currentRotation.eulerAngles = new Vector3(0, 0, 180);
           x = 0;
           y = -1;
       }
        if(weaponDirection.direction == WeaponDirection.Direction.Left)
       {
           currentRotation.eulerAngles = new Vector3(0, 0, 90);
           x = -1;
           y = 0;
       }
        if(weaponDirection.direction == WeaponDirection.Direction.Right)
       {
           currentRotation.eulerAngles = new Vector3(0, 0, 270);
           x = 1;
           y = 0;
       }
       //spawning object if trigger is true
       //trigger is in player input
        if(trigger == true)
        {
            Instantiate(fireBolt, transform.position + new Vector3(x, y, 0), currentRotation);
            trigger = false;
        }
    }
}
