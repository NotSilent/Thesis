using System;
using System.Collections;
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
        StartCoroutine(HealthRecovery(1f, 10f));
    }

    IEnumerator HealthRecovery(float amountRecovered, float timeBetweenRecoveries)
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenRecoveries);
            TakeDamage(-amountRecovered);
        }
    }

    public void RecoverHealth()
    {
        if (isServer)
            RpcTakeDamage(-maxHealth);
        EventOnDamageTaken(currentHealth, maxHealth);
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
        GetComponent<Player>().Disable();
    }
}
