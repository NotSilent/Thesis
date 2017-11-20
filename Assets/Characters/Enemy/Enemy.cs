using UnityEngine;
using UnityEngine.Networking;

public class Enemy : NetworkBehaviour
{
    public enum AiState
    {
        Patrol,
        Attack,
    }

    public class AiStateParameters
    {
        public NetworkIdentity currentTarget;
        public float angle;
    }

    [SerializeField] float speed = 5f;
    [SerializeField] float patrolSize = 50f;
    [SerializeField] float timeBetweenShoots = 2f;
    [SerializeField] AnimationCurve playerAproachCurve;

    float currentTimeBetweenShoots;
    float timeBetweenStateChange = 1f;
    float currentTimeBetweenStateChange;

    Rigidbody rb;

    AiState currentState;
    Player currentTarget;
    Weapon weapon;

    float angle;

    void Start()
    {
        angle = 0f;
        weapon = GetComponent<Weapon>();

        currentState = AiState.Patrol;
        currentTarget = null;

        rb = GetComponent<Rigidbody>();
        currentTimeBetweenShoots = timeBetweenShoots;
        currentTimeBetweenStateChange = timeBetweenStateChange;

        if (isServer)
            TryToChangeState();
    }

    void Update()
    {
        if (!isServer)
            return;

        currentTimeBetweenStateChange += Time.deltaTime;
        if (currentTimeBetweenStateChange > timeBetweenStateChange)
        {
            TryToChangeState();
            currentTimeBetweenStateChange = ((Random.value * 2) - 1) * timeBetweenStateChange * 0.1f;
        }

        switch (currentState)
        {
            case AiState.Patrol:
                break;

            case AiState.Attack:
                currentTimeBetweenShoots += Time.deltaTime;
                if (currentTimeBetweenShoots > timeBetweenShoots)
                {
                    Shoot(currentTarget);
                    currentTimeBetweenShoots = 0;
                }
                break;
        }
    }

    void Shoot(Player currentTarget)
    {
        weapon.Fire(transform.position, currentTarget.transform.position, GetComponent<NetworkIdentity>());
    }

    void FixedUpdate()
    {
        switch (currentState)
        {
            case AiState.Patrol:
                break;

            case AiState.Attack:
                MoveTowards(currentTarget);
                break;
        }
    }

    [Server]
    private void TryToChangeState()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, patrolSize, 1 << LayerMask.NameToLayer("Player"));
        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                Player player = collider.GetComponent<Player>();

                if (player)
                {
                    RpcChangeState(AiState.Attack, new AiStateParameters
                    {
                        currentTarget = player.gameObject.GetComponent<NetworkIdentity>(),
                        angle = playerAproachCurve.Evaluate(Random.value)
                    });
                    return;
                }
            }
        }
    }

    [ClientRpc]
    void RpcChangeState(AiState state, AiStateParameters aiStateParameters)
    {
        currentState = state;
        switch (state)
        {
            case AiState.Attack:
                currentTarget = aiStateParameters.currentTarget.GetComponent<Player>();
                angle = aiStateParameters.angle;
                break;
        }
    }

    void MoveTowards(Player currentTarget)
    {
        rb.velocity = Quaternion.AngleAxis(angle, Vector3.up) * (currentTarget.gameObject.transform.position - transform.position).normalized * speed;
    }
}
