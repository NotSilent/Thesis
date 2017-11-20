using System;
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
        currentPlayers = 0;
        area = FindObjectOfType<Area>();
    }

    [ClientRpc]
    public void RpcRegisterPlayer()
    {
        currentPlayers++;

        if (currentPlayers >= 2)
        {
            StartCoroutine(Init(1f));
        }
    }

    IEnumerator Init(float timeToStart)
    {
        yield return new WaitForSeconds(timeToStart);
        area.Init();

    }
}
