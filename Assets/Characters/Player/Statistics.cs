using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Statistics : NetworkBehaviour
{
    [SerializeField] Levels levels;

    UiManager uiManager;

    void Start()
    {
        uiManager = FindObjectOfType<UiManager>();
    }

    float experience = 0f;
    int currentLevel = 0;
    int currentPointsToSpend = 0;

    public void AddExperience(float experience)
    {
        if (isServer)
            RpcAddExperience(experience);
    }

    [ClientRpc]
    private void RpcAddExperience(float experience)
    {
        this.experience += experience;
        if (this.experience > levels.GetNextLevelRequirement(currentLevel))
        {
            InitiateLevelUp();
        }
    }

    void InitiateLevelUp()
    {
        currentLevel++;
        currentPointsToSpend++;

        if (isServer)
            CmdLevelUp();
    }

    [Command]
    public void CmdLevelUp()
    {
        RpcOpenUpgradeWindow();
    }

    [ClientRpc]
    void RpcOpenUpgradeWindow()
    {
        if (isLocalPlayer)
            OpenUpgradeWindow();
    }

    public int RemovePoint()
    {
        currentPointsToSpend--;
        return currentPointsToSpend;
    }

    void OpenUpgradeWindow()
    {
        if (uiManager)
            uiManager.upgradeWindow.Open();
    }
}
