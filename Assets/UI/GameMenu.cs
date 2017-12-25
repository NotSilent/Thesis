using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : NetworkBehaviour
{
    public Button disconnect;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            disconnect.gameObject.SetActive(!disconnect.gameObject.activeInHierarchy);
        }
    }
}
