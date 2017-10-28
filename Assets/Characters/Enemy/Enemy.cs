using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Enemy : NetworkBehaviour
{
    [SerializeField] float speed = 5;

    Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        RandomWalk();
    }

    void RandomWalk()
    {
        int direction = Random.Range(0, 3);
        switch (direction)
        {
            case 0:
                rb.velocity = Vector3.forward * speed;
                break;

            case 1:
                rb.velocity = Vector3.back * speed;
                break;

            case 2:
                rb.velocity = Vector3.left * speed;
                break;

            case 3:
                rb.velocity = Vector3.right * speed;
                break;

            default:
                return;
        }
        rb.AddForce(Physics.gravity);
    }
}
