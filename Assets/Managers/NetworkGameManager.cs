using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkGameManager : NetworkBehaviour
{
    int currentPlayers;

    Area area;

    void Awake()
    {
        area = FindObjectOfType<Area>();
    }

    [ClientRpc]
    public void RpcRegisterPlayer()
    {
        currentPlayers++;

        if (currentPlayers >= 2)
        {
            area.Init();
        }
    }
}
