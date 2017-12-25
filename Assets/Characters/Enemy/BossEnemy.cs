using UnityEngine;
using UnityEngine.Networking;

public class BossEnemy : Enemy
{
    Vector3 startingPosition;

    protected override void Start()
    {
        base.Start();
        startingPosition = transform.position;
    }

    protected override void MoveTowards(Player currentTarget, float multiplier = 1f)
    {
        if (currentTarget && Vector3.Distance(startingPosition, transform.position) < 20)
        {
            rb.velocity = Quaternion.AngleAxis(angle, Vector3.up) * (currentTarget.gameObject.transform.position - transform.position).normalized * speed * multiplier;
        }
        else
        {
            rb.velocity = (startingPosition - transform.position).normalized * speed * multiplier;
        }
    }

    void OnDestroy()
    {
        FindObjectOfType<NetworkGameManager>().InitializeNextLevel();   
    }
}
