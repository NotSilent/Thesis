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

    NetworkManager networkManager;

    private void Awake()
    {
        networkManager = FindObjectOfType<NetworkManager>();
        networkManager.networkAddress = "localhost";
    }

    private void Start()
    {
        networkManager.maxConnections = 2;
    }

    void OnEnable()
    {
        single.onClick.AddListener(StartSingle);
        host.onClick.AddListener(StartHost);
        client.onClick.AddListener(StartClient);
    }

    void OnDisable()
    {
        host.onClick.RemoveAllListeners();
        client.onClick.RemoveAllListeners();
    }

    void StartSingle()
    {

    }

    void StartHost()
    {
        networkManager.StartHost();
    }

    void StartClient()
    {
        networkManager.StartClient();
    }
}
