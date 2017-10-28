using System;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    [SerializeField] Bullet bullet;
    [SerializeField] GameObject model;
    [SerializeField] float speed = 5f;

    Rigidbody rb;
    Collider col;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        if (!isLocalPlayer)
        {
            MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer meshRenderer in meshRenderers)
                meshRenderer.material.color = Color.red;
        }
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        ProcessInput();

        RaycastHit raycastHit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out raycastHit, 100f, ~0, QueryTriggerInteraction.Ignore))
        {
            model.transform.LookAt(new Vector3(raycastHit.point.x, transform.position.y, raycastHit.point.z));
        }
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;
        ProcessMovement();
    }

    void ProcessInput()
    {
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

    void ProcessMovement()
    {
        int w = Convert.ToInt32(Input.GetKey(KeyCode.W));
        int s = Convert.ToInt32(Input.GetKey(KeyCode.S));
        int a = Convert.ToInt32(Input.GetKey(KeyCode.A));
        int d = Convert.ToInt32(Input.GetKey(KeyCode.D));

        int horizontal = d - a;
        int vertical = w - s;

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        rb.velocity = direction * speed + Physics.gravity;
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
        Physics.IgnoreCollision(col, bulletObject.GetComponent<Collider>());
    }
}
