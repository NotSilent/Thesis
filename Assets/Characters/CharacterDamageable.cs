using UnityEngine;
using UnityEngine.Networking;

public class CharacterDamageable : NetworkBehaviour, IDamageable
{
    public delegate void NotifyOnDamageTaken(float currentHealth, float maxHealth);
    
    public event NotifyOnDamageTaken EventOnDamageTaken;

    [SerializeField] float maxHealth = 100f;
    float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (isServer)
            RpcTakeDamage(damage);
        EventOnDamageTaken(currentHealth, maxHealth);
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
