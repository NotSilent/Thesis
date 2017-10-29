using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkGameManager : NetworkBehaviour
{
    static int currentPlayers;

    Area area;

    void Awake()
    {
        area = FindObjectOfType<Area>();
    }

    public void RegisterPlayer()
    {
        currentPlayers++;

        if (currentPlayers >= 2)
        {
            area.Init();
        }
    }
}
