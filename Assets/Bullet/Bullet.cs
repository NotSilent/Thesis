using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(SphereCollider))]
public class Bullet : NetworkBehaviour
{
    [SerializeField] float speed = 10;
    [SerializeField] float destroyAfter = 5f;
    [SerializeField] float damage = 5f;

    private Vector3 direction;
    public Vector3 Direction
    {
        get
        {
            return direction;
        }
        set
        {
            direction = new Vector3(value.x, 0, value.z);
        }
    }

    void Start()
    {
        Destroy(gameObject, destroyAfter);
    }

    void Update()
    {
        transform.position += Direction * speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isServer)
            RpcOnCollisionEnter(collision.gameObject.GetComponent<NetworkIdentity>());
    }

    [Command]
    void CmdOnCollisionEnter(NetworkIdentity collision)
    {
        RpcOnCollisionEnter(collision);
    }

    [ClientRpc]
    void RpcOnCollisionEnter(NetworkIdentity collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
            NetworkServer.Destroy(gameObject);
        }
    }
}
