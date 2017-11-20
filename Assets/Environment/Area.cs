using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Area : NetworkBehaviour
{
    [SerializeField] float time;

    Vector3 startingScale;

    bool isInitialized = false;

    void Start()
    {
        startingScale = transform.localScale;
    }

    void Update()
    {
        if (isInitialized && transform.localScale.x > 50)
            transform.localScale -= startingScale / time * Time.deltaTime;
    }

    void OnTriggerExit(Collider other)
    {
        NetworkIdentity networkIdentity = other.gameObject.GetComponent<NetworkIdentity>();
        if (networkIdentity && isServer)
            RpcOnTriggerExit(networkIdentity);
    }

    [ClientRpc]
    void RpcOnTriggerExit(NetworkIdentity other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player)
        {
            player.Disable();
        }
        else
        {
            Destroy(other.gameObject);
        }
    }

    public void Init()
    {
        if (isServer)
        {
            RpcInit();
            RpcEnablePlayers();
        }
    }

    [ClientRpc]
    private void RpcEnablePlayers()
    {
        Player[] players = FindObjectsOfType<Player>();
        foreach (Player player in players)
        {
            player.Enable();
        }
    }

    [ClientRpc]
    private void RpcInit()
    {
        isInitialized = true;
    }
}
