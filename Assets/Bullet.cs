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

    private SphereCollider sphereCollider;

    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();

        Destroy(gameObject, destroyAfter);
    }

    void Update()
    {
        transform.position += Direction * speed * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, sphereCollider.radius);
        if (colliders.Length > 0)
        {
            CollisionEvent(colliders);
        }
    }
    
    [Server]
    void CollisionEvent(Collider[] colliders)
    {
        foreach (Collider collider in colliders)
        {
            IDamageable damageable = collider.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.RpcTakeDamage(damage);
                NetworkServer.Destroy(gameObject);
            }
        }
    }
}
