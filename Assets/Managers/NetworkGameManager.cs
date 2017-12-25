using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkGameManager : NetworkBehaviour
{
    int currentPlayers;
    int currentLevel;

    Area area;
    EnemySpawner enemySpawner;

    void Start()
    {
        currentLevel = 0;
        currentPlayers = 0;
        area = FindObjectOfType<Area>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    [ClientRpc]
    public void RpcRegisterPlayer()
    {
        currentPlayers++;

        if (currentPlayers >= area.playersToStart)
        {
            StartCoroutine(Init(1f));
        }
    }

    IEnumerator Init(float timeToStart)
    {
        area.Init();

        yield return new WaitForSeconds(timeToStart);
        if (enemySpawner)
        {
            enemySpawner.isInitialized = true;
            enemySpawner.SpawnWave(currentLevel);
        }
    }

    public void InitializeNextLevel()
    {
        currentLevel++;
        StartCoroutine(Init(1f));
        InitPlayers();

        EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
        if (enemySpawner)
        {
            enemySpawner.DeleteAllEnemies();
            enemySpawner.SpawnWave(currentLevel);
        }
    }

    private static void InitPlayers()
    {
        Player[] players = FindObjectsOfType<Player>();
        foreach (Player player in players)
        {
            player.ReInit();
        }
    }
}
