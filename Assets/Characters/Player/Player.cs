using System;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    [SerializeField] Bullet bullet;
    [SerializeField] GameObject model;
    [SerializeField] float speed = 5f;

    WeaponHandler weaponHandler;

    NetworkIdentity networkIdentity;

    Rigidbody rb;
    Collider col;

    private void Start()
    {
        networkIdentity = GetComponent<NetworkIdentity>();
        weaponHandler = GetComponent<WeaponHandler>();

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
                weaponHandler.FireCurrentWeapon(direction, networkIdentity);
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
}
