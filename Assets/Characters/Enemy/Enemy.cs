using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Enemy : NetworkBehaviour
{
    [SerializeField] float speed = 5;

    Rigidbody rb;

    int direction = 0;

    float timeToChangeDirection = 1f;
    float currentTimeToChangeDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentTimeToChangeDirection = timeToChangeDirection;
    }

    void Update()
    {
        if (!isServer)
            return;

        currentTimeToChangeDirection += Time.deltaTime;
        if (currentTimeToChangeDirection > timeToChangeDirection)
        {
            int direction = UnityEngine.Random.Range(0, 3);
            RpcSetDirection(direction);
            currentTimeToChangeDirection = 0;
        }
    }

    void FixedUpdate()
    {
        RandomWalk(direction);
    }

    [ClientRpc]
    void RpcSetDirection(int direction)
    {
        this.direction = direction;
    }

    void RandomWalk(int direction)
    {
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
