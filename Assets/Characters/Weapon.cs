﻿using UnityEngine;
using UnityEngine.Networking;

class Weapon : NetworkBehaviour
{
    [SerializeField] Bullet bullet;

    [Command]
    public void CmdSpawnBullet(Vector3 startingPosition, Vector3 direction, NetworkIdentity networkIdentityToIgnore)
    {
        GameObject bulletObject = Instantiate(bullet.gameObject) as GameObject;
        bulletObject.transform.SetParent(null);
        bulletObject.transform.position = startingPosition;

        NetworkServer.Spawn(bulletObject);

        RpcSetBulletDirection(bulletObject, direction, networkIdentityToIgnore);
    }

    [ClientRpc]
    void RpcSetBulletDirection(GameObject bulletObject, Vector3 direction, NetworkIdentity networkIdentityToIgnore)
    {
        Bullet newBullet = bulletObject.GetComponent<Bullet>();
        newBullet.Direction = direction.normalized;
        Collider col = networkIdentityToIgnore.GetComponent<Collider>();
        if (GetComponent<Collider>())
            Physics.IgnoreCollision(col, bulletObject.GetComponent<Collider>());
    }
}