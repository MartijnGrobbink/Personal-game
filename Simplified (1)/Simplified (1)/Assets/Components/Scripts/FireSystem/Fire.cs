using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    GameObject fire;
    public GameObject firepartical;

    public bool FireContact = false;
    private bool damageTilesSpawned = false;
    public bool partical = false;

    public float fireTime;

    public float health = 10;

    public float damage = 5;
    public int damageRadius;

    // Update is called once per frame
    void Update()
    {
        if(FireContact == true || health <= 0)
        {
            gameObject.tag = "OnFire";
            if(partical == false)
            {
                fire = Instantiate(firepartical, transform.position + new Vector3(1, 0, -2), transform.rotation);
                partical = true;
            }
            //if the object's time on fire reaches zero destory object with the damage tiles and animation
            if(fireTime <= 0)
            {
                Destroy(fire);
                Destroy(gameObject);
            }
            else
            {
                fireTime -= Time.deltaTime;//remove time from timer
            }

            if(damageTilesSpawned == false)
            {
                StartCoroutine(FireCheck());
                damageTilesSpawned = true;
            }
        }
    }

    public void DealDamage(float fireDamage)
    {
        health -= fireDamage;
    }

    private IEnumerator FireCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            Collider2D[] InRange = Physics2D.OverlapCircleAll(transform.position, damageRadius);
            for (int i = 0; i < InRange.Length; i++)
            {
                if (InRange[i].tag == "Flammable")
                {
                    Fire fire = InRange[i].gameObject.GetComponent<Fire>();
                    fire.DealDamage(damage);
                }
                else if (InRange[i].gameObject.tag == "Player" || InRange[i].gameObject.tag == "Zombie")
                {
                    NewHealth newHealth = InRange[i].gameObject.GetComponent<NewHealth>();
                    newHealth.damage = damage;
                }
            }
        }
    }
}
