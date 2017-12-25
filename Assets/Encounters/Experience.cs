using UnityEngine;
using UnityEngine.Networking;

class Experience : NetworkBehaviour
{
    [SerializeField] float speed;

    GameObject target;
    Rigidbody rb;

    float experiencePerSpawn;

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
        rb.velocity = leveledDirection.normalized * speed;
    }

    public void Init(GameObject target, Vector3 startingPosition, float experiencePerSpawn)
    {
        this.target = target;
        this.experiencePerSpawn = experiencePerSpawn;

        transform.position = startingPosition + Vector3.up;

        rb = GetComponent<Rigidbody>();
        transform.forward = Vector3.up;
    }

    void OnTriggerEnter(Collider other)
    {
        if (isServer)
            RpcOnCollision(other.gameObject.GetComponent<NetworkIdentity>());
    }

    [ClientRpc]
    void RpcOnCollision(NetworkIdentity networkIdentity)
    {
        networkIdentity.GetComponent<Statistics>().AddExperience(experiencePerSpawn);
        Destroy(gameObject);
    }
}