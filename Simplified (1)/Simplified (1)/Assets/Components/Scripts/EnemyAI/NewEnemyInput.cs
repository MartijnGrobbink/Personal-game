using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemyInput : MonoBehaviour
{
    new Rigidbody2D rigidbody;
    public Vector3 EnemyDirection;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var localVelocity = transform.InverseTransformDirection(rigidbody.velocity);

        if(localVelocity.x > 0){
            EnemyDirection = Vector3.right;
        }
        if(localVelocity.x < 0){
            EnemyDirection = Vector3.left;
        }
        if(localVelocity.y > 0){
            EnemyDirection = Vector3.up;
        }
        if(localVelocity.y < 0){
            EnemyDirection = Vector3.down;
        }
    }
}
