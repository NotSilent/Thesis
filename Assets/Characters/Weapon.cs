using System;
using UnityEngine;
using UnityEngine.Networking;

public class Weapon : NetworkBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] [SyncVar] float damage = 1f;

    public virtual void Fire(Vector3 startingPosition, Vector3 target, NetworkIdentity networkIdentityToIgnore)
    {
        CmdSpawnBullet(startingPosition, target - startingPosition, networkIdentityToIgnore);
    }

    [Command]
    protected void CmdSpawnBullet(Vector3 startingPosition, Vector3 direction, NetworkIdentity networkIdentityToIgnore)
    {
        GameObject bulletObject = Instantiate(bullet.gameObject) as GameObject;
        bulletObject.transform.SetParent(null);
        bulletObject.transform.position = startingPosition;

        NetworkServer.Spawn(bulletObject);

        RpcInitBullet(bulletObject, direction, networkIdentityToIgnore);
    }

    [ClientRpc]
    void RpcInitBullet(GameObject bulletObject, Vector3 direction, NetworkIdentity networkIdentityToIgnore)
    {
        Bullet newBullet = bulletObject.GetComponent<Bullet>();
        newBullet.Direction = direction.normalized;
        newBullet.Damage = damage;
        Collider col = networkIdentityToIgnore.GetComponent<Collider>();
        if (GetComponent<Collider>())
            Physics.IgnoreCollision(col, bulletObject.GetComponent<Collider>());
    }

    public void IncreaseBulletDamage()
    {
        damage++;
    }
}