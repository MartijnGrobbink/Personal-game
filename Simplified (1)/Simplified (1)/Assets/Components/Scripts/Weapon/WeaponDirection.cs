using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDirection : MonoBehaviour
{
    public GameObject player;
    public enum Direction { None, Up, Down, Left, Right }
    public Direction direction = Direction.None;

    //checking what direction the mouse is to the player
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.transform.position;
        if(mousePos.y < 0)
        {
            if(mousePos.x < 0)
            {
                if(mousePos.y < mousePos.x)
                {
                    direction = Direction.Down;
                }
                else
                {
                    direction = Direction.Left;
                }
            }

            else
            {
                if(mousePos.y * -1 > mousePos.x)
                {
                    direction = Direction.Down;
                } 
                else
                {
                    direction = Direction.Right;
                }
            } 
        } 

        else
        {
            if(mousePos.x < 0)
            {
                if(mousePos.y > mousePos.x * -1)
                {
                    direction = Direction.Up;
                }
                else
                {
                    direction = Direction.Left;
                }
            }
            
            else
            {
                if(mousePos.y > mousePos.x)
                {
                    direction = Direction.Up;
                }
                else
                {
                    direction = Direction.Right;
                }
            }
        }
    }
}
