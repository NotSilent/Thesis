using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] Button single;
    [SerializeField] Button host;
    [SerializeField] Button client;
    [SerializeField] InputField text;

    NetworkManager networkManager;
    
    private void Start()
    {
        networkManager = FindObjectOfType<NetworkManager>();
        if (networkManager)
            networkManager.networkAddress = "localhost";

        networkManager.maxConnections = 2;
    }

    void OnEnable()
    {
        single.onClick.AddListener(StartSingle);
        host.onClick.AddListener(StartHost);
        client.onClick.AddListener(StartClient);
        text.onEndEdit.AddListener(OnIpEdit);
    }

    void OnDisable()
    {
        single.onClick.RemoveAllListeners();
        host.onClick.RemoveAllListeners();
        client.onClick.RemoveAllListeners();
        text.onEndEdit.RemoveAllListeners();
    }

    void StartSingle()
    {
        networkManager.onlineScene = "Single";
        networkManager.StartHost();
    }

    void StartHost()
    {
        networkManager.onlineScene = "Multi";
        networkManager.StartHost();
    }

    void StartClient()
    {
        networkManager.onlineScene = "Multi";
        networkManager.StartClient();
    }

    void OnIpEdit(string text)
    {
        networkManager.networkAddress = text;
    }
}
