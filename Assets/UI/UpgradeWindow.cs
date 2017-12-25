using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UpgradeWindow : NetworkBehaviour
{
    [SerializeField] Button damage;
    [SerializeField] Button bullet;

    void OnEnable()
    {
        damage.onClick.AddListener(OnDamageClicked);
        bullet.onClick.AddListener(OnBulletClicked);
    }

    void OnDisable()
    {
        damage.onClick.RemoveAllListeners();
        bullet.onClick.RemoveAllListeners();
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    void Close()
    {
        gameObject.SetActive(false);
    }

    void OnDamageClicked()
    {
        Player[] players = FindObjectsOfType<Player>();

        foreach (Player player in players)
        {
            if (player.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                player.GetComponent<Weapon>().IncreaseBulletDamage();
                int points = player.GetComponent<Statistics>().RemovePoint();
                if (points <= 0)
                {
                    Close();
                }
                break;
            }
        }
    }

    void OnBulletClicked()
    {
        Player[] players = FindObjectsOfType<Player>();

        foreach (Player player in players)
        {
            if (player.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                player.GetComponent<OmniDirectionalWeapon>().IncreaseBullets();
                int points = player.GetComponent<Statistics>().RemovePoint();
                if (points <= 0)
                {
                    Close();
                }
                break;
            }
        }
    }
}