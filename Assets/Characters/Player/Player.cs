using System;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    [SerializeField] Bullet bullet;
    [SerializeField] GameObject model;
    [SerializeField] float speed = 5f;

    [SerializeField]
    LocalPlayerHealth localPlayerHealth;

    WeaponHandler weaponHandler;

    NetworkIdentity networkIdentity;

    Rigidbody rb;

    private void Awake()
    {
        //if (FindObjectOfType<NetworkGameManager>())
        //{
        //    FindObjectOfType<NetworkGameManager>().RpcRegisterPlayer();
        //}

    }

    private void Start()
    {
        networkIdentity = GetComponent<NetworkIdentity>();
        weaponHandler = GetComponent<WeaponHandler>();

        rb = GetComponent<Rigidbody>();

        if (!isLocalPlayer)
        {
            MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer meshRenderer in meshRenderers)
                meshRenderer.material.color = Color.red;
        }
        else
        {
            GetComponentInChildren<HealthIndicator>().gameObject.SetActive(false);
            GameObject playerHealthUI = Instantiate(localPlayerHealth.gameObject) as GameObject;
            playerHealthUI.GetComponent<LocalPlayerHealth>().Init(GetComponent<CharacterDamageable>());
            Debug.Log("ASDASDASD");
        }
    }

    public void Disable()
    {
        rb.velocity = Vector3.zero;
        foreach (MeshRenderer meshRenderer in model.GetComponentsInChildren<MeshRenderer>())
        {
            meshRenderer.enabled = false;
        }
        this.enabled = false;
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        RaycastHit raycastHit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out raycastHit, 100f, ~0, QueryTriggerInteraction.Ignore))
        {
            model.transform.LookAt(new Vector3(raycastHit.point.x, transform.position.y, raycastHit.point.z));
            ProcessInput(raycastHit.point);
        }

    }

    void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;

        ProcessMovement();
    }

    void ProcessInput(Vector3 target)
    {
        if (Input.GetMouseButtonDown(0))
        {
            weaponHandler.FireCurrentWeapon(target, networkIdentity);
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
