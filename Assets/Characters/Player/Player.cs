using System;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    [SerializeField] Bullet bullet;
    [SerializeField] float speed = 5f;

    CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (!isLocalPlayer)
        {
            MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer meshRenderer in meshRenderers)
                meshRenderer.material.color = Color.red;
        }
    }

    private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        characterController.SimpleMove(Vector3.down * Physics.gravity.y * Time.deltaTime);
        ProcessInput();
    }

    private void ProcessInput()
    {
        ProcessMovement();

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit, 100f, ~0, QueryTriggerInteraction.Ignore))
            {
                Vector3 direction = raycastHit.point - transform.position;
                CmdShoot(direction);
            }
        }
    }

    private void ProcessMovement()
    {
        float horizontal = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        Vector3 direction = new Vector3(horizontal, 0, vertical);

        characterController?.SimpleMove(direction);
    }

    [Command]
    void CmdShoot(Vector3 direction)
    {
        GameObject bulletObject = Instantiate(bullet.gameObject) as GameObject;
        bulletObject.transform.SetParent(null);
        bulletObject.transform.position = transform.position;

        NetworkServer.Spawn(bulletObject);

        RpcSetBulletDirection(bulletObject, direction);
    }

    [ClientRpc]
    void RpcSetBulletDirection(GameObject bulletObject, Vector3 direction)
    {
        Bullet newBullet = bulletObject.GetComponent<Bullet>();
        newBullet.Direction = direction.normalized;
        Physics.IgnoreCollision(characterController, bulletObject.GetComponent<Collider>());
    }
}
