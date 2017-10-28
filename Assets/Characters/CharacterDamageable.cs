using UnityEngine;
using UnityEngine.Networking;

public class CharacterDamageable : NetworkBehaviour, IDamageable
{
    public delegate void NotifyOnDamageTaken(float currentHealth, float maxHealth);

    [SyncEvent]
    public event NotifyOnDamageTaken EventOnDamageTaken;

    [SerializeField] float maxHealth = 100f;
    float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    [Command]
    public void CmdTakeDamage(float damage)
    {
        EventOnDamageTaken(currentHealth, maxHealth);
        RpcTakeDamage(damage);
    }
    
    [ClientRpc]
    private void RpcTakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0f)
        {
            OnDied();
        }

        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
    }

    protected virtual void OnDied()
    {
        Destroy(gameObject);
    }
}
