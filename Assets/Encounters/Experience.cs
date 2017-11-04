using UnityEngine;
using UnityEngine.Networking;

class Experience : NetworkBehaviour
{
    [SerializeField] float speed;

    GameObject target;
    Rigidbody rb;
    
    void FixedUpdate()
    {
        if (target)
        {
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        Vector3 toDirection = target.transform.position - transform.position;
        Vector3 leveledDirection = new Vector3(toDirection.x, 0, toDirection.z);
        rb.velocity = leveledDirection * speed;
    }

    public void Init(GameObject target, Vector3 startingPosition)
    {
        this.target = target;
        transform.position = startingPosition + Vector3.up;

        rb = GetComponent<Rigidbody>();
        transform.forward = Vector3.up;
    }
    
    void OnCollisionEnter(Collision collision)
    {
        CmdOnCollision();
    }
    
    [Command]
    void CmdOnCollision()
    {
        RpcOnCollision();
    }

    [ClientRpc]
    void RpcOnCollision()
    {
        Destroy(gameObject);
    }
}