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

    private void Start()
    {
        networkIdentity = GetComponent<NetworkIdentity>();
        weaponHandler = GetComponent<WeaponHandler>();

        NetworkGameManager networkGameManager = FindObjectOfType<NetworkGameManager>();
        if (networkGameManager && isServer)
        {
            networkGameManager.RpcRegisterPlayer();
        }

        rb = GetComponent<Rigidbody>();
        Disable();

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
            InitPlayerStartingPosition();

            GameMenu gameMenu = FindObjectOfType<GameMenu>();
            gameMenu.disconnect.onClick.AddListener(Disconnect);

            gameMenu.disconnect.gameObject.SetActive(false);
        }
    }

    void InitPlayerStartingPosition()
    {
        float x = UnityEngine.Random.Range(50f, 100f);
        float z = UnityEngine.Random.Range(50f, 100f);
        int minus = UnityEngine.Random.Range(0, 1);
        if (minus == 1)
        {
            x *= -1;
            z *= -1;
        }
        transform.position = new Vector3(x, 0, z);
    }

    public void Enable()
    {
        foreach (MeshRenderer meshRenderer in model.GetComponentsInChildren<MeshRenderer>())
        {
            meshRenderer.enabled = true;
        }
        this.enabled = true;
    }

    public void Disable()
    {
        return;
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

    void Disconnect()
    {
        if (isServer)
        {
            RpcDisconnect();
        }
        else
        {
            CmdDisconnect();
        }
    }

    [Command]
    void CmdDisconnect()
    {
        RpcDisconnect();
    }

    [ClientRpc]
    void RpcDisconnect()
    {
        Network.Disconnect();
        NetworkManager networkManager = FindObjectOfType<NetworkManager>();
        if (isServer)
        {
            networkManager.StopHost();
        }
        else
        {
            networkManager.StopClient();
        }

    }
}

