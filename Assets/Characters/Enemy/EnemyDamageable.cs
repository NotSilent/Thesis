using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyDamageable : NetworkBehaviour, IDamageable
{
    [SerializeField] float maxHealth = 100f;
    float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    [ClientRpc]
    public void RpcTakeDamage(float damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0f)
        {
            OnDied();
        }

        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
    }

    private void OnDied()
    {
        Destroy(gameObject);
    }
}
