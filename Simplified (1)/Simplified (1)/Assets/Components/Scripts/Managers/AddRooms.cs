using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRooms : MonoBehaviour
{
private RoomTemplates templates;
void Start(){
    //add the room to templates "rooms" list
    templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
    templates.rooms.Add(this.gameObject);
    }
}