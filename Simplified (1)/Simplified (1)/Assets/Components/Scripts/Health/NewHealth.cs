using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewHealth : MonoBehaviour
{
    //source
    //https://www.youtube.com/watch?v=BLfNP4Sc_iA

    public float maxhealth = 100;
    public float health;
    //to get the damage and set the invinsability timer
    public float damage;
    public float invTimer;
    //to get the health bar to update it
    public HealthBar healthBar;
    //setting health to max health and setting the max health of healthbar
    void Start()
    {
        health = maxhealth;
        healthBar.SetMaxHealth(maxhealth);
    }

    void Update()
    {
        //if not invinsable do damage
        if(invTimer <= 0)
        {
            if(damage > 0)
            {
                //if damage recived remove some health set the invinsability timer and update health bar
                health -= damage;
                invTimer = 1f;
                healthBar.SetHealth(health);
            }
        }
        else 
        {
            invTimer -= Time.deltaTime;
            damage = 0;
        }
        //players death is already in player input
        if(this.gameObject.tag != "Player" && health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
