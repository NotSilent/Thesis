using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalPlayerHealth : MonoBehaviour
{
    [SerializeField]
    Slider slider;

    CharacterDamageable health;

    public void Init(CharacterDamageable health)
    {
        this.health = health;
        health.EventOnDamageTaken -= Health_EventOnDamageTaken;
        health.EventOnDamageTaken += Health_EventOnDamageTaken;
    }

    private void OnEnable()
    {
        if (health)
            health.EventOnDamageTaken += Health_EventOnDamageTaken;
    }

    private void OnDisable()
    {
        if (health)
            health.EventOnDamageTaken -= Health_EventOnDamageTaken;
    }

    private void Health_EventOnDamageTaken(float currentHealth, float maxHealth)
    {
        float currentHealthAsPercentage = Mathf.Clamp(currentHealth / maxHealth, 0, 1);
        slider.value = currentHealthAsPercentage;
    }
}
