using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class CustomNetworkManager : NetworkManager
{
    void OnPlayerDisconnected(UnityEngine.NetworkPlayer player)
    {
        //Network.CloseConnection(Network.player, true);
        //SceneManager.LoadScene("Menu");
    }

    public override void OnStopHost()
    {
        base.OnStopHost();
    }
}
