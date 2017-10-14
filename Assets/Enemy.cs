using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(CharacterController))]
public class Enemy : NetworkBehaviour
{
    [SerializeField] float speed = 100;

    CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        characterController.SimpleMove(Vector3.down * Physics.gravity.y * Time.deltaTime);
        RandomWalk();
    }

    void RandomWalk()
    {
        int direction = Random.Range(0, 3);
        switch (direction)
        {
            case 0:
                characterController.SimpleMove(Vector3.forward * speed * Time.deltaTime);
                break;

            case 1:
                characterController.SimpleMove(Vector3.back * speed * Time.deltaTime);
                break;

            case 2:
                characterController.SimpleMove(Vector3.left * speed * Time.deltaTime);
                break;

            case 3:
                characterController.SimpleMove(Vector3.right * speed * Time.deltaTime);
                break;

            default:
                return;
        }
    }
}
