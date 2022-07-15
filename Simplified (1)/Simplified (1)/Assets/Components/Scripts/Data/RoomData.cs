using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData : MonoBehaviour
{
    //save all the monster in a list under the room 
    //Is usefull for later when disabling their movement when a room is not activated
    //also handy if you have to unlock the room after defeating all the monsters 
    public List<GameObject> monsterlist;
}
